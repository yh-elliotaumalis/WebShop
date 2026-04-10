using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;
using Webshop.Domain.Enums;
using Webshop.Domain.Interfaces;

namespace Webshop.Application.Services;

public class LeverantörService(ILeverantörRepository leverantörRepository) : ILeverantörService
{
    public async Task<Leverantör?> GetByIdAsync(Guid id)
        => await leverantörRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Leverantör>> GetAllAsync()
        => await leverantörRepository.GetAllAsync();

    public async Task AddAsync(Leverantör leverantör)
        => await leverantörRepository.AddAsync(leverantör);

    public async Task UpdateAsync(Leverantör leverantör)
        => await leverantörRepository.UpdateAsync(leverantör);

    public async Task DeleteAsync(Guid id)
        => await leverantörRepository.DeleteAsync(id);
}
