using Webshop.Domain.Entitites;

namespace Webshop.Domain.Interfaces;

public interface IProduktRepository
{
    Task<IEnumerable<Produkt>> GetAllAsync();
    Task<Produkt?> GetByIdAsync(Guid id);
    Task<IEnumerable<Produkt>> GetByCategoryAsync(Guid categoryId);
    Task<IEnumerable<Produkt>> GetBySearchAsync(string search);
    Task AddAsync(Produkt produkt);
    Task UpdateAsync(Produkt produkt);
    Task DeleteAsync(Guid id);
}
