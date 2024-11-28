using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace PLVT08_HSZF_2024251.Console
{
    public class Option
    {
        public string Title { get; set; }
        public Action Action { get; set; }
        public Menu SubMenu { get; set; }

        List<Option> InnerOptions { get; set; } = new List<Option>();

        public Option(string title)
        {
            Title = title;
        }

        public Option(string title, Action action)
        {
            Title = title;
            Action = action;
        }

        public Option(string title, Menu subMenu)
        {
            Title = title;
            SubMenu = subMenu;
        }

        public Option(string title, Action action, Menu subMenu) : this(title, action)
        {
            SubMenu = subMenu;
        }

        public void Execute(int row = 0)
        {
            Action?.Invoke();
            SubMenu?.Show();

            if (InnerOptions.Count != 0)
            {
                ShowInerOptions(row);
            }

        }

        private void ShowInerOptions(int row)
        {
            int selectedIndex = 0;
            ConsoleKey key;

            do
            {
                System.Console.SetCursorPosition(0, row);
                System.Console.Write(this.Title);

                for (int i = 0; i < InnerOptions.Count; i++)
                {
                    InnerOptions[i].Draw(i == selectedIndex);
                    System.Console.Write(new string(' ', 15 - InnerOptions[i].Title.Length));
                }

                key = System.Console.ReadKey().Key;
                if (key == ConsoleKey.LeftArrow)
                {
                    selectedIndex = selectedIndex == 0 ? InnerOptions.Count - 1 : selectedIndex - 1;
                }
                else if (key == ConsoleKey.RightArrow)
                {
                    selectedIndex = selectedIndex == InnerOptions.Count - 1 ? 0 : selectedIndex + 1;
                }

            } while (key != ConsoleKey.Escape && key != ConsoleKey.Enter);

            if (key == ConsoleKey.Enter)
            {
                System.Console.SetCursorPosition(0, row);
                Draw(true);
                System.Console.Write(new string(' ', System.Console.WindowWidth - System.Console.GetCursorPosition().Left));

                InnerOptions[selectedIndex].Execute();
            }

        }

        public void AddInnerOption(Option option)
        {
            InnerOptions.Add(option);
        }

        public void Draw(bool chosen)
        {
            if (chosen)
            {
                System.Console.BackgroundColor = ConsoleColor.White;
                System.Console.ForegroundColor = ConsoleColor.Black;
            }
            System.Console.Write(this.Title);
            System.Console.ResetColor();
            int left = System.Console.GetCursorPosition().Left;
            System.Console.Write(new string(' ', System.Console.WindowWidth - left));
            System.Console.SetCursorPosition(left, System.Console.GetCursorPosition().Top);
        }
    }
}
