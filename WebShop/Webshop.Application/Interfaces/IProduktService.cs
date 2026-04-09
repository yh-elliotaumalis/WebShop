using Webshop.Domain.Entitites;

namespace Webshop.Application.Interfaces;

public interface IProduktService
{
    Task<IEnumerable<Produkt>> GetAllAsync();
    Task<Produkt?> GetByIdAsync(Guid id);
    Task<IEnumerable<Produkt>> GetBestSellersAsync(int antal);
    Task<IEnumerable<Produkt>> GetByCategoryAsync(Guid kategoriId);
    Task<IEnumerable<Produkt>> GetBySearchAsync(string search);
    Task<IEnumerable<Produkt>> GetFeaturedAsync();
    Task AddAsync(Produkt produkt);
    Task UpdateAsync(Produkt produkt);
    Task DeleteAsync(Guid id);
}
