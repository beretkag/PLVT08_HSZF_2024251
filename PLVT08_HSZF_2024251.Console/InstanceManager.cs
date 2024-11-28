using Microsoft.Extensions.Options;
using PLVT08_HSZF_2024251.Model;
using System.Reflection;


namespace PLVT08_HSZF_2024251.Console
{
    public static class InstanceManager<T>
    {
        public static T Create()
        {
            object instance = Activator.CreateInstance(typeof(T));
            PropertyInfo[] properties = typeof(T).GetProperties().Where(x => x.GetCustomAttribute<CreateAttribute>() != null).ToArray();
            int currentPropertyIndex = 0;

            // Tulajdonságok megjelenítése
            System.Console.Clear();
            System.Console.ResetColor();
            System.Console.CursorVisible = false;
            for (int i = 0; i < properties.Length; i++)
            {
                System.Console.WriteLine($"{properties[i].GetCustomAttribute<DisplayNameAttribute>().Name}: ");
            }

            // Végighalad az adattagokon
            while (currentPropertyIndex < properties.Length)
            {
                if (properties[currentPropertyIndex].PropertyType == typeof(bool))              //BOOLEAN
                {
                    System.Console.CursorVisible = false;

                    int boolIndex = 0;
                    ConsoleKey key = ConsoleKey.NoName;
                    string[] options = { "Igaz", "Hamis" };

                    while (key != ConsoleKey.Enter)
                    {
                        System.Console.SetCursorPosition(properties[currentPropertyIndex].GetCustomAttribute<DisplayNameAttribute>().Name.Length + 2, currentPropertyIndex);
                        System.Console.Write(new string(' ', System.Console.WindowWidth - (properties[currentPropertyIndex].GetCustomAttribute<DisplayNameAttribute>().Name.Length + 2)));
                        System.Console.SetCursorPosition(properties[currentPropertyIndex].GetCustomAttribute<DisplayNameAttribute>().Name.Length + 2, currentPropertyIndex);

                        for (int j = 0; j < options.Length; j++)
                        {
                            System.Console.ResetColor();
                            if (j == boolIndex)
                            {
                                System.Console.BackgroundColor = ConsoleColor.White;
                                System.Console.ForegroundColor = ConsoleColor.Black;
                            }
                            System.Console.Write(options[j]);
                            System.Console.ResetColor();
                            if (j < options.Length - 1)
                            {
                                System.Console.Write(" / ");
                            }
                        }

                        key = System.Console.ReadKey().Key;
                        if (key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow)
                        {
                            boolIndex = (boolIndex == 0) ? options.Length - 1 : boolIndex - 1;
                        }
                    }
                    System.Console.SetCursorPosition(properties[currentPropertyIndex].GetCustomAttribute<DisplayNameAttribute>().Name.Length + 2, currentPropertyIndex);
                    System.Console.Write(new string(' ', System.Console.WindowWidth - (properties[currentPropertyIndex].GetCustomAttribute<DisplayNameAttribute>().Name.Length + 2)));
                    System.Console.SetCursorPosition(properties[currentPropertyIndex].GetCustomAttribute<DisplayNameAttribute>().Name.Length + 2, currentPropertyIndex);
                    System.Console.Write(boolIndex == 0 ? "Igaz" : "Hamis");

                    properties[currentPropertyIndex].SetValue(instance, boolIndex == 0);
                    currentPropertyIndex++;
                }
                else if (properties[currentPropertyIndex].PropertyType == typeof(string))       //STRING
                {
                    HandleCreateInput<string>(ref properties, ref instance, ref currentPropertyIndex, "Hibás fromátum, max 15 karaktert írhastz be!", (string input, out string y) => {
                        y = input; 
                        if (input.Length > 15) return false;
                        return true;
                    });
                }
                else if (properties[currentPropertyIndex].PropertyType == typeof(int))          //INT
                {
                    HandleCreateInput<int>(ref properties, ref instance, ref currentPropertyIndex, "Hibás formátum (Egész számot adjon meg!)", int.TryParse);
                }
                else if (properties[currentPropertyIndex].PropertyType == typeof(double))       //DOUBLE
                {
                    HandleCreateInput<double>(ref properties, ref instance, ref currentPropertyIndex, "Hibás formátum, tizedes számot adjon meg (\",\"-vel elválasztva)", double.TryParse);
                }
                else if (properties[currentPropertyIndex].PropertyType == typeof(DateTime))     //DATETIME
                {
                    HandleCreateInput<DateTime>(ref properties, ref instance, ref currentPropertyIndex, "Hibás formátum, dátumot adjon meg (ÉÉÉÉ-HH-NN)", DateTime.TryParse);
                }
            }

            System.Console.Clear();
            return (T)instance;
        }

        private delegate bool TryParseDelegate<V>(string input, out V result);
        private static void HandleCreateInput<V>(ref PropertyInfo[] properties, ref object? instance, ref int currentPropertyIndex, string errorMessage, TryParseDelegate<V> tryParse)
        {
            System.Console.SetCursorPosition(properties[currentPropertyIndex].GetCustomAttribute<DisplayNameAttribute>().Name.Length + 2, currentPropertyIndex);
            System.Console.Write(new string(' ', System.Console.WindowWidth - (properties[currentPropertyIndex].GetCustomAttribute<DisplayNameAttribute>().Name.Length + 2)));
            System.Console.SetCursorPosition(properties[currentPropertyIndex].GetCustomAttribute<DisplayNameAttribute>().Name.Length + 2, currentPropertyIndex);
            System.Console.CursorVisible = true;
            string input = System.Console.ReadLine();
            System.Console.CursorVisible = false;

            if (tryParse(input, out V value))
            {
                properties[currentPropertyIndex].SetValue(instance, value);
                currentPropertyIndex++;

                System.Console.SetCursorPosition(0, properties.Length + 2);
                System.Console.Write(new string(' ', System.Console.WindowWidth));
            }
            else
            {
                System.Console.SetCursorPosition(0, properties.Length + 2);
                System.Console.WriteLine(errorMessage);
            }
        }
    }
}
