using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PLVT08_HSZF_2024251.Model;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLVT08_HSZF_2024251.Application.Services
{
    public interface IFileService
    {
        public void Import(string path);
    }

    public class FileService : IFileService
    {
        private readonly IStorageProvider storageProvider;
        private readonly IProductProvider productProvider;
        private readonly IPersonProvider personProvider;
        private readonly IFavoriteProductProvider favoriteProductProvider;

        public FileService(IStorageProvider storageProvider, IProductProvider productProvider, IPersonProvider personProvider, IFavoriteProductProvider favoriteProductProvider)
        {
            this.storageProvider = storageProvider;
            this.productProvider = productProvider;
            this.personProvider = personProvider;
            this.favoriteProductProvider = favoriteProductProvider;
        }

        public void Import(string path)
        {
            JObject root = JObject.Parse(File.ReadAllText(path));


            //Storages
            if (root["Fridge"] != null)
            {

                storageProvider.Update(true, new Storage()
                {
                    Capacity = root["Fridge"]!.Value<int?>("Capacity") ?? 50
                });
            }
            if (root["Pantry"] != null)
            {

                storageProvider.Update(false, new Model.Storage()
                {
                    Capacity = root["Pantry"]!.Value<int?>("Capacity") ?? 100
                });
            }

            if (root["Products"] == null)
            {
                throw new FormatException("Products missing");
            }
            ICollection<Product> products = JsonConvert.DeserializeObject<ICollection<Product>>(root["Products"]!.ToString(), new JsonSerializerSettings() { 
                MissingMemberHandling = MissingMemberHandling.Error
            });

            //Products
            foreach (Product product in products.Select(x => new Product()
            {
                Id = x.Id,
                BestBefore = x.BestBefore,
                CriticalLevel = x.CriticalLevel,
                Name = x.Name,
                Quantity = x.Quantity,
                StoreInFridge = x.StoreInFridge
            }))
            {
                Storage storage = storageProvider.GetAll().First(x => x.Id == product.StoreInFridge);
                if (storage.Capacity < storage.UsedCapacity + product.Quantity || 0 > storage.UsedCapacity + product.Quantity)
                {
                    throw new ArgumentOutOfRangeException("Tároló túltöltése léphet fel!");
                }
                else
                {
                    if (productProvider.GetAll().Any(x => x.Id == product.Id))
                    {
                        Product updating = productProvider.GetAll().First(x => x.Id == product.Id);
                        updating.Quantity += product.Quantity;
                        productProvider.Update(updating.Id, updating);
                    }
                    else
                    {
                        productProvider.Add(product);
                    }
                }
            }

            //People
            
            if (root["Persons"] != null)
            {
                JArray personsJson = JArray.Parse(root["Persons"]!.ToString());
                foreach (var personJson in personsJson)
                {
                    Person person = JsonConvert.DeserializeObject<Person>(personJson.ToString());

                    var Ids = JArray.Parse(personJson["FavoriteProductIds"].ToString());

                    Person newPerson = new Person()
                    {
                        Name = person.Name,
                        ResponsibleForPurchase = person.ResponsibleForPurchase
                    };
                    personProvider.Add(newPerson);

                    foreach (var productId in Ids.Select(x => (string)x))
                    {
                        if (!favoriteProductProvider.GetAll().Any(x => x.PersonId == newPerson.Id && x.ProductId == productId))
                        {
                            FavoriteProduct favoriteProduct = new FavoriteProduct()
                            {
                                PersonId = newPerson.Id,
                                ProductId = productId
                            };
                            favoriteProductProvider.Add(favoriteProduct);
                        }
                    }
                }
            }
        }
    }
}
