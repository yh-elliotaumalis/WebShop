using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Webshop.Domain.Entitites;

namespace Webshop.Infrastructure.EF.Configurations;

public class FraktOmbudConfiguration : IEntityTypeConfiguration<FraktOmbud>
{
    public void Configure(EntityTypeBuilder<FraktOmbud> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Namn)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Pris)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.HasMany(x => x.Ordrar)
            .WithOne(x => x.FraktOmbud)
            .HasForeignKey(x => x.FraktOmbudId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
