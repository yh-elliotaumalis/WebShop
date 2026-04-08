using Webshop.Domain.Entitites;

namespace Webshop.Domain.Interfaces;

public interface ILeverantörRepository
{
    Task<IEnumerable<Leverantör>> GetAllAsync();
    Task<Leverantör?> GetByIdAsync(Guid id);
    Task<IEnumerable<(Leverantör Leverantör, decimal TotalSales)>> GetSalesBySupplierAsync();
    Task AddAsync(Leverantör leverantör);
    Task UpdateAsync(Leverantör leverantör);
    Task DeleteAsync(Guid id);
}
