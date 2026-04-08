namespace HustlersAB.Admin.Menus;

public abstract class MenuBase
{
    protected string[] _options = Array.Empty<string>();

    protected void PrintMenu(int selectedIndex, string title)
    {
        Console.WriteLine($"=== {title} ===\n");

        for (int i = 0; i < _options.Length; i++)
        {
            string prefix = (i == selectedIndex) ? "> " : "  ";
            Console.WriteLine($"{prefix}{_options[i]}");
        }
    }

    public void ShowMenu(string title)
    {
        int selectedIndex = 0;

        while (true)
        {
            Console.Clear();
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

    protected abstract bool ExecuteChoice(int selectedIndex);
}
