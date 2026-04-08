namespace HustlersAB.Admin.MenuHandlers;

public class AdminHandler
{
    private readonly IProductService _productService;

    public AdminHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task HandleAddProductAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Lägg till ny produkt ===\n");

        var selectedSubCategory = await SelectSubCategoryAsync();
        if (selectedSubCategory == null) return;

        var name = ProductValidator.ReadRequiredString("Produktnamn: ");
        var description = ProductValidator.ReadRequiredString("Beskrivning: ");
        var price = ProductValidator.ReadDecimal("Pris: ", minValue: 0.01m);
        var qty = ProductValidator.ReadInt("Lagersaldo: ", minValue: 0);

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Price = price,
            QtyInStock = qty,
            SubCategoryId = selectedSubCategory.Id

        };

        await _productService.AddProductAsync(product);

        Console.WriteLine($"\nProdukten {product.Name} har lagts till i {selectedSubCategory.Name}!");

        Console.ReadKey(true);
    }

    private async Task<ProductSubCategory?> SelectSubCategoryAsync()
    {
        var subCategories = await _productService.GetAllSubCategoriesAsync();
        var menu = new SubCategoryMenu(subCategories.ToList());
        menu.ShowMenu("Välj underkategori");
        return menu.SelectedSubCategory;
    }


}
