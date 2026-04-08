using Microsoft.EntityFrameworkCore;
using Webshop.Domain.Entitites;
using Webshop.Domain.Interfaces;
using Webshop.Infrastructure.EF;

namespace Webshop.Infrastructure.Repositories;

public class KundRepository(WebshopDbContext db) : IKundRepository
{
    public async Task<IEnumerable<Kund>> GetAllAsync()
        => await db.Kunder.ToListAsync();

    public async Task<Kund?> GetByIdAsync(Guid id)
        => await db.Kunder
            .Include(k => k.Ordrar)
                .ThenInclude(o => o.ProduktOrdrar)
                    .ThenInclude(po => po.Produkt)
            .FirstOrDefaultAsync(k => k.Id == id);

    public async Task<IEnumerable<Kund>> GetBySearchAsync(string search)
        => await db.Kunder
            .Where(k => k.Namn.Contains(search) ||
                        k.Epost.Contains(search) ||
                        k.Stad.Contains(search))
            .ToListAsync();

    public async Task AddAsync(Kund kund)
    {
        await db.Kunder.AddAsync(kund);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Kund kund)
    {
        db.Kunder.Update(kund);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var kund = await db.Kunder.FindAsync(id);
        if (kund is null) return;

        db.Kunder.Remove(kund);
        await db.SaveChangesAsync();
    }
}
