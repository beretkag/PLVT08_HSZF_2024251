
using PLVT08_HSZF_2024251.Application.Services;
using PLVT08_HSZF_2024251.Model;

namespace PLVT08_HSZF_2024251.Console.Menus
{
    public class StorageMenu
    {
        public static IProductService productService { get; set; }

        public static IStorageService storageService { get; set; }

        public static Menu StorageChoose(string title, bool storeInFridge)
        {
            Menu storageMenu = new Menu(title);

            storageMenu.RefreshOptions = (Menu menu) => {
                menu.RemoveOptions();
                menu.AddOption(new Option("Teljes kapacitás módosítása", () => { ChangeCapacity(storageService.GetByFilter(x => x.Id == storeInFridge).First()); }));
                menu.AddOption(new Option("Tárolókapacitás", () => { ActualStorageCapacity(storeInFridge, 10); }));
                menu.AddOption(new Option("Közeli lejáratú termékek", () => { ListProducts(x => x.BestBefore < DateTime.Now.AddDays(7) && x.StoreInFridge == storeInFridge, $"Közeli lejáratú termékek ({title})"); }));
                menu.AddOption(new Option("Kifogyóban lévő termékek", () => { ListProducts(x => x.CriticalLevel >= x.Quantity && x.StoreInFridge == storeInFridge, $"Kifogyóban lévő termékek ({title})"); }));
                menu.AddOption(new Option("Termékek exportálása", () => { ExportProducts(storageService.GetByFilter(x => x.Id == storeInFridge).First()); }));
                menu.AddOption(new Option("Termékek kezelése", ProductMenu.ProductChoose(storeInFridge)));
            };

            return storageMenu;
        }

        private static void ChangeCapacity(Storage storage)
        {
            System.Console.Clear();

            System.Console.WriteLine($"Teljes méret: {storage.Capacity}");
            System.Console.WriteLine($"Felhasznált hely: {storage.UsedCapacity}");
            System.Console.WriteLine($"Új méret: ");


            bool goodQuantity = false;
            int quantity = 0;
            while (!goodQuantity)
            {
                string input;
                bool goodTpye = false;

                while (!goodTpye)
                {
                    System.Console.SetCursorPosition(10, 2);
                    System.Console.Write(new string(' ', System.Console.WindowWidth - 10));
                    System.Console.SetCursorPosition(10, 2);
                    System.Console.CursorVisible = true;
                    input = System.Console.ReadLine();
                    System.Console.CursorVisible = false;
                    if (int.TryParse(input, out quantity))
                    {
                        goodTpye = true;
                        System.Console.SetCursorPosition(0, 4);
                        System.Console.Write(new string(' ', System.Console.WindowWidth));
                    }
                    else
                    {
                        System.Console.SetCursorPosition(0, 4);
                        System.Console.Write("Hibás formátum (Egész számot adjon meg!)");
                    }
                }
                goodQuantity = storageService.ChangeQuantity(storage, quantity);
                if (!goodQuantity)
                {
                    System.Console.SetCursorPosition(0, 4);
                    System.Console.Write("Hibás bemenet, ügyelj a tároló méretére");
                }
            }
            System.Console.Clear();
            
        }

        private static void ExportProducts(Storage storage)
        {
            System.Console.Clear();

            DateTime now = DateTime.Now;
            string directoryName = $"{now.Day :00}{now.Month :00}{now.Year}";
            if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

            TimeSpan nowtime = now.TimeOfDay;
            string fileName = $"{nowtime.Hours :00}{nowtime.Minutes :00}{now.Second :00}";

            string content = Product.HeadLine() + "\n";
            foreach (Product product in storage.Products)
            {
                content += $"{product.ToString()}\n";
            }
            string fullPath = $"{directoryName}/HouseholdRegisterExport_{fileName}.txt";

            File.WriteAllText(fullPath, content);
            System.Console.WriteLine($"Az adatok exportálása megtörtént. ({fullPath})");

            WaitForEnd();
        }

        private static void ActualStorageCapacity(bool storeInFridge, double criticalLevelPercent)
        {
            System.Console.Clear();
            Storage storage = storageService.GetByFilter(x => x.Id == storeInFridge).First();

            System.Console.WriteLine($"Teljes méret: {storage.Capacity}");
            System.Console.WriteLine($"Felhasznált hely: {storage.UsedCapacity}");
            System.Console.WriteLine($"Szabad helyek aránya: {storageService.ActualStorageCapacity(storage, criticalLevelPercent): 0.00}%");

            WaitForEnd();
        }

        private static void ListProducts(Func<Product, bool> filter, string title)
        {
            System.Console.Clear();
            System.Console.WriteLine($"-- {title} --\n");
            System.Console.WriteLine(Product.HeadLine());
            ICollection<Product> products = productService.GetByFilter(filter);
            foreach (Product product in products)
            {
                System.Console.WriteLine(product.ToString());
            }

            WaitForEnd();
        }

        private static void WaitForEnd()
        {
            System.Console.ForegroundColor = ConsoleColor.Black;
            ConsoleKey key;
            do key = System.Console.ReadKey().Key;
            while (key != ConsoleKey.Enter && key != ConsoleKey.Escape);
            System.Console.Clear();
            System.Console.ResetColor();
        }
    }
}
