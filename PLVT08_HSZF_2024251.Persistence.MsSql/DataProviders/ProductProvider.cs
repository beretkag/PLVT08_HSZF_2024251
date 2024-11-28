
using Microsoft.EntityFrameworkCore;
using PLVT08_HSZF_2024251.Model;

namespace PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders
{
    public interface IProductProvider
    {
        public Product Add(Product Product);                // C
        public ICollection<Product> GetAll();               // R
        public void Update(string id, Product Product);     // U
        public void DeleteById(string id);                  // D
    }

    public class ProductProvider : IProductProvider
    {
        //FIELDS
        private readonly HouseholdContext householdContext;

        //CONSTRUCTORS
        public ProductProvider(HouseholdContext householdContext)
        {
            this.householdContext = householdContext;
        }

        //METHODS
        public Product Add(Product Product)
        {
            Product newProduct = householdContext.Products.Add(Product).Entity;
            householdContext.SaveChanges();
            return newProduct;
        }
        public ICollection<Product> GetAll()
        {
            return householdContext.Products.ToHashSet();
        }

        public void Update(string id, Product product)
        {
            Product updating = householdContext.Products.First(x => x.Id == id);
            updating = product;
            householdContext.SaveChanges();
        }

        public void DeleteById(string id)
        {
            Product deleting = householdContext.Products.First(x => x.Id == id);
            householdContext.Remove(deleting);
            householdContext.SaveChanges();
        }
    }
}
