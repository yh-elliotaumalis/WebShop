using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;
using Webshop.Domain.Interfaces;

namespace Webshop.Application.Services;

public class ProduktService(IProduktRepository repo) : IProduktService
{
    public async Task<IEnumerable<Produkt>> GetAllAsync()
        => await repo.GetAllAsync();

    public async Task<Produkt?> GetByIdAsync(Guid id)
        => await repo.GetByIdAsync(id);

    public async Task<IEnumerable<Produkt>> GetByCategoryAsync(Guid kategoriId)
        => await repo.GetByCategoryAsync(kategoriId);

    public async Task<IEnumerable<Produkt>> GetBySearchAsync(string search)
        => await repo.GetBySearchAsync(search);

    public async Task<IEnumerable<Produkt>> GetFeaturedAsync()
        => await repo.GetFeaturedAsync();

    public async Task<IEnumerable<Produkt>> GetBestSellersAsync(int antal)
        => await repo.GetBestSellersAsync(antal);

    public async Task AddAsync(Produkt produkt)
        => await repo.AddAsync(produkt);

    public async Task UpdateAsync(Produkt produkt)
        => await repo.UpdateAsync(produkt);

    public async Task DeleteAsync(Guid id)
        => await repo.DeleteAsync(id);
}
