

using PLVT08_HSZF_2024251.Application.Services;
using System.Reflection.PortableExecutable;

namespace PLVT08_HSZF_2024251.Console.Menus
{
    public static class FileMenu
    {
        public static IFileService fileService { get; set; }
        public static void FileReading()
        {
            while (BoolQuestion("Szeretne fájlt beolvasni? "))
            {
                System.Console.Write("Fájlnév: ");
                fileService.Import(System.Console.ReadLine());
                System.Console.WriteLine("Sikeres fájlbeolvasás");
                WaitForEnd();
            }
        }

        private static bool BoolQuestion(string title)
        {
            System.Console.Clear();
            System.Console.CursorVisible = false;
            System.Console.Write(title);

            int boolIndex = 0;
            ConsoleKey key = ConsoleKey.NoName;
            string[] options = { "Igaz", "Hamis" };

            while (key != ConsoleKey.Enter)
            {
                System.Console.SetCursorPosition(title.Length, 0);
                System.Console.Write(new string(' ', System.Console.WindowWidth - title.Length));
                System.Console.SetCursorPosition(title.Length, 0);

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
            System.Console.Clear();
            return boolIndex == 0;
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
