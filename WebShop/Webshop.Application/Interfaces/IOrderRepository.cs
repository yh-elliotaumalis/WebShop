using Webshop.Domain.Entitites;

namespace Webshop.Domain.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(Guid id);
    Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid kundId);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
}
