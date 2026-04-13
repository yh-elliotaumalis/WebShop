using HustlersAB.Admin.Menus;
using Webshop.Application.Interfaces;
using Webshop.Application.Services;
using Webshop.Domain.Entitites;
using WebShop.Presentation.Validator;

namespace WebShop.Presentation.MenuHandlers;

public class AdminCategoryHandler
{
    private readonly IKategoriService _kategoriService;

    public AdminCategoryHandler(IKategoriService kategoriService)
    {
        _kategoriService = kategoriService;
    }

    public async Task HandleAddCategoryAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Lägg till kategori ===\n");

        var name = ConsoleHelper.ReadString("Ange kategorinamn: ");
        if (!CategoryValidator.ValidateCategoryName(name)) { 
            Console.WriteLine("Ogiltigt kategorinamn. Försök igen.");
            Console.ReadKey(true);
            return;
        }

        var category = new Kategori {
            Id = Guid.NewGuid(),
            Namn = name
        };

        await _kategoriService.AddAsync(category);

        Console.WriteLine($"Kategori '{name}' har skapats.");
        Console.ReadKey(true);
    }

    public async Task HandleUpdateCategoryAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Uppdatera kategori ===\n");

        var categories = await _kategoriService.GetAllAsync();

        var category = MenuBase.NavigateList(categories.ToList(), (list, selectedIndex) => {
            Console.WriteLine("Välj en kategori att uppdatera:");
            for (int i = 0; i < list.Count; i++)
            {
                var prefix = i == selectedIndex ? "-> " : "   ";
                Console.WriteLine($"{prefix}{list[i].Namn}");
            }
        });

        if (category == null)
        {
            Console.WriteLine("Ingen kategori vald.");
            Console.ReadKey(true);
            return;
        }

        var newName = ConsoleHelper.ReadString($"Ange nytt namn för '{category.Namn}': ");
        if (!CategoryValidator.ValidateCategoryName(newName))
        {
            Console.WriteLine("Ogiltigt kategorinamn. Försök igen.");
            Console.ReadKey(true);
            return;
        }

        category.Namn = newName;
        await _kategoriService.UpdateAsync(category);

        Console.WriteLine($"Kategori '{newName}' har uppdaterats.");
        Console.ReadKey(true);
    }

    public async Task HandleDeleteCategoryAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Ta bort kategori ===\n");

        var categories = await _kategoriService.GetAllAsync();

        var category = MenuBase.NavigateList(categories.ToList(), (list, selectedIndex) => {
            Console.WriteLine("Välj en kategori att ta bort:");
            for (int i = 0; i < list.Count; i++)
            {
                var prefix = i == selectedIndex ? "-> " : "   ";
                Console.WriteLine($"{prefix}{list[i].Namn}");
            }
        });

        if (category == null)
        {
            Console.WriteLine("Ingen kategori vald.");
            Console.ReadKey(true);
            return;
        }

        if (category.Produkter.Any()) {
            Console.WriteLine("Kategorin har produkter kopplade och kan inte tas bort.");
            Console.ReadKey(true);
            return;
        }

        await _kategoriService.DeleteAsync(category.Id);
        Console.WriteLine($"Kategori '{category.Namn}' har tagits bort.");
        Console.ReadKey(true);
    }
}
