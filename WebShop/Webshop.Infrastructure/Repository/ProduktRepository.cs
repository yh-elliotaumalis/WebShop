using Microsoft.EntityFrameworkCore;
using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;
using Webshop.Infrastructure.EF;

namespace Webshop.Infrastructure.Repositories;

public class ProduktRepository(WebshopDbContext db) : IProduktRepository
{
    private IQueryable<Produkt> GetProdukterWithIncludes()
        => db.Produkter
            .Include(p => p.Kategori)
            .Include(p => p.Leverantör);

    public async Task<IEnumerable<Produkt>> GetAllAsync()
        => await GetProdukterWithIncludes()
            .AsNoTracking().ToListAsync();

    public async Task<Produkt?> GetByIdAsync(Guid id)
        => await GetProdukterWithIncludes()
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Produkt>> GetByCategoryAsync(Guid categoryId)
        => await GetProdukterWithIncludes()
            .Where(p => p.KategoriId == categoryId)
            .ToListAsync();
    public async Task<IEnumerable<Produkt>> GetFeaturedAsync()
    => await GetProdukterWithIncludes()
        .Where(p => p.ÄrUtvald)
        .ToListAsync();

    public async Task<IEnumerable<Produkt>> GetBestSellersAsync(int antal)
        => await GetProdukterWithIncludes()
            .OrderByDescending(p => p.ProduktOrdrar.Sum(po => po.Antal))
            .Take(antal)
            .ToListAsync();


    public async Task<IEnumerable<Produkt>> GetBySearchAsync(string search)
        => await GetProdukterWithIncludes()
            .Where(p => p.Namn.Contains(search) || p.Beskrivning.Contains(search))
            .ToListAsync();

    public async Task AddAsync(Produkt produkt)
    {
        await db.Produkter.AddAsync(produkt);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Produkt produkt)
    {
        db.Produkter.Update(produkt);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var produkt = await db.Produkter.FindAsync(id);
        if (produkt is null) return;

        db.Produkter.Remove(produkt);
        await db.SaveChangesAsync();
    }
}
