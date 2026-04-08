using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webshop.Domain.Entitites;

namespace Webshop.Infrastructure.EF.Configurations;

public class ProduktConfiguration : IEntityTypeConfiguration<Produkt>
{
    public void Configure(EntityTypeBuilder<Produkt> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Namn)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Beskrivning)
            .HasMaxLength(1024);

        builder.Property(x => x.Pris)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Färg)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Storlek)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasMany(x => x.ProduktOrdrar)
            .WithOne(x => x.Produkt)
            .HasForeignKey(x => x.ProduktId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}