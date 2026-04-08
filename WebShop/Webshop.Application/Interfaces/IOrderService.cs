using Webshop.Domain.Entitites;
using Webshop.Domain.Enums;

public interface IOrderService
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<IEnumerable<Order>> GetByCustomerAsync(Guid kundId);
    Task<Order> CreateOrderAsync(Guid kundId, List<ProduktOrder> produkter, Guid fraktOmbudId, Betalsätt betalsätt);
}
