using Webshop.Domain.Entitites;

namespace Webshop.Application.Interfaces;

public interface IKundService
{
    Task<IEnumerable<Kund>> GetAllAsync();
    Task<Kund?> GetByIdAsync(Guid id);
    Task<IEnumerable<Kund>> GetBySearchAsync(string search);
    Task AddAsync(Kund kund);
    Task UpdateAsync(Kund kund);
    Task DeleteAsync(Guid id);
}
