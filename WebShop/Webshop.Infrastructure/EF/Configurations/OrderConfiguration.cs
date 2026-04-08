using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webshop.Domain.Entitites;

namespace Webshop.Infrastructure.EF.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.HasOne(o => o.Kund)
               .WithMany(k => k.Ordrar)
               .HasForeignKey(o => o.KundId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.FraktOmbud)
               .WithMany(f => f.Ordrar)
               .HasForeignKey(o => o.FraktOmbudId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(o => o.ProduktOrdrar)
               .WithOne(po => po.Order)
               .HasForeignKey(po => po.OrderId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
