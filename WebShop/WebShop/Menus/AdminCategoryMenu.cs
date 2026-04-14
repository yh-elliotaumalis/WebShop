using HustlersAB.Admin.Menus;
using WebShop.Presentation.MenuHandlers;

namespace WebShop.Presentation.Menus;

public class AdminCategoryMenu : MenuBase
{
    private readonly AdminCategoryHandler _handler;

    public AdminCategoryMenu(AdminCategoryHandler handler)
    {
        _handler = handler;

        _options = new[]
        {
            "Lägg till kategori",
            "Uppdatera kategori",
            "Ta bort kategori",
            "Tillbaka"
        };
    }

    protected override bool ExecuteChoice(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                _handler.HandleAddCategoryAsync().GetAwaiter().GetResult();
                return false;

            case 1:
                _handler.HandleUpdateCategoryAsync().GetAwaiter().GetResult();
                return false;

            case 2:
                _handler.HandleDeleteCategoryAsync().GetAwaiter().GetResult();
                return false;

            case 3:
                return true;
        }

        return false;
    }
}
