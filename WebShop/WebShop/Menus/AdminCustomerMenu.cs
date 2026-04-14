using HustlersAB.Admin.Menus;
using WebShop.Presentation.MenuHandlers;

namespace WebShop.Presentation.Menus;

public class AdminCustomerMenu : MenuBase
{
    private readonly AdminCustomerHandler _handler;

    public AdminCustomerMenu(AdminCustomerHandler handler)
    {
        _handler = handler;

        _options = new[]
        {
            "Visa kunder",
            "Lägg till ny kund",
            "Uppdatera kund",
            "Ta bort kund",
            "Tillbaka"
        };
    }

    protected override bool ExecuteChoice(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                _handler.HandleShowCustomerAsync().GetAwaiter().GetResult();
                return false;

            case 1:
                _handler.HandleAddCustomerAsync().GetAwaiter().GetResult();
                return false;

            case 2:
                _handler.HandleUpdateCustomerAsync().GetAwaiter().GetResult();
                return false;

            case 3:
                _handler.HandleDeleteCustomerAsync().GetAwaiter().GetResult();
                return false;

            case 4:
                return true;
        }

        return false;
    }
}
