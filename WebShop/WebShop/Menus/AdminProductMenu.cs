using WebShop.Presentation.MenuHandlers;

namespace WebShop.Presentation.Menus;

public class AdminProductMenu : MenuBase
{
    private readonly AdminProductHandler _handler;

    public AdminProductMenu(AdminProductHandler handler)
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
                _handler.HandleDeleteProductAsync().GetAwaiter().GetResult();
                return false;

            case 3:
                return true;
        }

        return false;
    }
}
