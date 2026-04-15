using Webshop.Domain.Entitites;

namespace Webshop.Application.Interfaces;

public interface IFraktOmbudService
{
    Task<IEnumerable<FraktOmbud>> GetAllAsync();
    Task<FraktOmbud?> GetByIdAsync(Guid id);
    Task AddAsync(FraktOmbud fraktOmbud);
    Task UpdateAsync(FraktOmbud fraktOmbud);
    Task DeleteAsync(Guid id);
}
