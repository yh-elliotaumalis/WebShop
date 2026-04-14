using WebShop.Presentation.MenuHandlers;
using WebShop.Presentation.Menus;

namespace HustlersAB.Admin.Menus;

public class AdminMenu : MenuBase
{
    private readonly AdminProductHandler _productHandler;
    private readonly AdminCategoryHandler _categoryHandler;
    private readonly AdminCustomerHandler _customerHandler;
    private readonly AdminLeverantörHandler _leverantörHandler;

    public AdminMenu(AdminProductHandler productHandler, AdminCategoryHandler categoryHandler, AdminCustomerHandler customerHandler, AdminLeverantörHandler leverantörHandler)
    {
        _productHandler = productHandler;
        _categoryHandler = categoryHandler;
        _customerHandler = customerHandler;
        _leverantörHandler = leverantörHandler;

        _options = new[]
        {
            "Administrera produkter",
            "Administrera kategorier",
            "Administrera kunder",
            "Administrera leverantörer",
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
                Console.WriteLine("Statistik kommer senare...");
                Console.ReadKey(true);
                return false;

            case 5:
                return true;
        }

        return false;
    }
}
