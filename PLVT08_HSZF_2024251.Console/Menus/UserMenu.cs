using PLVT08_HSZF_2024251.Console.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLVT08_HSZF_2024251.Console
{
    public static class UserMenu
    {
        public static Menu UserChoose(string username)
        {
            Menu userMenu = new Menu(username);

            userMenu.RefreshOptions = (Menu menu) => {
                menu.RemoveOptions();
                menu.AddOption(new Option("Hűtő", StorageMenu.StorageChoose("Hűtő" ,true)));
                menu.AddOption(new Option("Kamra", StorageMenu.StorageChoose("Kamra", false)));
                menu.AddOption(new Option("Beérkezett levelek", MailMenu.MailChoose()));
            };

            return userMenu;
        }
    }
}
