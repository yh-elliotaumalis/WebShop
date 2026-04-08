using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Webshop.Domain.Entitites;

namespace Webshop.Infrastructure.EF.Configurations;

public class LeverantörConfiguration : IEntityTypeConfiguration<Leverantör>
{
    public void Configure(EntityTypeBuilder<Leverantör> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Namn)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasMany(x => x.Produkter)
            .WithOne(p => p.Leverantör)
            .HasForeignKey(p => p.LeverantörId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
