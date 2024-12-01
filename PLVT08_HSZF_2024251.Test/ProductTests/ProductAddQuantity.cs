

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using PLVT08_HSZF_2024251.Application.Services;
using PLVT08_HSZF_2024251.Model;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;

namespace PLVT08_HSZF_2024251.Test.ProductTests
{
    [TestFixture]
    internal class ProductAddQuantity
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

            productProvider.Setup(x => x.Update(It.IsAny<string>(), It.IsAny<Product>())).Verifiable();
            storageProvider.Setup(x => x.GetAll()).Returns(TestData.storages).Verifiable();
        }

        [TestCase(5)]
        [TestCase(-20)]
        [TestCase(-40)]
        public void AddQuantityTest(double quantity)
        {
            Product product = new()
            {
                Id = "0",
                Name = "First_Product",
                Quantity = 30,
                StoreInFridge = false,
                CriticalLevel = 5,
                BestBefore = DateTime.Now,
            };
            bool result = quantity <= 10 && quantity >= -30;
            Assert.That(() => productService.AddQuantity(product, quantity), Is.EqualTo(result));
            productProvider.Verify(x => x.Update(It.IsAny<string>(), It.IsAny<Product>()), result ? Times.Once() : Times.Never());

        }

    }
}
