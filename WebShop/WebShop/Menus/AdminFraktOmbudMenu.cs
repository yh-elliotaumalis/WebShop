using HustlersAB.Admin.Menus;
using WebShop.Presentation.MenuHandlers;

namespace WebShop.Presentation.Menus;

public class AdminFraktOmbudMenu : MenuBase
{
    private readonly AdminFraktOmbudHandler _handler;

    public AdminFraktOmbudMenu(AdminFraktOmbudHandler handler)
    {
        _handler = handler;

        _options = new[]
        {
            "Lägg till fraktombud",
            "Uppdatera fraktombud",
            "Ta bort fraktombud",
            "Tillbaka"
        };
    }

    protected override bool ExecuteChoice(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                _handler.HandleAddFraktOmbudAsync().GetAwaiter().GetResult();
                return false;

            case 1:
                _handler.HandleUpdateFraktOmbudAsync().GetAwaiter().GetResult();
                return false;

            case 2:
                _handler.HandleDeleteFraktOmbudAsync().GetAwaiter().GetResult();
                return false;

            case 3:
                return true;
        }

        return false;
    }
}
