using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Webshop.Domain.Entitites;

namespace Webshop.Infrastructure.EF.Configurations;

public class KundConfiguration : IEntityTypeConfiguration<Kund>
{
    public void Configure(EntityTypeBuilder<Kund> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Namn)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Adress)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Stad)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Postnummer)
            .IsRequired();

        builder.Property(x => x.MobilNummer)
            .IsRequired();

        builder.Property(x => x.Epost)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasMany(x => x.Ordrar)
            .WithOne(x => x.Kund)
            .HasForeignKey(x => x.KundId)
            .OnDelete(DeleteBehavior.Restrict);


    }
}
