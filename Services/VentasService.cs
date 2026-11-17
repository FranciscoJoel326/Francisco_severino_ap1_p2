using Aplicada1.Core;
using FranciscoJoel_AP1_P2.Context;
using FranciscoJoel_AP1_P2.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FranciscoJoel_AP1_P2.Services;

public class VentasService(IDbContextFactory<Contexto> contextFactory) : IServiceResult<Ventas, int>
{
    public async Task<Result> Guardar(Ventas entidad)
    {
        if (entidad.Detalle.Count == 0)
        {
            return Fallo("Debe agregar al menos un producto al detalle de la venta.");
        }

        if (entidad.Detalle.Any(d => d.Cantidad <= 0))
        {
            return Fallo("La cantidad de cada producto debe ser mayor que cero.");
        }

        try
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            await using var transaccion = await contexto.Database.BeginTransactionAsync();

            var productos = await contexto.Productos.ToListAsync();

            var nuevoDetalle = entidad.Detalle
                .GroupBy(d => d.ProductoId)
                .Select(g => new VentasDetalle
                {
                    ProductoId = g.Key,
                    Cantidad = g.Sum(x => x.Cantidad)
                })
                .ToList();

            Ventas venta;

            if (entidad.VentaId > 0)
            {
                var existente = await contexto.Ventas.FirstOrDefaultAsync(v => v.VentaId == entidad.VentaId);

                if (existente is null)
                {
                    return Fallo("La venta que intenta modificar no existe.");
                }

                var detalleAnterior = await contexto.VentasDetalle
                    .Where(d => d.VentaId == entidad.VentaId)
                    .ToListAsync();

                foreach (var anterior in detalleAnterior)
                {
                    var producto = productos.FirstOrDefault(p => p.ProductoId == anterior.ProductoId);

                    if (producto is not null)
                    {
                        producto.Stock += anterior.Cantidad;
                    }
                }

                contexto.VentasDetalle.RemoveRange(detalleAnterior);

                existente.Fecha = entidad.Fecha;
                existente.Observacion = entidad.Observacion;
                venta = existente;
            }
            else
            {
                venta = new Ventas
                {
                    Fecha = entidad.Fecha,
                    Observacion = entidad.Observacion
                };
                contexto.Ventas.Add(venta);
            }

            foreach (var linea in nuevoDetalle)
            {
                var producto = productos.FirstOrDefault(p => p.ProductoId == linea.ProductoId);

                if (producto is null)
                {
                    return Fallo("Uno de los productos seleccionados no existe.");
                }

                if (producto.Stock < linea.Cantidad)
                {
                    return Fallo($"Stock insuficiente para {producto.Nombre}. Disponible: {producto.Stock}.");
                }

                producto.Stock -= linea.Cantidad;
                linea.Precio = producto.Precio;
                venta.Detalle.Add(linea);
            }

            venta.Total = nuevoDetalle.Sum(d => d.Cantidad * d.Precio);

            await contexto.SaveChangesAsync();
            await transaccion.CommitAsync();

            return Exito();
        }
        catch (Exception ex)
        {
            return Fallo(ex.InnerException?.Message ?? ex.Message);
        }
    }

    public async Task<Result<Ventas?>> Buscar(int id)
    {
        try
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();

            var venta = await contexto.Ventas
                .AsNoTracking()
                .Include(v => v.Detalle)
                .FirstOrDefaultAsync(v => v.VentaId == id);

            return Exito<Ventas?>(venta);
        }
        catch (Exception ex)
        {
            return Fallo<Ventas?>(ex.InnerException?.Message ?? ex.Message);
        }
    }

    public async Task<Result<List<Ventas>>> GetList(Expression<Func<Ventas, bool>> criterio)
    {
        try
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();

            var lista = await contexto.Ventas
                .AsNoTracking()
                .Where(criterio)
                .OrderByDescending(v => v.VentaId)
                .ToListAsync();

            return Exito(lista);
        }
        catch (Exception ex)
        {
            return Fallo<List<Ventas>>(ex.InnerException?.Message ?? ex.Message);
        }
    }

    public async Task<Result> Eliminar(int id)
    {
        try
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            await using var transaccion = await contexto.Database.BeginTransactionAsync();

            var venta = await contexto.Ventas
                .Include(v => v.Detalle)
                .FirstOrDefaultAsync(v => v.VentaId == id);

            if (venta is null)
            {
                return Fallo("La venta no existe.");
            }

            var productos = await contexto.Productos.ToListAsync();

            foreach (var linea in venta.Detalle)
            {
                var producto = productos.FirstOrDefault(p => p.ProductoId == linea.ProductoId);

                if (producto is not null)
                {
                    producto.Stock += linea.Cantidad;
                }
            }

            contexto.Ventas.Remove(venta);

            await contexto.SaveChangesAsync();
            await transaccion.CommitAsync();

            return Exito();
        }
        catch (Exception ex)
        {
            return Fallo(ex.InnerException?.Message ?? ex.Message);
        }
    }

    private static Result Exito() => Result.Success();

    private static Result<T> Exito<T>(T valor) => Result.Success(valor);

    private static Result Fallo(string mensaje) => Result.Failure(new Error("Ventas", mensaje));

    private static Result<T> Fallo<T>(string mensaje) => Result.Failure<T>(new Error("Ventas", mensaje));
}
