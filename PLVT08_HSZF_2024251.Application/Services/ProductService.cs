using PLVT08_HSZF_2024251.Model;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;

namespace PLVT08_HSZF_2024251.Application.Services
{
    public interface IProductService
    {
        public Product Add(Product Product);
        public ICollection<Product> GetAll();
        public ICollection<Product> GetByFilter(Func<Product, bool> filter);
        public bool AddQuantity(Product Product, double quantity);
        public void Delete(Product Product);

        public event Action<Product> ProcureEvent;
        public event Action<Product> CriticalLeveleEvent;
    }


    public class ProductService : IProductService
    {
        private readonly IProductProvider productProvider;
        private readonly IStorageProvider storageProvider;
        private readonly IMailProvider mailProvider;
        private readonly IPersonProvider personProvider;

        public event Action<Product> ProcureEvent;
        public event Action<Product> CriticalLeveleEvent;

        public ProductService(IProductProvider productProvider, IStorageProvider storageProvider, IMailProvider mailProvider, IPersonProvider personProvider)
        {
            this.productProvider = productProvider;
            this.storageProvider = storageProvider;
            this.mailProvider = mailProvider;
            this.personProvider = personProvider;
        }

        public Product Add(Product product)
        {
            return productProvider.Add(product);
        }
        public ICollection<Product> GetAll()
        {
            return productProvider.GetAll();
        }

        public ICollection<Product> GetByFilter(Func<Product, bool> filter)
        {
            return productProvider.GetAll().Where(filter).ToHashSet();
        }
        public bool AddQuantity(Product product, double quantity)
        {
            bool storeInFridge = product.StoreInFridge;
            Storage storage = storageProvider.GetAll().Where(x => x.Id == storeInFridge).First();
            if (Convert.ToDouble(storage.Capacity) < storage.UsedCapacity + quantity || 0 > storage.UsedCapacity + quantity)
            {
                return false;
            }
            product.Quantity += quantity;
            productProvider.Update(product.Id, product);

            if (quantity > 0) ProcureEvent?.Invoke(product);
            else if (product.Quantity < product.CriticalLevel) CriticalLeveleEvent?.Invoke(product);

            return true;
        }
        public void Delete(Product product)
        {
            productProvider.DeleteById(product.Id);
        }
    }
}
