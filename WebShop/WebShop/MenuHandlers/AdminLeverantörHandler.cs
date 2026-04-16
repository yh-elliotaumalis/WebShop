using Webshop.Application.Interfaces;
using Webshop.Application.Services;
using Webshop.Domain.Entitites;
using WebShop.Presentation.Menus;
using WebShop.Presentation.Validator;

namespace WebShop.Presentation.MenuHandlers;

public class AdminLeverantörHandler
{
    private readonly ILeverantörService _leverantörService;

    public AdminLeverantörHandler(ILeverantörService leverantörService)
    {
        _leverantörService = leverantörService;
    }

    public async Task HandleAddLeverantörAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Lägg till leverantör ===\n");

        var name = ConsoleHelper.ReadString("Ange leverantörens namn: ");
        if (!CategoryValidator.ValidateCategoryName(name)) { 
            Console.WriteLine("Ogiltigt leverantörsnamn. Försök igen.");
            Console.ReadKey(true);
            return;
        }

        var leverantör = new Leverantör {
            Id = Guid.NewGuid(),
            Namn = name
        };

        await _leverantörService.AddAsync(leverantör);

        Console.WriteLine($"Leverantör '{name}' har skapats.");
        Console.ReadKey(true);
    }

    public async Task HandleUpdateLeverantörAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Uppdatera leverantör ===\n");

        var leverantörer = await _leverantörService.GetAllAsync();

        var leverantör = MenuBase.NavigateList(leverantörer.ToList(), (list, selectedIndex) => {
            Console.WriteLine("Välj en leverantör att uppdatera:");
            for (int i = 0; i < list.Count; i++)
            {
                var prefix = i == selectedIndex ? "-> " : "   ";
                Console.WriteLine($"{prefix}{list[i].Namn}");
            }
        });

        if (leverantör == null)
        {
            Console.WriteLine("Ingen leverantör vald.");
            Console.ReadKey(true);
            return;
        }

        var newName = ConsoleHelper.ReadString($"Ange nytt namn för '{leverantör.Namn}': ");
        if (!CategoryValidator.ValidateCategoryName(newName))
        {
            Console.WriteLine("Ogiltigt leverantörsnamn. Försök igen.");
            Console.ReadKey(true);
            return;
        }

        leverantör.Namn = newName;
        await _leverantörService.UpdateAsync(leverantör);

        Console.WriteLine($"Leverantör '{newName}' har uppdaterats.");
        Console.ReadKey(true);
    }

    public async Task HandleDeleteLeverantörAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Ta bort leverantör ===\n");

        var leverantörer = await _leverantörService.GetAllAsync();

        var leverantör = MenuBase.NavigateList(leverantörer.ToList(), (list, selectedIndex) => {
            Console.WriteLine("Välj en leverantör att ta bort:");
            for (int i = 0; i < list.Count; i++)
            {
                var prefix = i == selectedIndex ? "-> " : "   ";
                Console.WriteLine($"{prefix}{list[i].Namn}");
            }
        });

        if (leverantör == null)
        {
            Console.WriteLine("Ingen kategori vald.");
            Console.ReadKey(true);
            return;
        }

        if (leverantör.Produkter.Any()) {
            Console.WriteLine("Leverantören har produkter kopplade och kan inte tas bort.");
            Console.ReadKey(true);
            return;
        }

        await _leverantörService.DeleteAsync(leverantör.Id);
        Console.WriteLine($"Leverantören '{leverantör.Namn}' har tagits bort.");
        Console.ReadKey(true);
    }
}
