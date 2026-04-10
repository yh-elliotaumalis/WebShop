using HustlersAB.Admin.MenuHandlers;
using Webshop.Application.Interfaces;

namespace HustlersAB.Admin.Menus;

public class MainMenu : MenuBase
{
    private readonly IProduktService _productService;

    public MainMenu(IProduktService productService)
    {
        _productService = productService;

        _options = new[] { "Kund", "Admin", "Avsluta" };
    }

    protected override bool ExecuteChoice(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                new CustomerMenu(_productService).ShowMenu("Kund Meny");
                return false;

            case 1:
                var adminHandler = new AdminHandler(_productService);
                var adminMenu = new AdminMenu(adminHandler);
                adminMenu.ShowMenu("Admin Meny");
                return false;

            case 2:
                Environment.Exit(0);
                return true;
        }

        return false;
    }
    private void WriteCentered(string text)
    {
        int windowWidth = Console.WindowWidth;
        int textLength = text.Length;
        int spaces = (windowWidth - textLength) / 2;
        Console.WriteLine(new string(' ', spaces) + text);
    }
    private void ShowWelcome()
    {
        WriteCentered("Välkommen till WebShop!");
        WriteCentered("************************");
        Console.WriteLine();

       
        WriteCentered("Här Kan Du Bläddra Bland Produkter Och Hantera Ditt Konto.");
        WriteCentered("Använd Piltangenterna För Att Navigera Och Enter För Att Välja.");
        Console.WriteLine();
    }
    //Show welcome Text
    public new void ShowMenu(string title)
    {
        int selectedIndex = 0;

        while (true)
        {
            Console.Clear();

            ShowWelcome();

            PrintMenu(selectedIndex, title);

            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = (selectedIndex - 1 + _options.Length) % _options.Length;
                    break;

                case ConsoleKey.DownArrow:
                    selectedIndex = (selectedIndex + 1) % _options.Length;
                    break;

                case ConsoleKey.Enter:
                    if (ExecuteChoice(selectedIndex))
                        return;
                    break;
            }
        }
    }
}
