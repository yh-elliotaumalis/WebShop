using Microsoft.EntityFrameworkCore;
using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;
using Webshop.Infrastructure.EF;

namespace Webshop.Infrastructure.Repositories;

public class KategoriRepository(WebshopDbContext db) : IKategoriRepository
{
    private IQueryable<Kategori> GetCategoriesWithIncludes()
        => db.Kategorier
            .Include(k => k.Produkter);
    public async Task<IEnumerable<Kategori>> GetAllAsync()
        => await GetCategoriesWithIncludes()
            .ToListAsync();

    public async Task<Kategori?> GetByIdAsync(Guid id)
        => await GetCategoriesWithIncludes()
            .FirstOrDefaultAsync(k => k.Id == id);

    public async Task<Kategori?> GetMostPopularCategoryAsync()
    {
        var kategoriId = await db.ProduktOrdrar
            .Include(p => p.Produkt)
            .GroupBy(p => p.Produkt.KategoriId)
            .OrderByDescending(g => g.Sum(p => p.Antal))
            .Select(g => g.Key)
            .FirstOrDefaultAsync();

        if (kategoriId == default) return null;

        return await db.Kategorier
            .Include(k => k.Produkter)
            .FirstOrDefaultAsync(k => k.Id == kategoriId);
    }


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
