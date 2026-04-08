using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;
using Webshop.Domain.Interfaces;

namespace Webshop.Application.Services;

public class KundService(IKundRepository kundRepo) : IKundService
{
    public async Task<IEnumerable<Kund>> GetAllAsync()
        => await kundRepo.GetAllAsync();

    public async Task<Kund?> GetByIdAsync(Guid id)
        => await kundRepo.GetByIdAsync(id);

    public async Task<IEnumerable<Kund>> GetBySearchAsync(string search)
        => await kundRepo.GetBySearchAsync(search);

    public async Task AddAsync(Kund kund)
        => await kundRepo.AddAsync(kund);

    public async Task UpdateAsync(Kund kund)
        => await kundRepo.UpdateAsync(kund);

    public async Task DeleteAsync(Guid id)
        => await kundRepo.DeleteAsync(id);
}
