using Webshop.Application.Interfaces;

namespace HustlersAB.Admin.Menus;

public class CustomerMenu : MenuBase
{
    private readonly IProduktService _productService;
    public CustomerMenu(IProduktService productService)
    {
        _productService = productService;

        _options = new[] { "Handla produkter", "Varukorgen", "Tillbaka" };
    }

    protected override bool ExecuteChoice(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                Console.Clear();
                Console.WriteLine("Handla produkter kommer senare...");
                Console.ReadKey(true);
                return false;

            case 1:
                Console.Clear();
                Console.WriteLine("Varukorgen kommer senare...");
                Console.ReadKey(true);
                return false;

            case 2:
                return true;
        }

        return false;
    }
}
