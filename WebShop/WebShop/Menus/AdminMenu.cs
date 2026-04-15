using WebShop.Presentation.MenuHandlers;
using WebShop.Presentation.Menus;

namespace HustlersAB.Admin.Menus;

public class AdminMenu : MenuBase
{
    private readonly AdminProductHandler _productHandler;
    private readonly AdminCategoryHandler _categoryHandler;
    private readonly AdminCustomerHandler _customerHandler;
    private readonly AdminLeverantörHandler _leverantörHandler;
    private readonly AdminFraktOmbudHandler _fraktOmbudHandler;

    public AdminMenu(AdminProductHandler productHandler, AdminCategoryHandler categoryHandler, AdminCustomerHandler customerHandler, AdminLeverantörHandler leverantörHandler, AdminFraktOmbudHandler fraktOmbudHandler)
    {
        _productHandler = productHandler;
        _categoryHandler = categoryHandler;
        _customerHandler = customerHandler;
        _leverantörHandler = leverantörHandler;
        _fraktOmbudHandler = fraktOmbudHandler;

        _options = new[]
        {
            "Administrera produkter",
            "Administrera kategorier",
            "Administrera kunder",
            "Administrera leverantörer",
            "Administrera fraktombud",
            "Se statistik",
            "Tillbaka"
        };
    }

    protected override bool ExecuteChoice(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                Console.Clear();
                var productMenu = new AdminProductMenu(_productHandler);
                productMenu.ShowMenu("Administrera produkter");
                return false;

            case 1:
                Console.Clear();
                var categoryMenu = new AdminCategoryMenu(_categoryHandler);
                categoryMenu.ShowMenu("Administrera kategorier");
                return false;

            case 2:
                Console.Clear();
                var customerMenu = new AdminCustomerMenu(_customerHandler);
                customerMenu.ShowMenu("Administrera kunder");
                return false;

            case 3:
                Console.Clear();
                var leverantörMenu = new AdminLeverantörMenu(_leverantörHandler);
                leverantörMenu.ShowMenu("Administrera leverantörer");
                return false;

            case 4:
                Console.Clear();
                var fraktOmbudMenu = new AdminFraktOmbudMenu(_fraktOmbudHandler);
                fraktOmbudMenu.ShowMenu("Administrera fraktombud");
                return false;

            case 5:
                Console.Clear();
                var statisticsMenu = new AdminStatisticsMenu(_productHandler, _categoryHandler);
                statisticsMenu.ShowMenu("Se statistik");
                return false;
                return false;

            case 6:
                return true;
        }

        return false;
    }
}
