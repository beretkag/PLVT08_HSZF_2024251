using PLVT08_HSZF_2024251.Model;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;

namespace PLVT08_HSZF_2024251.Application.Services
{
    public interface IFavoriteProductService
    {
        public FavoriteProduct Add(Person person, Product product);
        public ICollection<FavoriteProduct> GetAll();
        public ICollection<FavoriteProduct> GetByFilter(Func<FavoriteProduct, bool> filter);
        public void Delete(Product product, Person person);

    }


    public class FavoriteProductService : IFavoriteProductService
    {
        private readonly IFavoriteProductProvider FavoriteProductProvider;

        public FavoriteProductService(IFavoriteProductProvider FavoriteProductProvider)
        {
            this.FavoriteProductProvider = FavoriteProductProvider;
        }

        public FavoriteProduct Add(Person person, Product product)
        {
            FavoriteProduct favoriteProduct = new FavoriteProduct(){
                ProductId = product.Id,
                PersonId = person.Id,
            };
            return FavoriteProductProvider.Add(favoriteProduct);
        }
        public ICollection<FavoriteProduct> GetAll()
        {
            return FavoriteProductProvider.GetAll();
        }

        public ICollection<FavoriteProduct> GetByFilter(Func<FavoriteProduct, bool> filter)
        {
            return FavoriteProductProvider.GetAll().Where(filter).ToHashSet();
        }
        public void Delete(Product product, Person person)
        {
            int favoriteProductId=  FavoriteProductProvider.GetAll().Where(x => x.ProductId == product.Id && x.PersonId == person.Id).First().Id;
            FavoriteProductProvider.DeleteById(favoriteProductId);
        }
    }
}
