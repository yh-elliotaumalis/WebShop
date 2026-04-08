namespace Webshop.Domain.Entitites;

public class FraktOmbud
{
    public Guid Id { get; set; }
    public string Namn { get; set; } = null!;
    public decimal Pris { get; set; }
    public List<Order> Ordrar { get; set; } = new List<Order>();

}
