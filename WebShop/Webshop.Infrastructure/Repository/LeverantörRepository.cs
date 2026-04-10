using Microsoft.EntityFrameworkCore;
using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;
using Webshop.Domain.Interfaces;
using Webshop.Infrastructure.EF;

namespace Webshop.Infrastructure.Repositories;

public class LeverantörRepository(WebshopDbContext db) : ILeverantörRepository
{
    private IQueryable<Leverantör> GetLeverantörerWithIncludes()
        => db.Leverantörer
            .Include(l => l.Produkter);

    public async Task<IEnumerable<Leverantör>> GetAllAsync()
        => await GetLeverantörerWithIncludes()
            .ToListAsync();

    public async Task<Leverantör?> GetByIdAsync(Guid id)
        => await GetLeverantörerWithIncludes()
            .FirstOrDefaultAsync(l => l.Id == id);

    public async Task AddAsync(Leverantör leverantör)
    {
        await db.Leverantörer.AddAsync(leverantör);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Leverantör leverantör)
    {
        db.Leverantörer.Update(leverantör);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var leverantör = await db.Leverantörer.FindAsync(id);
        if (leverantör is null) return;
        db.Leverantörer.Remove(leverantör);
        await db.SaveChangesAsync();
    }
}
