using System;
using System.Collections.Generic;
using System.Text;
using Webshop.Domain.Entitites;
using System.Linq;


namespace Webshop.Infrastructure.EF.Seeds
{
    public static class WebShopSeeder
    {
        public static void Seed(WebshopDbContext context)
        {
            if (context.Produkter.Any())
            {
                return;
            }
            var klader = new Kategori { Id = Guid.NewGuid(), Namn = "Kläder" };
            var skor = new Kategori { Id = Guid.NewGuid(), Namn = "Skor" };
            var accessoarer = new Kategori { Id = Guid.NewGuid(), Namn = "Accessoarer" };

            var nike = new Leverantör { Id = Guid.NewGuid(), Namn = "Nike" };
            var adidas = new Leverantör { Id = Guid.NewGuid(), Namn = "Adidas" };
            var puma = new Leverantör { Id = Guid.NewGuid(), Namn = "Puma" };

            var standardFrakt = new FraktOmbud { Id = Guid.NewGuid(), Namn = "Standard Frakt", Pris = 49.99m };
            var expressFrakt = new FraktOmbud { Id = Guid.NewGuid(), Namn = "Express Frakt", Pris = 99.99m };

            var produkter = new List<Produkt>
            {
                new Produkt {Id=Guid.NewGuid(), Namn = "Vit T-shirt", Beskrivning = "En vit T-shirt i bomull", Pris = 199.99m,
                                    Färg="Vit",Storlek="M",LagerAntal=40,KategoriId=klader.Id,LeverantörId=nike.Id },
                new Produkt {Id=Guid.NewGuid(), Namn = "Svarta Sneakers", Beskrivning = "Svarta sneakers med bra grepp", Pris = 899.99m,
                                    Färg="Svart",Storlek="XL",LagerAntal=25,KategoriId=skor.Id,LeverantörId=adidas.Id },
                new Produkt {Id=Guid.NewGuid(), Namn = "Röd Keps", Beskrivning = "En röd keps med justerbar storlek", Pris = 149.99m,
                                    Färg="Röd",Storlek="One Size",LagerAntal=60,KategoriId=accessoarer.Id,LeverantörId=puma.Id },
                new Produkt {Id=Guid.NewGuid(), Namn = "Jeans", Beskrivning = "Blå jeans med stretch", Pris = 499.99m,
                                    Färg="Blå",Storlek="L",LagerAntal=30,KategoriId=klader.Id,LeverantörId=nike.Id },
                new Produkt {Id=Guid.NewGuid(), Namn = "Sandaler", Beskrivning = "Gröna sandaler för sommaren", Pris = 299.99m,
                                    Färg="Grön",Storlek="38",LagerAntal=20,KategoriId=skor.Id,LeverantörId=puma.Id },
                new Produkt {Id=Guid.NewGuid(), Namn = "Svart Bälte", Beskrivning = "Ett svart bälte i läder", Pris = 249.99m,
                                    Färg="Svart",Storlek="One Size",LagerAntal=50,KategoriId=accessoarer.Id,LeverantörId=adidas.Id }


            };

            var kunder = new List<Kund>
            {
                new Kund { Id = Guid.NewGuid(), Namn = "Anna Andersson", Adress = "Storgatan 1", Stad = "Stockholm", Postnummer = 41105, MobilNummer = 0701234567, Epost = "anna.andersson@example.com" },
                new Kund { Id = Guid.NewGuid(), Namn = "Lars Larsson", Adress = "Lilla Vägen 2", Stad = "Göteborg", Postnummer = 41106, MobilNummer = 0702345678, Epost = "lars.larsson@example.com" },
                new Kund { Id = Guid.NewGuid(), Namn = "Eva Eriksson", Adress = "Södra Gatan 3", Stad = "Malmö", Postnummer = 41107, MobilNummer = 0703456789, Epost = "eva.eriksson@example.com" },
                new Kund { Id = Guid.NewGuid(), Namn = "Per Persson", Adress = "Norra Vägen 4", Stad = "Uppsala", Postnummer = 41108, MobilNummer = 0704567890, Epost = "per.persson@example.com" }
            };

            var ordrar = new List<Order>
            {
                new Order { Id = Guid.NewGuid(),OrderDatum=DateTime.Now.AddDays(-4) ,KundId = kunder[0].Id, FraktOmbudId = standardFrakt.Id, TotalPris = 1198.99m },
                new Order { Id = Guid.NewGuid(),OrderDatum=DateTime.Now.AddDays(-2),KundId = kunder[1].Id, FraktOmbudId = expressFrakt.Id, TotalPris = 899.99m },
                new Order { Id = Guid.NewGuid(),OrderDatum=DateTime.Now.AddDays(-1),KundId = kunder[2].Id, FraktOmbudId = standardFrakt.Id, TotalPris = 1876.99m }
            };

            context.Kategorier.AddRange(klader, skor, accessoarer);
            context.AddRange(nike, adidas, puma);
            context.AddRange(standardFrakt, expressFrakt);
            context.AddRange(produkter);
            context.AddRange(kunder);
            context.AddRange(ordrar);

            context.SaveChanges();

        }
    }


}