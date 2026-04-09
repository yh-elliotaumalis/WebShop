using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;

namespace Webshop.Application.Services;

public class VarukorgService : IVarukorgService
{
    private readonly Dictionary<Guid, int> _varor = new();

    public void AddProduct(Guid produktId, int antal)
    {
        if (_varor.ContainsKey(produktId))
            _varor[produktId] += antal;
        else
            _varor[produktId] = antal;
    }

    public void RemoveProduct(Guid produktId)
        => _varor.Remove(produktId);

    public void UpdateQuantity(Guid produktId, int nyttAntal)
    {
        if (_varor.ContainsKey(produktId))
            _varor[produktId] = nyttAntal;
    }

    public IReadOnlyDictionary<Guid, int> GetItems()
        => _varor;

    public decimal CalculateTotal(IReadOnlyList<Produkt> produkter)
        => produkter.Sum(p => p.Pris * _varor.GetValueOrDefault(p.Id));

    public bool IsEmpty()
        => _varor.Count == 0;

    public void Clear()
        => _varor.Clear();
}
