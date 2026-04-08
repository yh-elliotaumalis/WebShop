using System;
using System.Collections.Generic;
using System.Text;

namespace Webshop.Domain.Entitites;

public class ProduktOrder
{
    public Guid Id { get; set; }
    public int Antal { get; set; }
    public decimal PrisvidKöp { get; set; }
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
    public Guid ProduktId { get; set; }
    public Produkt Produkt { get; set; }
    public ProduktOrder() { }
}
