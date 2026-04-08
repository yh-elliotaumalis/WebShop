using Webshop.Domain.Entitites;

namespace Webshop.Application.Interfaces;

public interface IVarukorgService
{
    void AddProduct(Guid produktId, int antal);
    void RemoveProduct(Guid produktId);
    void UpdateQuantity(Guid produktId, int nyttAntal);
    IReadOnlyDictionary<Guid, int> GetItems();
    decimal CalculateTotal(IReadOnlyList<Produkt> produkter);
    bool IsEmpty();
    void Clear();
}
