using HustlersAB.Admin.Menus;
using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Presentation.MenuHandlers;

namespace WebShop.Presentation.Menus
{
    public class AdminStatisticsMenu : MenuBase
    {
        private readonly AdminProductHandler _productHandler;
        private readonly AdminCategoryHandler _categoryHandler;

        public AdminStatisticsMenu(
            AdminProductHandler productHandler,
            AdminCategoryHandler categoryHandler)
        {
            _productHandler = productHandler;
            _categoryHandler = categoryHandler;

            _options = new[]
            {
            "Bäst säljande produkter",
            "Populäraste kategori",
            "Tillbaka"
        };
        }

        protected override bool ExecuteChoice(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0:
                    ShowBestSellers();
                    return false;

                case 1:
                    ShowPopularCategory();
                    return false;

                case 2:
                    return true;
            }

            return false;
        }

        private void ShowBestSellers()
        {
            Console.Clear();

            var products = _productHandler
                .GetBestProductsAsync(3)
                .GetAwaiter()
                .GetResult();

            Console.WriteLine("=== Bäst säljande produkter ===\n");

            foreach (var p in products)
            {
                Console.WriteLine($"- {p.Namn} ({p.Pris} kr)");
            }

            Console.WriteLine("\nTryck valfri tangent...");
            Console.ReadKey(true);
        }

        private void ShowPopularCategory()
        {
            Console.Clear();

            var category = _categoryHandler
                .GetPopularCategoriesAsync()
                .GetAwaiter()
                .GetResult();

            Console.WriteLine("=== Populäraste kategori ===\n");

            Console.WriteLine(category != null ? category.Namn : "Ingen data");

            Console.WriteLine("\nTryck valfri tangent...");
            Console.ReadKey(true);
        }
    }
}
