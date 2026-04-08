using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace WebShop.Presentation.UI;

public record ProductPreview(string Name, decimal Price,string Category,string Size,int Stock);

public static class MenuWindow
{
    private static readonly string[] MainItems = ["Startsida", "Search", "Varukorg", "Frakt", "Betalning", "Admin", "Avsluta"];
    private const decimal VatRate = 0.25m;
    private const decimal StandardShipping = 40m;
    private const decimal ExpressShipping = 90m;
    private const decimal FreeShippingLimit = 500m;

    private enum ShippingType { Standard, Express }
    private enum PaymentType { Card, Swish }

    private sealed class CheckoutState
    {
        public ShippingType Shipping { get; set; } = ShippingType.Standard;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }

    public static void Start(IReadOnlyList<ProductPreview>? popularProducts = null)
    {
        var products = (popularProducts ?? []).Take(6).ToList();
        if (Console.IsInputRedirected || Console.IsOutputRedirected)
        {
            PrintFallback(products);
            return;
        }

        try
        {
            Console.CursorVisible = false;
            Console.BackgroundColor = Theme.Background;
            Console.Clear();
            ShowWelcomeScreen();
            RunInteractive(products);
        }
        finally
        {
            Console.ResetColor();
            try { Console.CursorVisible = true; } catch { }
        }
    }

    private static void RunInteractive(IReadOnlyList<ProductPreview> products)
    {
        var menuIndex = 0;
        var search = string.Empty;
        var cart = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var checkout = new CheckoutState();
        var status = "Valkommen! S = sok, 1-6 = detaljer, A-F = kop.";

        while (true)
        {
            DrawMain(products, menuIndex, cart, search, checkout, status);
            var key = Console.ReadKey(true).Key;

          
            if (key == ConsoleKey.UpArrow)
            {
                menuIndex = (menuIndex - 1 + MainItems.Length) % MainItems.Length;
                continue;
            }

            if (key == ConsoleKey.DownArrow)
            {
                menuIndex = (menuIndex + 1) % MainItems.Length;
                continue;
            }

        
            if (key == ConsoleKey.S)
            {
                var input = Prompt("Search (name or category):");

                if (string.IsNullOrWhiteSpace(input))
                {
                    search = "";
                    status = "Showing all products.";
                }
                else
                {
                    search = input;

                    var found = products.Any(p =>
                        p.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
                    || p.Category.Contains(search, StringComparison.OrdinalIgnoreCase)
                    );

                    status = found
                        ? $"Results for: {search}"
                        : "No products found ";
                }

                continue;
            }

           
            if (key >= ConsoleKey.D1 && key <= ConsoleKey.D6)
            {
                var idx = key - ConsoleKey.D1;

                if (idx < products.Count)
                {
                    ShowProductDetails(products[idx]);
                    status = $"Visar detaljer for {products[idx].Name}.";
                }

                continue;
            }

        
            if (TryAddFromLetterKey(key, products, cart, out var added))
            {
                status = $"{added} tillagd i varukorg.";
                continue;
            }

        
            if (key == ConsoleKey.K)
            {
                var result = RunCheckout(products, cart, checkout);
                status = result;
                continue;
            }

        
            if (key == ConsoleKey.Enter)
            {
                var selected = MainItems[menuIndex];

                if (selected == "Avsluta") return;

           
                if (selected == "Startsida")
                {
                    search = "";
                    status = "Startsida vald.";
                    continue;
                }

             
                if (selected == "Search")
                {
                    var input = Prompt("Search (name or category):");

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        search = "";
                        status = "Showing all products.";
                    }
                    else
                    {
                        search = input;

                        var found = products.Any(p =>
                            p.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
                        || p.Category.Contains(search, StringComparison.OrdinalIgnoreCase)
                        );

                        status = found
                            ? $"Results for: {search}"
                            : "No products found ";
                    }

                    continue;
                }

             
                if (selected == "Varukorg")
                {
                    var wantsCheckout = ManageCart(products, cart);
                    status = wantsCheckout
                        ? RunCheckout(products, cart, checkout)
                        : "Varukorg uppdaterad.";
                    continue;
                }

           
                if (selected == "Frakt")
                {
                    ConfigureShipping(checkout);
                    status = "Frakt uppdaterad.";
                    continue;
                }

              
                if (selected == "Betalning")
                {
                    status = RunCheckout(products, cart, checkout);
                    continue;
                }

             
                if (selected == "Admin")
                {
                    status = "Admin hanteras av annan branch.";
                    continue;
                }

                status = $"{selected} vald.";
                continue;
            }

            if (key == ConsoleKey.Escape) return;
        }
    }

    private static void DrawMain(
       IReadOnlyList<ProductPreview> products,
       int menuIndex,
       Dictionary<string, int> cart,
       string search,
       CheckoutState checkout,
       string status)
    {
        Console.Clear();

        var filtered = products
            .Where(p =>
                string.IsNullOrWhiteSpace(search) ||
                p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.Category.Contains(search, StringComparison.OrdinalIgnoreCase)
            )
            .ToList();

        
        var maxTextWidth = 0;
        foreach (var p in filtered.Take(6))
        {
            var text = $"{p.Name} ({p.Category}) Size: {p.Size} Stock: {p.Stock}";
            if (text.Length > maxTextWidth)
                maxTextWidth = text.Length;
        }

        var leftWidth = Math.Clamp(maxTextWidth + 12, 55, 85);

        var rootWidth = Math.Clamp(Console.WindowWidth - 4, leftWidth + 50, 130);
        var rootHeight = Math.Clamp(Console.WindowHeight - 4, 22, 26);

        new Box(2, 1, rootWidth, rootHeight, " Dashboard ").Draw();

        var leftX = 4;
        var leftHeight = 18;

        new Box(leftX, 3, leftWidth, leftHeight, " Fina butiken ").Draw();

        Console.SetCursorPosition(leftX + 2, 5);
        Console.ForegroundColor = Theme.Hint;
        Console.Write($"Sok: {(string.IsNullOrWhiteSpace(search) ? "(alla)" : search)}");
        Console.ResetColor();

        var y = 7;
        var limit = Math.Min(6, filtered.Count);

        for (var i = 0; i < limit; i++)
        {
            var p = filtered[i];
            var buyKey = (char)('A' + i);

           
            Console.SetCursorPosition(leftX + 2, y);

            var line1 = $"[{i + 1}] {p.Name} ({p.Category})";
            Console.ForegroundColor = Theme.Product;
            Console.Write(line1.Length > leftWidth - 4 ? line1[..(leftWidth - 4)] : line1);

          
            Console.SetCursorPosition(leftX + 2, y + 1);

            var stockText =
                p.Stock == 0 ? "Out ❌" :
                p.Stock <= 3 ? $"Low ⚠ ({p.Stock})" :
                $"In ✔ ({p.Stock})";

            var line2 = $"Pris: {p.Price:0} kr | Size: {p.Size} | {stockText} | [{buyKey}]";

            Console.ForegroundColor = Theme.Price;
            Console.Write(line2.Length > leftWidth - 4 ? line2[..(leftWidth - 4)] : line2);

            Console.ResetColor();

            y += 2;
        }

       
        var rightX = leftX + leftWidth + 2;
        var rightWidth = rootWidth - (rightX - 2) - 2;

        new Box(rightX, 3, rightWidth, 9, " Kundmeny ").Draw();

        for (var i = 0; i < MainItems.Length; i++)
        {
            Console.SetCursorPosition(rightX + 2, 5 + i);
            var prefix = i == menuIndex ? "> " : "  ";
            ConsoleUI.Highlight(prefix + MainItems[i], i == menuIndex);
        }

       
        new Box(rightX, 13, rightWidth, 10, " Varukorg ").Draw();

        var subtotal = CalculateSubtotal(products, cart);
        var shipping = CalculateShipping(subtotal, checkout.Shipping);
        var vat = subtotal * VatRate;
        var total = subtotal + shipping + vat;

        Console.SetCursorPosition(rightX + 2, 15); Console.Write($"Antal: {cart.Values.Sum()}");
        Console.SetCursorPosition(rightX + 2, 16); Console.Write($"Delsumma: {subtotal:0.##} kr");
        Console.SetCursorPosition(rightX + 2, 17); Console.Write($"Moms (25%): {vat:0.##} kr");
        Console.SetCursorPosition(rightX + 2, 18); Console.Write($"Frakt ({checkout.Shipping}): {shipping:0.##} kr");
        Console.SetCursorPosition(rightX + 2, 19); Console.Write($"Total: {total:0.##} kr");
        Console.SetCursorPosition(rightX + 2, 20); Console.Write("K = checkout");

        
        ConsoleUI.Message(status);

        var help = "S=sok | 1-6=detaljer | A-F=kop | Enter=oppna meny | +/-/T i varukorg | ESC=ut";
        Console.SetCursorPosition(4, rootHeight - 1);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(help.PadRight(Math.Max(1, rootWidth - 2)));
        Console.ResetColor();
    }

    private static bool TryAddFromLetterKey(ConsoleKey key, IReadOnlyList<ProductPreview> products, Dictionary<string, int> cart, out string added)
    {
        added = string.Empty;
        if (key < ConsoleKey.A || key > ConsoleKey.F) return false;
        var idx = key - ConsoleKey.A;
        if (idx < 0 || idx >= products.Count) return false;
        var p = products[idx];
        cart[p.Name] = cart.TryGetValue(p.Name, out var count) ? count + 1 : 1;
        added = p.Name;
        return true;
    }

    private static void ShowProductDetails(ProductPreview product)
    {
        Console.Clear();
        new Box(2, 1, Math.Clamp(Console.WindowWidth - 4, 70, 100), 12, " Produktdetaljer ").Draw();
        Console.SetCursorPosition(5, 4); Console.Write($"Namn: {product.Name}");
        Console.SetCursorPosition(5, 5); Console.Write($"Pris: {product.Price:0.##} kr");
        Console.SetCursorPosition(5, 6); Console.Write("Beskrivning: Seed-produkt");
        Console.SetCursorPosition(5, 7); Console.Write("Farg/Storlek/Kategori: visas i seed-data");
        Console.SetCursorPosition(5, 9); Console.Write("Tryck valfri knapp for att ga tillbaka...");
        Console.ReadKey(true);
    }

    private static bool ManageCart(IReadOnlyList<ProductPreview> products, Dictionary<string, int> cart)
    {
        while (true)
        {
            Console.Clear();
            new Box(2, 1, Math.Clamp(Console.WindowWidth - 4, 70, 100), 18, " Varukorg ").Draw();
            var names = cart.Keys.ToList();
            var line = 4;
            for (var i = 0; i < names.Count; i++)
            {
                var name = names[i];
                var price = products.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))?.Price ?? 0m;

                var quantity = cart[name];
                var itemTotal = price * quantity;

                Console.SetCursorPosition(5, line++);

              
                Console.Write($"[{i + 1}] ");

               
                Console.ForegroundColor = Theme.Product;
                Console.Write(name);
                Console.ResetColor();

                
                Console.Write($"  x{quantity}  ");

               
                Console.ForegroundColor = Theme.Price;
                Console.Write($"{price:0.##} kr/st");
                Console.ResetColor();

              
                Console.Write(" = ");

               
                Console.ForegroundColor = Theme.Price;
                Console.Write($"{itemTotal:0.##} kr");
                Console.ResetColor();
            }

            Console.SetCursorPosition(5, 13); Console.Write("+/- = antal, T+[1-9] = ta bort, K = checkout, ESC = tillbaka");
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Escape) return false;
            if (key == ConsoleKey.K) return true;

            var idxKey = Console.ReadKey(true).Key;
            if (idxKey < ConsoleKey.D1 || idxKey > ConsoleKey.D9) continue;
            var idx = idxKey - ConsoleKey.D1;
            if (idx < 0 || idx >= names.Count) continue;
            var selected = names[idx];

            if (key == ConsoleKey.Add || key == ConsoleKey.OemPlus) cart[selected]++;
            if (key == ConsoleKey.Subtract || key == ConsoleKey.OemMinus) cart[selected] = Math.Max(1, cart[selected] - 1);
            if (key == ConsoleKey.T || key == ConsoleKey.Delete) cart.Remove(selected);
        }
    }

    private static void ConfigureShipping(CheckoutState checkout)
    {
        Console.Clear();
        new Box(2, 1, Math.Clamp(Console.WindowWidth - 4, 70, 100), 14, " Frakt ").Draw();

       
        Console.SetCursorPosition(5, 4);
        Console.ForegroundColor = Theme.DefaultText;
        Console.Write("1 = ");

        Console.ForegroundColor = Theme.Product;
        Console.Write("Standard ");

        Console.ForegroundColor = Theme.DefaultText;
        Console.Write("(");

        Console.ForegroundColor = Theme.Price;
        Console.Write("40 kr");

        Console.ForegroundColor = Theme.DefaultText;
        Console.Write(")");
        Console.ResetColor();

      
        Console.SetCursorPosition(5, 5);
        Console.ForegroundColor = Theme.DefaultText;
        Console.Write("2 = ");

        Console.ForegroundColor = Theme.Product;
        Console.Write("Express ");

        Console.ForegroundColor = Theme.DefaultText;
        Console.Write("(");

        Console.ForegroundColor = Theme.Price;
        Console.Write("90 kr");

        Console.ForegroundColor = Theme.DefaultText;
        Console.Write(")");
        Console.ResetColor();

      
        Console.SetCursorPosition(5, 6);
        Console.ForegroundColor = Theme.Hint;
        Console.Write("Fri frakt over 500 kr.");
        Console.ResetColor();

      
        Console.SetCursorPosition(5, 8);
        Console.ForegroundColor = Theme.Message;
        Console.Write("Valj frakt (1/2): ");
        Console.ResetColor();

        var k = Console.ReadKey(true).Key;
        checkout.Shipping = k == ConsoleKey.D2 ? ShippingType.Express : ShippingType.Standard;

       
        Console.ForegroundColor = Theme.Hint;
        checkout.Name = Prompt("Namn:");
        Console.ResetColor();

      
        Console.ForegroundColor = Theme.Hint;
        checkout.Address = Prompt("Adress:");
        Console.ResetColor();
    }

    private static string RunCheckout(IReadOnlyList<ProductPreview> products, Dictionary<string, int> cart, CheckoutState checkout)
    {
        if (cart.Count == 0) return "Varukorg ar tom.";

        if (string.IsNullOrWhiteSpace(checkout.Name) || string.IsNullOrWhiteSpace(checkout.Address))
        {
            ConfigureShipping(checkout);
        }

        if (string.IsNullOrWhiteSpace(checkout.Name) || string.IsNullOrWhiteSpace(checkout.Address))
        {
            return "Fel: namn och adress maste fyllas i.";
        }

        var subtotal = CalculateSubtotal(products, cart);
        var vat = subtotal * VatRate;
        var shipping = CalculateShipping(subtotal, checkout.Shipping);
        var total = subtotal + vat + shipping;

        var purchasedItems = cart.Select(kvp =>
        {
            var product = products.FirstOrDefault(p => p.Name.Equals(kvp.Key, StringComparison.OrdinalIgnoreCase));
            var unitPrice = product?.Price ?? 0m;

            return new
            {
                Name = kvp.Key,
                Quantity = kvp.Value,
                UnitPrice = unitPrice,
                RowTotal = unitPrice * kvp.Value
            };
        }).ToList();

        // BETALNING 
        Console.Clear();
        Console.ResetColor();
        Console.BackgroundColor = Theme.Background;

        new Box(2, 1, Math.Clamp(Console.WindowWidth - 4, 70, 100), 18, " Betalning ").Draw();

        Console.SetCursorPosition(5, 4);
        Console.ForegroundColor = Theme.Hint;
        Console.Write("Kund: ");
        Console.ForegroundColor = Theme.DefaultText;
        Console.Write(checkout.Name);

        Console.SetCursorPosition(5, 5);
        Console.ForegroundColor = Theme.Hint;
        Console.Write("Adress: ");
        Console.ForegroundColor = Theme.DefaultText;
        Console.Write(checkout.Address);

        Console.SetCursorPosition(5, 7);
        Console.ForegroundColor = Theme.Hint;
        Console.Write("Delsumma: ");
        Console.ForegroundColor = Theme.Price;
        Console.Write($"{subtotal:0.##} kr");

        Console.SetCursorPosition(5, 8);
        Console.ForegroundColor = Theme.Hint;
        Console.Write("Moms: ");
        Console.ForegroundColor = Theme.Price;
        Console.Write($"{vat:0.##} kr");

        Console.SetCursorPosition(5, 9);
        Console.ForegroundColor = Theme.Hint;
        Console.Write("Frakt: ");
        Console.ForegroundColor = Theme.Price;
        Console.Write($"{shipping:0.##} kr");

        Console.SetCursorPosition(5, 10);
        Console.ForegroundColor = Theme.Hint;
        Console.Write("Total: ");
        Console.ForegroundColor = Theme.Price;
        Console.Write($"{total:0.##} kr");

        Console.SetCursorPosition(5, 12);
        Console.ForegroundColor = Theme.Message;
        Console.Write("Valj betalning: ");
        Console.ForegroundColor = Theme.DefaultText;
        Console.Write("1=Kort, 2=Swish");

        Console.ResetColor();
        var key = Console.ReadKey(true).Key;
        var payment = key == ConsoleKey.D2 ? PaymentType.Swish : PaymentType.Card;

        // SUCCESS MESSAGE 
        Console.Clear();
        Console.ForegroundColor = Theme.Message;
        Console.WriteLine("Betalning genomförd!");

        Console.ForegroundColor = Theme.Hint;
        Console.WriteLine("Tryck valfri knapp för att visa kvittot...");

        Console.ResetColor();
        Console.ReadKey(true);

        cart.Clear();

        // KVITTENS 
        var deliveryDays = Random.Shared.Next(1, 8);
        var paidAt = DateTime.Now;

        Console.Clear();
        Console.ResetColor();
        Console.BackgroundColor = Theme.Background;

        new Box(2, 1, Math.Clamp(Console.WindowWidth - 4, 80, 110), 20, " Kvittens ").Draw();

        Console.SetCursorPosition(5, 3);
        Console.ForegroundColor = Theme.Message;
        Console.Write("Betalning genomford!");

        Console.SetCursorPosition(5, 4);
        Console.ForegroundColor = Theme.Hint;
        Console.Write("Datum: ");
        Console.ForegroundColor = Theme.DefaultText;
        Console.Write($"{paidAt:yyyy-MM-dd HH:mm}");

        Console.SetCursorPosition(5, 5);
        Console.ForegroundColor = Theme.Hint;
        Console.Write("Kund: ");
        Console.ForegroundColor = Theme.DefaultText;
        Console.Write(checkout.Name);

        Console.SetCursorPosition(5, 6);
        Console.ForegroundColor = Theme.Hint;
        Console.Write("Adress: ");
        Console.ForegroundColor = Theme.DefaultText;
        Console.Write(checkout.Address);

        Console.SetCursorPosition(5, 7);
        Console.ForegroundColor = Theme.Hint;
        Console.Write("Metod: ");
        Console.ForegroundColor = Theme.Product;
        Console.Write(payment);

        
        var row = 9;
        foreach (var item in purchasedItems.Take(6))
        {
            Console.SetCursorPosition(5, row++);

            Console.ForegroundColor = Theme.Product;
            Console.Write($"- {item.Name} ");

            Console.ForegroundColor = Theme.Hint;
            Console.Write($"x{item.Quantity} ");

            Console.ForegroundColor = Theme.Price;
            Console.Write($"{item.RowTotal:0.##} kr");
        }

        
        Console.SetCursorPosition(5, 14);
        Console.ForegroundColor = Theme.Hint;
        Console.Write("Delsumma: ");
        Console.ForegroundColor = Theme.Price;
        Console.Write($"{subtotal:0.##} kr");

        Console.SetCursorPosition(5, 15);
        Console.ForegroundColor = Theme.Hint;
        Console.Write("Moms: ");
        Console.ForegroundColor = Theme.Price;
        Console.Write($"{vat:0.##} kr");

        Console.SetCursorPosition(40, 15);
        Console.ForegroundColor = Theme.Hint;
        Console.Write("Frakt: ");
        Console.ForegroundColor = Theme.Price;
        Console.Write($"{shipping:0.##} kr");

        Console.SetCursorPosition(5, 16);
        Console.ForegroundColor = Theme.Hint;
        Console.Write("TOTAL: ");
        Console.ForegroundColor = Theme.Price;
        Console.Write($"{total:0.##} kr");

        Console.SetCursorPosition(5, 18);
        Console.ResetColor();
        Console.Write("Tryck valfri knapp...");
        Console.ReadKey(true);

        try
        {
            var receiptsDir = Path.Combine(Environment.CurrentDirectory, "receipts");
            Directory.CreateDirectory(receiptsDir);

            var filePath = Path.Combine(receiptsDir, $"kvitto_{DateTime.Now:yyyyMMdd_HHmmss}.txt");

            var sb = new StringBuilder();
            sb.AppendLine("Kvittens");
            sb.AppendLine($"Datum: {paidAt}");
            sb.AppendLine($"Kund: {checkout.Name}");
            sb.AppendLine($"Adress: {checkout.Address}");
            sb.AppendLine($"Metod: {payment}");
            sb.AppendLine();

            foreach (var item in purchasedItems)
            {
                sb.AppendLine($"{item.Name} x{item.Quantity} = {item.RowTotal:0.##} kr");
            }

            sb.AppendLine();
            sb.AppendLine($"Total: {total:0.##} kr");

            File.WriteAllText(filePath, sb.ToString());
        }
        catch { }

        return $"Kvittens klar ({payment}). Total {total:0.##} kr.";
    }

    private static string Prompt(string label)
    {
        Console.CursorVisible = true;
        Console.SetCursorPosition(2, Console.WindowHeight - 3);
        Console.Write(new string(' ', Math.Max(1, Console.WindowWidth - 4)));
        Console.SetCursorPosition(2, Console.WindowHeight - 3);
        Console.Write($"{label} ");
        var value = Console.ReadLine() ?? string.Empty;
        Console.CursorVisible = false;
        return value.Trim();
    }

    private static decimal CalculateSubtotal(IReadOnlyList<ProductPreview> products, Dictionary<string, int> cart)
    {
        decimal subtotal = 0;
        foreach (var p in products)
        {
            if (cart.TryGetValue(p.Name, out var count)) subtotal += p.Price * count;
        }
        return subtotal;
    }

    private static decimal CalculateShipping(decimal subtotal, ShippingType shipping)
    {
        if (subtotal >= FreeShippingLimit) return 0m;
        return shipping == ShippingType.Standard ? StandardShipping : ExpressShipping;
    }

    private static void PrintFallback(IReadOnlyList<ProductPreview> products)
    {
        Console.WriteLine("WebShop fallback");
        Console.WriteLine("Startsida med utvalda produkter:");
        foreach (var p in products.Take(6))
        {
            Console.WriteLine($"- {p.Name} ({p.Price.ToString("0.##", CultureInfo.InvariantCulture)} kr)");
        }
        Console.WriteLine("Features: sok, produktdetaljer, varukorg, frakt, betalning, kvittens.");
    }
    private static void ShowWelcomeScreen()
    {
        Console.Clear();
        Console.ResetColor();
        Console.BackgroundColor = Theme.Background;

        new Box(2, 1, Math.Clamp(Console.WindowWidth - 4, 70, 100), 20, " WebShop ").Draw();

        Console.SetCursorPosition(5, 5);
        Console.ForegroundColor = Theme.Message;
        Console.WriteLine("✨ Välkommen till WebShop!");

        Console.SetCursorPosition(5, 7);
        Console.ForegroundColor = Theme.Hint;
        Console.WriteLine("Handla enkelt och snabbt 👕👟");

        Console.SetCursorPosition(5, 10);
        Console.ForegroundColor = Theme.Product;
        Console.WriteLine("[ ENTER ] Starta");

        Console.SetCursorPosition(5, 12);
        Console.ForegroundColor = Theme.Hint;
        Console.WriteLine("Tryck ENTER för att fortsätta...");

        Console.ResetColor();

        while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
    }
}
