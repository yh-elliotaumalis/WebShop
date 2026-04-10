using HustlersAB.Admin.MenuHandlers;

namespace HustlersAB.Admin.Menus;

public class AdminProductMenu : MenuBase
{
    private readonly AdminHandler _handler;

    public AdminProductMenu(AdminHandler handler)
    {
        _handler = handler;

        _options = new[]
        {
            "Lägg till produkt",
            "Ändra produkt",
            "Ta bort produkt",
            "Tillbaka"
        };
    }

    protected override bool ExecuteChoice(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                _handler.HandleAddProductAsync().GetAwaiter().GetResult();
                return false;

            case 1:
                _handler.HandleUpdateProductAsync().GetAwaiter().GetResult();
                return false;

            case 2:
                Console.Clear();
                Console.WriteLine("Här ska vi senare ta bort produkt.");
                Console.ReadKey(true);
                return false;

            case 3:
                return true;
        }

        return false;
    }
}
