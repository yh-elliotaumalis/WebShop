using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;

namespace WebShop.Presentation.MenuHandlers;

public class CustomerProductHandler
{
    private readonly IProduktService _productService;
    public CustomerProductHandler(IProduktService produktService)
    {
        _productService = produktService;

    }

    public async Task<Produkt?> GetProductAsync(Guid id)
        => await _productService.GetByIdAsync(id);
    public async Task<IEnumerable<Produkt>> SearchProductAsync(string search)
        => await _productService.GetBySearchAsync(search);
    public async Task<IEnumerable<Produkt>> GetAllProductsAsync()
        => await _productService.GetAllAsync();






}
