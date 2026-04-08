using Microsoft.EntityFrameworkCore;
using Webshop.Domain.Entitites;

namespace Webshop.Infrastructure.EF;

public class WebshopDbContext(DbContextOptions<WebshopDbContext> options) : DbContext(options)
{
    public DbSet<Produkt> Produkter { get; set; }
    public DbSet<Kund> Kunder { get; set; }
    public DbSet<FraktOmbud> FraktOmbud { get; set; }
    public DbSet<Leverantör> Leverantörer { get; set; }
    public DbSet<Kategori> Kategorier { get; set; }
    public DbSet<Order> Ordrar { get; set; }
    public DbSet<ProduktOrder> ProduktOrdrar { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WebshopDbContext).Assembly);
    }
}
