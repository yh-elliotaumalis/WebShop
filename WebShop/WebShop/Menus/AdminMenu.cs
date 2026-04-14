using WebShop.Presentation.MenuHandlers;
using WebShop.Presentation.Menus;

namespace HustlersAB.Admin.Menus;

public class AdminMenu : MenuBase
{
    private readonly AdminProductHandler _productHandler;
    private readonly AdminCategoryHandler _categoryHandler;

    public AdminMenu(AdminProductHandler productHandler, AdminCategoryHandler categoryHandler)
    {
        _productHandler = productHandler;
        _categoryHandler = categoryHandler;

        _options = new[]
        {
            "Administrera produkter",
            "Administrera kategorier",
            "Administrera kunder",
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
                productMenu.ShowMenu("ProductMenu");
                return false;

            case 1:
                Console.Clear();
                var categoryMenu = new AdminCategoryMenu(_categoryHandler);
                categoryMenu.ShowMenu("CategoryMenu");
                return false;

            case 2:
                Console.Clear();
                Console.WriteLine("Kundadministration kommer senare...");
                Console.ReadKey(true);
                return false;

            case 3:
                Console.Clear();
                Console.WriteLine("Statistik kommer senare...");
                Console.ReadKey(true);
                return false;

            case 4:
                return true;
        }

        return false;
    }
}
