using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PLVT08_HSZF_2024251.Application.Services;
using PLVT08_HSZF_2024251.Console.Menus;
using PLVT08_HSZF_2024251.Model;
using PLVT08_HSZF_2024251.Persistence.MsSql;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;
using System;

namespace PLVT08_HSZF_2024251.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            var app = Host.CreateDefaultBuilder()
                .ConfigureServices((hostCtx, services) =>
                {
                    //Database
                    services.AddDbContext<HouseholdContext>();
                    //DataProviders
                    services.AddSingleton<IProductProvider, ProductProvider>();
                    services.AddSingleton<IPersonProvider, PersonProvider>();
                    services.AddSingleton<IFavoriteProductProvider, FavoriteProductProvider>();
                    services.AddSingleton<IStorageProvider, StorageProvider>();
                    services.AddSingleton<IMailProvider, MailProvider>();

                    //Services
                    services.AddSingleton<IProductService, ProductService>();
                    services.AddSingleton<IPersonService, PersonService>();
                    services.AddSingleton<IFavoriteProductService, FavoriteProductService>();
                    services.AddSingleton<IStorageService, StorageService>();
                    services.AddSingleton<IMailService, MailService>();
                    services.AddSingleton<IFileService, FileService>();

                })
                .Build();
            app.Start();

            IPersonService personService = app.Services.CreateScope().ServiceProvider.GetService<IPersonService>();
            IProductService productService = app.Services.CreateScope().ServiceProvider.GetService<IProductService>();
            IFavoriteProductService favoriteProductService = app.Services.CreateScope().ServiceProvider.GetService<IFavoriteProductService>();
            IStorageService storageService = app.Services.CreateScope().ServiceProvider.GetService<IStorageService>();
            IMailService mailService = app.Services.CreateScope().ServiceProvider.GetService<IMailService>();
            IFileService fileService = app.Services.CreateScope().ServiceProvider.GetService<IFileService>();

            Preparation(storageService, productService, mailService, personService);
            FileMenu.FileReading();

            

            //Dummy Data
            Product prod1 = productService.Add(new Product()
            {
                Name = "Elso Termek",
                CriticalLevel = 1,
                BestBefore = DateTime.Now,
                StoreInFridge = true
            });
            Product prod2 = productService.Add(new Product()
            {
                Name = "Masodik Termek",
                CriticalLevel = 1.7,
                BestBefore = DateTime.Now,
                StoreInFridge = true
            });


            Person pers1 = personService.Add(new Person()
            {
                Name = "Elso Ember",
                ResponsibleForPurchase = true,
            });

            favoriteProductService.Add(pers1, prod1);
            //Dummy data end

            PersonMenu.personService = personService;
            ProductMenu.productService = productService;
            ProductMenu.favoriteProductService = favoriteProductService;
            StorageMenu.productService = productService;
            StorageMenu.storageService = storageService;
            MailMenu.mailService = mailService;
            FileMenu.fileService = fileService;







            Menu personMenu = PersonMenu.PersonChoose();
            personMenu.Show();







        }

        private static void Preparation(IStorageService? storageService, IProductService? productService, IMailService? mailService, IPersonService? personService)
        {
            storageService.AddBaseStorages();

            storageService.CriticalLevelEvent += (Storage storage, double percent) =>
            {
                ICollection<Person> persons = personService.GetAll();
                foreach (Person person in persons)
                {
                    if (person.ResponsibleForPurchase)
                    {
                        mailService.Add(new Mail()
                        {
                            PersonId = person.Id,
                            Date = DateTime.Now,
                            Content = $"A {storage.Name} kapacitása kritikus szint alá csökkent, már csak {percent : 0.00}% hely maradt"
                        });
                    }
                }
            };

            productService.CriticalLeveleEvent += (Product product) =>
            {
                ICollection<Person> persons = personService.GetByFilter(x => x.ResponsibleForPurchase);
                foreach (Person person in persons)
                {
                    mailService.Add(new Mail()
                    {
                        PersonId = person.Id,
                        Date = DateTime.Now,
                        Content = $"Az alábbi termék mennyisége a {(product.StoreInFridge ? "hűtőben" : "kamrában")} kritikus szint alá csökkent:\n" +
                                $"{product.ToString()}"
                    });
                }
            };

            productService.ProcureEvent += (Product product) =>
            {
                ICollection<Person> persons = personService.GetByFilter(x => x.FavoriteProducts.Count(x => x.Id == product.Id) > 0);
                foreach (Person person in persons)
                {
                    mailService.Add(new Mail()
                    {
                        PersonId = person.Id,
                        Date = DateTime.Now,
                        Content = $"A(z) {product.Name} termékből újabb beszerzés történt"
                    });
                }

                
            };
        }
    }
}