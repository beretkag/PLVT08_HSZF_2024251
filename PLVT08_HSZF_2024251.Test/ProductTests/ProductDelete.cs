using Moq;
using PLVT08_HSZF_2024251.Application.Services;
using PLVT08_HSZF_2024251.Model;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;


namespace PLVT08_HSZF_2024251.Test.ProductTests
{
    [TestFixture]
    internal class ProductDelete
    {
        private Mock<IStorageProvider> storageProvider;
        private Mock<IProductProvider> productProvider;
        private IProductService productService;

        [SetUp]
        public void Init()
        {
            storageProvider = new Mock<IStorageProvider>(MockBehavior.Strict);
            productProvider = new Mock<IProductProvider>(MockBehavior.Strict);
            productService = new ProductService(productProvider.Object, storageProvider.Object);

            productProvider.Setup(x => x.DeleteById(It.IsAny<string>())).Verifiable();
        }

        [Test]
        public void DeleteTest()
        {
            Product product = new()
            {
                Id = "3",
                Name = "Third_Product",
                Quantity = 30,
                StoreInFridge = true,
                CriticalLevel = 5,
                BestBefore = DateTime.Now
            };

            productService.Delete(product);
            productProvider.Verify(x => x.DeleteById(product.Id), Times.Once());
        }
    }
}
