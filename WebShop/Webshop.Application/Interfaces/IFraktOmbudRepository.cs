using Webshop.Domain.Entitites;

namespace Webshop.Domain.Interfaces;

public interface IFraktOmbudRepository
{
    Task<IEnumerable<FraktOmbud>> GetAllAsync();
    Task<FraktOmbud?> GetByIdAsync(Guid id);
}
