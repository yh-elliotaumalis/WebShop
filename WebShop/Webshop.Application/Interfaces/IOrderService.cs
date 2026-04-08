using Webshop.Domain.Entitites;

namespace Webshop.Application.Interfaces;

public interface IOrderService
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<IEnumerable<Order>> GetByCustomerAsync(Guid kundId);
    Task<Order> CreateOrderAsync(Guid kundId, List<ProduktOrder> produkter, Guid fraktOmbudId);
    Task ConfirmPaymentAsync(Guid orderId, string paymentMethod);
}
