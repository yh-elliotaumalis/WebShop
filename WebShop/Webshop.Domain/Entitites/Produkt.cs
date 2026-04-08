using System;
using System.Collections.Generic;
using System.Text;

namespace Webshop.Domain.Entitites;

public class Produkt
{
    public Guid Id { get; set; }
    public string Namn { get; set; }
    public string Beskrivning { get; set; }
    public decimal Pris { get; set; }
    public string Färg { get; set; }
    public string Storlek { get; set; }
    public int LagerAntal { get; set; }
    public Guid LeverantörId { get; set; }
    public Leverantör Leverantör { get; set; }
    public Guid KategoriId { get; set; }
    public Kategori Kategori { get; set; }
    public List<ProduktOrder> ProduktOrdrar { get; set; }

    public Produkt() { }

}
