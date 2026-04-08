using System;
using System.Collections.Generic;
using System.Text;

namespace Webshop.Domain.Entitites;

public class Kund
{
    public Guid Id { get; set; }
    public string Namn { get; set; }
    public string Adress { get; set; }
    public string Stad { get; set; }
    public int Postnummer { get; set; }
    public int MobilNummer { get; set; }
    public string Epost { get; set; }
    public List<Order> Ordrar { get; set; } = new List<Order>();
}
