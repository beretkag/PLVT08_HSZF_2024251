using PLVT08_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLVT08_HSZF_2024251.Test
{
    public class TestData
    {

        public static ICollection<Storage> storages = [
            new(){
                Id = false,
                Name = "Pantry",
                Capacity = 100,
                Products =[
                    new(){
                        Id = "3",
                        Name = "Third_Product",
                        Quantity = 30,
                        StoreInFridge = true,
                        CriticalLevel = 5,
                        BestBefore = DateTime.Now,
                    }
                ]
            },
            new(){
                Id = true,
                Name = "Fridge",
                Capacity = 50,
                Products =[
                    new(){
                        Id = "0",
                        Name = "First_Product",
                        Quantity = 30,
                        StoreInFridge = false,
                        CriticalLevel = 5,
                        BestBefore = DateTime.Now,
                    },
                    new(){
                        Id = "1",
                        Name = "Second_Product",
                        Quantity = 10,
                        StoreInFridge = true,
                        CriticalLevel = 5,
                        BestBefore = DateTime.Now,
                    }
                ]
            }
        ];
        public static ICollection<Storage> empty_storages = [
            new(){
                Id = false,
                Name = "Pantry",
                Capacity = 100,
                Products =[]
            },
            new(){
                Id = true,
                Name = "Fridge",
                Capacity = 100,
                Products =[]
            }
        ];

        public static ICollection<Product> products = [
            new(){
                Id = "0",
                Name = "First_Product",
                Quantity = 30,
                StoreInFridge = false,
                CriticalLevel = 5,
                BestBefore = DateTime.Now,
            },
            new(){
                Id = "1",
                Name = "Second_Product",
                Quantity = 10,
                StoreInFridge = true,
                CriticalLevel = 5,
                BestBefore = DateTime.Now,
            },
            new(){
                Id = "2",
                Name = "Third_Product",
                Quantity = 30,
                StoreInFridge = true,
                CriticalLevel = 5,
                BestBefore = DateTime.Now,
            }
        ];

        public static ICollection<Person> persons = [
            new(){
                Id = "0",
                Name = "Elso_Ember",
                ResponsibleForPurchase = true,
                FavoriteProducts =[products.First()],
                Mails = []
            },
            new(){
                Id = "1",
                Name = "Masodik_Ember",
                ResponsibleForPurchase = false,
                FavoriteProducts =[products.First()],
                Mails = []
            }
            ];

        public static ICollection<Mail> mails = [
            new(){
                Id = 0,
                PersonId = "0",
                Date = DateTime.Now,
                Content = "Tartalom1"
            },
            new(){
                Id = 1,
                PersonId="1",
                Date = DateTime.Now,
                Content="Tartalom2"
            }
            ];

        public static ICollection<FavoriteProduct> favoriteProducts = [
            new(){
                Id = 0,
                PersonId = "0",
                ProductId = "0",
            },
            new(){
                Id = 1,
                PersonId = "1",
                ProductId = "0",
            }
            ];
    }
}
