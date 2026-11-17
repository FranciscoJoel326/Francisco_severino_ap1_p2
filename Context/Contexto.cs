using Microsoft.EntityFrameworkCore;
using FranciscoJoel_AP1_P2.Models;

namespace FranciscoJoel_AP1_P2.Context;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options) { }

    public DbSet<Productos> Productos { get; set; } = null!;
    public DbSet<Ventas> Ventas { get; set; } = null!;
    public DbSet<VentasDetalle> VentasDetalle { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Productos>().HasData(
            new Productos { ProductoId = 1, Nombre = "Cafe Moka", Precio = 150m, Stock = 10 },
            new Productos { ProductoId = 2, Nombre = "Cafe Vainilla", Precio = 150m, Stock = 10 },
            new Productos { ProductoId = 3, Nombre = "Cafe Caramelo", Precio = 160m, Stock = 10 },
            new Productos { ProductoId = 4, Nombre = "Doble Espresso", Precio = 180m, Stock = 10 },
            new Productos { ProductoId = 5, Nombre = "Cold Brew", Precio = 200m, Stock = 10 }
        );

        modelBuilder.Entity<Ventas>()
            .HasMany(v => v.Detalle)
            .WithOne()
            .HasForeignKey(d => d.VentaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<VentasDetalle>()
            .HasOne(d => d.Producto)
            .WithMany()
            .HasForeignKey(d => d.ProductoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
