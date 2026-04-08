namespace Webshop.Domain.Entitites;

public class Kund
{
    public Guid Id { get; set; }
    public string Namn { get; set; } = null!;
    public string Adress { get; set; } = null!;
    public string Stad { get; set; } = null!;
    public int Postnummer { get; set; }
    public int MobilNummer { get; set; }
    public string Epost { get; set; } = null!;
    public List<Order> Ordrar { get; set; } = new List<Order>();
}
