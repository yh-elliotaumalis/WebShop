using Microsoft.EntityFrameworkCore;
using Webshop.Domain.Entitites;
using Webshop.Domain.Interfaces;
using Webshop.Infrastructure.EF;

namespace Webshop.Infrastructure.Repositories;

public class OrderRepository(WebshopDbContext db) : IOrderRepository
{
    private IQueryable<Order> GetOrdersWithIncludes()
        => db.Ordrar
            .Include(o => o.Kund)
            .Include(o => o.FraktOmbud)
            .Include(o => o.ProduktOrdrar)
                .ThenInclude(po => po.Produkt);

    public async Task<IEnumerable<Order>> GetAllAsync()
        => await GetOrdersWithIncludes()
            .ToListAsync();

    public async Task<Order?> GetByIdAsync(Guid id)
        => await GetOrdersWithIncludes()
            .FirstOrDefaultAsync(o => o.Id == id);

    public async Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid kundId)
        => await GetOrdersWithIncludes()
            .Where(o => o.KundId == kundId)
            .ToListAsync();

    public async Task AddAsync(Order order)
    {
        await db.Ordrar.AddAsync(order);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order order)
    {
        db.Ordrar.Update(order);
        await db.SaveChangesAsync();
    }
}
