using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;
using Webshop.Domain.Enums;
using Webshop.Domain.Interfaces;

namespace Webshop.Application.Services;

public class OrderService(IOrderRepository orderRepository) : IOrderService
{
    public async Task<Order?> GetByIdAsync(Guid id)
        => await orderRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Order>> GetByCustomerAsync(Guid kundId)
        => await orderRepository.GetByCustomerIdAsync(kundId);

    public async Task<Order> CreateOrderAsync(Guid kundId, List<ProduktOrder> produkter, Guid fraktOmbudId, Betalsätt betalsätt)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            KundId = kundId,
            FraktOmbudId = fraktOmbudId,
            OrderDatum = DateTime.Now,
            TotalPris = produkter.Sum(p => p.PrisvidKöp * p.Antal),
            Betalsätt = betalsätt,
            ÄrBetald = true,
            ProduktOrdrar = produkter
        };

        await orderRepository.AddAsync(order);
        return order;
    }
}
