using HustlersAB.Admin.Menus;
using WebShop.Presentation.MenuHandlers;

namespace WebShop.Presentation.Menus;

public class AdminLeverantörMenu : MenuBase
{
    private readonly AdminLeverantörHandler _handler;

    public AdminLeverantörMenu(AdminLeverantörHandler handler)
    {
        _handler = handler;

        _options = new[]
        {
            "Lägg till leverantör",
            "Uppdatera leverantör",
            "Ta bort leverantör",
            "Tillbaka"
        };
    }

    protected override bool ExecuteChoice(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                _handler.HandleAddLeverantörAsync().GetAwaiter().GetResult();
                return false;

            case 1:
                _handler.HandleUpdateLeverantörAsync().GetAwaiter().GetResult();
                return false;

            case 2:
                _handler.HandleDeleteLeverantörAsync().GetAwaiter().GetResult();
                return false;

            case 3:
                return true;
        }

        return false;
    }
}
