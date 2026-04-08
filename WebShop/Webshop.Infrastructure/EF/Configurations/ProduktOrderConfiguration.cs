using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Webshop.Domain.Entitites;

namespace Webshop.Infrastructure.EF.Configurations;

public class ProduktOrderConfiguration : IEntityTypeConfiguration<ProduktOrder>
{
    public void Configure(EntityTypeBuilder<ProduktOrder> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Produkt)
            .WithMany(x => x.ProduktOrdrar)
            .HasForeignKey(x => x.ProduktId);
            
    }
}
