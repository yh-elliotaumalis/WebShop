using Microsoft.EntityFrameworkCore;
using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;
using Webshop.Infrastructure.EF;

namespace Webshop.Infrastructure.Repositories;

public class FraktOmbudRepository(WebshopDbContext db) : IFraktOmbudRepository
{
    private IQueryable<FraktOmbud> GetFraktOmbudWithIncludes()
        => db.FraktOmbud
            .Include(f => f.Ordrar);

    public async Task<IEnumerable<FraktOmbud>> GetAllAsync()
        => await GetFraktOmbudWithIncludes()
            .ToListAsync();

    public async Task<FraktOmbud?> GetByIdAsync(Guid id)
        => await GetFraktOmbudWithIncludes()
            .FirstOrDefaultAsync(f => f.Id == id);

    public async Task AddAsync(FraktOmbud fraktOmbud)
    {
        await db.FraktOmbud.AddAsync(fraktOmbud);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(FraktOmbud fraktOmbud)
    {
        db.FraktOmbud.Update(fraktOmbud);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var fraktOmbud = await db.FraktOmbud.FindAsync(id);
        if (fraktOmbud is null) return;

        db.FraktOmbud.Remove(fraktOmbud);
        await db.SaveChangesAsync();
    }
}
