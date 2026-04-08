using System;
using System.Collections.Generic;
using System.Text;

namespace Webshop.Domain.Entitites;

public class FraktOmbud
{
    public Guid Id { get; set; }
    public string Namn { get; set; }
    public decimal Pris { get; set; }
    public List<Order> Ordrar { get; set; } = new List<Order>();

}
