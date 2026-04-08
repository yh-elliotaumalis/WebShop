using System;
using System.Collections.Generic;
using System.Text;

namespace Webshop.Domain.Entitites;

public class Order
{
    public Guid Id { get; set; }
    public DateTime OrderDatum { get; set; }
    public decimal TotalPris { get; set; }
    public Guid KundId { get; set; }
    public Kund Kund { get; set; }
    public Guid FraktOmbudId { get; set; }
    public FraktOmbud FraktOmbud { get; set; }
    public List<ProduktOrder> ProduktOrdrar { get; set; } = new List<ProduktOrder>();
}
