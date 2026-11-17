using FranciscoJoel_AP1_P2.Context;
using FranciscoJoel_AP1_P2.Models;
using Microsoft.EntityFrameworkCore;

namespace FranciscoJoel_AP1_P2.Services;

public class ProductosService(IDbContextFactory<Contexto> contextFactory)
{
    public async Task<List<Productos>> Listar()
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Productos
            .AsNoTracking()
            .OrderBy(p => p.ProductoId)
            .ToListAsync();
    }
}
