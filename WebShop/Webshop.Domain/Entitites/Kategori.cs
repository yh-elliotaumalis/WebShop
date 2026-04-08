using System;
using System.Collections.Generic;
using System.Text;

namespace Webshop.Domain.Entitites;

public class Kategori
{
    public Guid Id { get; set; }
    public string Namn { get; set; }
    public List<Produkt> Produkter { get; set; } = new List<Produkt>();

    public Kategori() { }
    
    
}   
