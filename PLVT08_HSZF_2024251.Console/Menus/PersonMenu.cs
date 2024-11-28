using PLVT08_HSZF_2024251.Application.Services;
using PLVT08_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLVT08_HSZF_2024251.Console
{
    internal static class PersonMenu
    {
        public static Person user;
        public static IPersonService personService { get; set; }
        public static void RefreshUser()
        {

            user = personService.GetByFilter(x => x.Id == user.Id).First();
        }
        public static Menu PersonChoose()
        {
            Menu personMenu = new Menu("Felhasználók");

            personMenu.RefreshOptions = (Menu menu) =>
            {
                menu.RemoveOptions();
                menu.AddOption(new Option("Új Hozzáadása", () =>
                {
                    personService.Add(InstanceManager<Person>.Create());
                }));


                ICollection<Person> persons = personService.GetAll();
                foreach (Person person in persons)
                {
                    Option option = new Option(person.Name + new string(' ', 15 - person.Name.Length));
                    option.AddInnerOption(new Option("Belépés", () =>
                    {
                        user = person;
                    }, UserMenu.UserChoose("Tárolók")));
                    option.AddInnerOption(new Option("Törlés", () =>
                    {
                        personService.Delete(person);
                    }));
                    menu.AddOption(option);
                }
            };

            return personMenu;
            
        }


    }
}
