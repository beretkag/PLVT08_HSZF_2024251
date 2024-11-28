using Microsoft.Extensions.Options;
using PLVT08_HSZF_2024251.Application.Services;
using PLVT08_HSZF_2024251.Model;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;


namespace PLVT08_HSZF_2024251.Console
{
    public static class ProductMenu
    {
        public static IProductService productService { get; set; }
        public static IFavoriteProductService favoriteProductService { get; set; }


        public static Menu ProductChoose(bool storeInFridge)
        {
            Menu productMenu = new Menu(storeInFridge ? "Hűtő" : "Kamra", Product.HeadLineByUser());

            productMenu.RefreshOptions = (Menu menu) =>
            {
                menu.RemoveOptions();
                menu.AddOption(new Option("Új Hozzáadása", () =>
                {
                    productService.Add(InstanceManager<Product>.Create());
                }));


                ICollection<Product> products = productService.GetByFilter(x => x.StoreInFridge == storeInFridge);
                foreach (Product product in products)
                {
                    menu.AddOption(OptionCreator(product));
                }
            };

            return productMenu;
        }

        private static Option OptionCreator(Product product)
        {
            Option option = new Option(product.ToStringByUser(PersonMenu.user));
            option.AddInnerOption(new Option("Kedvenc", () =>
            {
                string productId = product.Id;
                if (PersonMenu.user.FavoriteProducts.Count(x => x.Id == productId) > 0)
                {
                    favoriteProductService.Delete(product, PersonMenu.user);
                }
                else
                {
                    favoriteProductService.Add(PersonMenu.user, product);
                }
                PersonMenu.RefreshUser();
                option.Title = product.ToStringByUser(PersonMenu.user);

            }));
            option.AddInnerOption(new Option("Felhasználás", () => { ChangeQuantity(option, product, -1, "Fogyasztás mennyisége: "); }));
            if (PersonMenu.user.ResponsibleForPurchase)
            {
                option.AddInnerOption(new Option("Beszerzés", () => { ChangeQuantity(option, product, 1, "Beszerzés mennyisége: "); }));
            }
            option.AddInnerOption(new Option("Törlés", () =>
            {
                productService.Delete(product);
            }));
            return option;
        }


        private static void ChangeQuantity(Option option, Product product, int irany, string line) 
        {
            System.Console.Clear();
            System.Console.Write(line);

            bool goodQuantity = false;
            double quantity = 0;
            while (!goodQuantity)
            {
                string input;
                bool goodTpye = false;

                while (!goodTpye)
                {
                    System.Console.SetCursorPosition(line.Length, 0);
                    System.Console.Write(new string(' ', System.Console.WindowWidth - (line.Length)));
                    System.Console.SetCursorPosition(line.Length, 0);
                    System.Console.CursorVisible = true;
                    input = System.Console.ReadLine();
                    System.Console.CursorVisible = false;
                    if (double.TryParse(input, out quantity))
                    {
                        goodTpye = true;
                        System.Console.SetCursorPosition(0, 2);
                        System.Console.Write(new string(' ', System.Console.WindowWidth));
                    }
                    else
                    {
                        System.Console.SetCursorPosition(0, 2);
                        System.Console.Write("Hibás formátum, tizedes számot adjon meg (\",\"-vel elválasztva)");
                    }
                }
                goodQuantity = productService.AddQuantity(product, quantity * irany);
                if(!goodQuantity)
                {
                    System.Console.SetCursorPosition(0, 2);
                    System.Console.Write("Hibás bemenet, ügyelj a tároló méretére");
                }
            }
            option.Title = product.ToStringByUser(PersonMenu.user);
            System.Console.Clear();
        }
    }
}
