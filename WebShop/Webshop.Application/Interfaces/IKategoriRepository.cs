using Webshop.Domain.Entitites;

namespace Webshop.Application.Interfaces;

public interface IKategoriRepository
{
    Task<IEnumerable<Kategori>> GetAllAsync();
    Task<Kategori?> GetByIdAsync(Guid id);
    Task<Kategori?> GetMostPopularCategoryAsync();
    Task AddAsync(Kategori kategori);
    Task UpdateAsync(Kategori kategori);
    Task DeleteAsync(Guid id);
}
