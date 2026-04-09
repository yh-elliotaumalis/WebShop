using Microsoft.EntityFrameworkCore;
using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;
using Webshop.Infrastructure.EF;

namespace Webshop.Infrastructure.Repositories;

public class FraktOmbudRepository(WebshopDbContext db) : IFraktOmbudRepository
{
    public async Task<IEnumerable<FraktOmbud>> GetAllAsync()
        => await db.FraktOmbud.ToListAsync();

    public async Task<FraktOmbud?> GetByIdAsync(Guid id)
        => await db.FraktOmbud
            .FirstOrDefaultAsync(f => f.Id == id);
}
