using PLVT08_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders
{
    public interface IFavoriteProductProvider
    {
        public FavoriteProduct Add(FavoriteProduct FavoriteProduct);            // C
        public ICollection<FavoriteProduct> GetAll();                           // R
        public void DeleteById(int id);                                         // D
    }

    public class FavoriteProductProvider : IFavoriteProductProvider
    {
        //FIELDS
        private readonly HouseholdContext householdContext;

        //CONSTRUCTORS
        public FavoriteProductProvider(HouseholdContext householdContext)
        {
            this.householdContext = householdContext;
        }

        //METHODS
        public FavoriteProduct Add(FavoriteProduct favoriteProduct)
        {
            FavoriteProduct newFavoriteProduct = householdContext.FavoriteProducts.Add(favoriteProduct).Entity;
            householdContext.SaveChanges();
            return newFavoriteProduct;
        }
        public ICollection<FavoriteProduct> GetAll()
        {
            return householdContext.FavoriteProducts.ToHashSet();
        }

        public void DeleteById(int id)
        {
            FavoriteProduct deleting = householdContext.FavoriteProducts.First(x => x.Id == id);
            householdContext.Remove(deleting);
            householdContext.SaveChanges();
        }
    }
}