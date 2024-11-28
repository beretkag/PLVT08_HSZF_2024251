using System.Runtime.CompilerServices;

namespace PLVT08_HSZF_2024251.Console
{
    public class Menu
    {
        private List<Option> Options { get; set; } = new List<Option>();
        public string Title { get; set; }
        private string Headline { get; }

        public Action<Menu> RefreshOptions { get; set; }

        public Menu(string title, string headline="")
        {
            Title = title;
            Headline = headline;
        }

        

        public void AddOption(Option option)
        {
            Options.Add(option);
        }
        public void RemoveOptions()
        {
            Options.Clear();
        }

        public void Show()
        {
            System.Console.Clear();
            int selectedIndex = 0;
            ConsoleKey key;

            System.Console.CursorVisible = false;

            do
            {
                RefreshOptions(this);
                System.Console.SetCursorPosition(0, 0);
                System.Console.WriteLine($"-- {Title} --\n");
                if (Headline.Length > 0)
                {
                    System.Console.WriteLine(Headline);
                }

                for (int i = 0; i < Options.Count; i++)
                {
                    Options[i].Draw(i == selectedIndex);
                    System.Console.WriteLine();
                }

                System.Console.ResetColor();

                key = System.Console.ReadKey().Key;
                int row = selectedIndex + (Headline.Length > 0 ? 3 : 2);

                if (key == ConsoleKey.UpArrow)
                {
                    selectedIndex = selectedIndex == 0 ? Options.Count - 1 : selectedIndex - 1;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    selectedIndex = selectedIndex == Options.Count - 1 ? 0 : selectedIndex + 1;
                }
                else if (key == ConsoleKey.Enter)
                {
                    if (selectedIndex < Options.Count)
                    {
                        Options[selectedIndex].Execute(row);
                    }
                }
                ClearBelowCursor(row);

            } while (key != ConsoleKey.Escape);

            System.Console.Clear();
        }

        private void ClearBelowCursor(int row)
        {
            for (int i = row; i < System.Console.WindowHeight; i++)
            {
                System.Console.SetCursorPosition(0, i);
                System.Console.Write(new string(' ', System.Console.WindowWidth));
            }
        }
    }
}
