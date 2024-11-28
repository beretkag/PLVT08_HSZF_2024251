using PLVT08_HSZF_2024251.Application.Services;
using PLVT08_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLVT08_HSZF_2024251.Console.Menus
{
    public static class MailMenu
    {
        public static IMailService mailService { get; set; }
        public static Menu MailChoose()
        {
            Menu mailMenu = new Menu("Beérkezett levelek");

            mailMenu.RefreshOptions = (Menu menu) => {
                menu.RemoveOptions();


                ICollection<Mail> mails = mailService.GetByFilter(x => x.PersonId == PersonMenu.user.Id);
                foreach (Mail mail in mails)
                {
                    Option option = new Option(mail.Date.ToShortDateString() + new string(' ', 15 - mail.Date.ToShortDateString().Length));
                    option.AddInnerOption(new Option("Megtekintés", () =>
                    {
                        MailDetails(mail);
                    }));
                    option.AddInnerOption(new Option("Törlés", () =>
                    {
                        mailService.Delete(mail);
                    }));
                    menu.AddOption(option);
                }

                //TODO Mails
            };

            return mailMenu;
        }

        private static void MailDetails(Mail mail)
        {
            System.Console.Clear();

            System.Console.WriteLine($"Dátum: {mail.Date}");
            System.Console.WriteLine($"Tartalom:");
            System.Console.WriteLine(mail.Content);

            System.Console.ForegroundColor = ConsoleColor.Black;
            ConsoleKey key;
            do key = System.Console.ReadKey().Key;
            while (key != ConsoleKey.Enter && key != ConsoleKey.Escape);
            System.Console.Clear();
            System.Console.ResetColor();
        }
    }
}
