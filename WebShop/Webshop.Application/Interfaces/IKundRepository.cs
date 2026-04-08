using Webshop.Domain.Entitites;

namespace Webshop.Domain.Interfaces;

public interface IKundRepository
{
    Task<IEnumerable<Kund>> GetAllAsync();
    Task<Kund?> GetByIdAsync(Guid id);
    Task<Kund?> GetBySearchAsync(string search);
    Task AddAsync(Kund kund);
    Task UpdateAsync(Kund kund);
    Task DeleteAsync(Guid id);
}

