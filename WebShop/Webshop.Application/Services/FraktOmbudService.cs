using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;

namespace Webshop.Application.Services;

public class FraktOmbudService(IFraktOmbudRepository fraktOmbudRepository) : IFraktOmbudService
{
    public async Task<FraktOmbud?> GetByIdAsync(Guid id)
        => await fraktOmbudRepository.GetByIdAsync(id);

    public async Task<IEnumerable<FraktOmbud>> GetAllAsync()
        => await fraktOmbudRepository.GetAllAsync();

    public async Task AddAsync(FraktOmbud fraktOmbud)
        => await fraktOmbudRepository.AddAsync(fraktOmbud);
    public async Task UpdateAsync(FraktOmbud fraktOmbud)
        => await fraktOmbudRepository.UpdateAsync(fraktOmbud);

    public async Task DeleteAsync(Guid id)
        => await fraktOmbudRepository.DeleteAsync(id);
}
