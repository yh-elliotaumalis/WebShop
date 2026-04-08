namespace HustlersAB.Admin.Menus;

public class CustomerMenu : MenuBase
{
    public CustomerMenu()
    {
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
