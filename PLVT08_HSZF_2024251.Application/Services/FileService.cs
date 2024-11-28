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
                BestBefore = x.BestBefore,
                CriticalLevel = x.CriticalLevel,
                Name = x.Name,
                Quantity = x.Quantity,
                StoreInFridge = x.StoreInFridge
            }))
            {
                productProvider.Add(product);
            }

            //People
            if (root["Persons"] != null)
            {
                JArray personsJson = JArray.Parse(root["Persons"]!.ToString());
                foreach (var personJson in personsJson)
                {
                    Person person = JsonConvert.DeserializeObject<Person>(personJson.ToString(), new JsonSerializerSettings()
                    {
                        MissingMemberHandling = MissingMemberHandling.Error
                    });

                    var Ids = JArray.Parse(personJson["FavoriteProductIds"].ToString());

                    string id= personProvider.Add(person).Id;
                    foreach (var productId in Ids.Select(x => (string)x))
                    {
                        if (productProvider.GetAll().Any(x => x.Id == productId))
                        {
                            FavoriteProduct favoriteProduct = new FavoriteProduct()
                            {
                                PersonId = id,
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
