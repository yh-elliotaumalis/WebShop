using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Webshop.Domain.Entitites;

namespace Webshop.Infrastructure.EF.Configurations;

public class KategoriConfiguration : IEntityTypeConfiguration<Kategori>
{
    public void Configure(EntityTypeBuilder<Kategori> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Namn)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasMany(x => x.Produkter)
            .WithOne(x => x.Kategori)
            .HasForeignKey(x => x.KategoriId)
            .OnDelete(DeleteBehavior.Restrict);



    }
}