using Webshop.Domain.Entitites;

namespace Webshop.Application.Interfaces;

public interface ILeverantörService
{
    Task<IEnumerable<Leverantör>> GetAllAsync();
    Task<Leverantör?> GetByIdAsync(Guid id);
    Task AddAsync(Leverantör leverantör);
    Task UpdateAsync(Leverantör leverantör);
    Task DeleteAsync(Guid id);
}
