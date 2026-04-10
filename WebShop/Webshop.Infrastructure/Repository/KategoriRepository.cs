using Microsoft.EntityFrameworkCore;
using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;
using Webshop.Infrastructure.EF;

namespace Webshop.Infrastructure.Repositories;

public class KategoriRepository(WebshopDbContext db) : IKategoriRepository
{
    public async Task<IEnumerable<Kategori>> GetAllAsync()
        => await db.Kategorier.ToListAsync();

    public async Task<Kategori?> GetByIdAsync(Guid id)
        => await db.Kategorier
            .Include(k => k.Produkter)
            .FirstOrDefaultAsync(k => k.Id == id);

    public async Task<Kategori?> GetMostPopularCategoryAsync()
     => await db.Kategorier
         .OrderByDescending(k => k.Produkter
             .Sum(p => p.ProduktOrdrar != null
                 ? p.ProduktOrdrar.Sum(po => po.Antal)
                 : 0))
         .FirstOrDefaultAsync();


    public async Task AddAsync(Kategori kategori)
    {
        await db.Kategorier.AddAsync(kategori);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Kategori kategori)
    {
        db.Kategorier.Update(kategori);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var kategori = await db.Kategorier.FindAsync(id);
        if (kategori is null) return;

        db.Kategorier.Remove(kategori);
        await db.SaveChangesAsync();
    }
}
