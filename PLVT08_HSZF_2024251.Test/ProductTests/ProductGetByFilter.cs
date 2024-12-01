using Moq;
using PLVT08_HSZF_2024251.Application.Services;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLVT08_HSZF_2024251.Test.ProductTests
{
    [TestFixture]
    internal class ProductGetByFilter
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

            productProvider.Setup(x => x.GetAll()).Returns(TestData.products).Verifiable();
        }

        [Test]
        public void TestGetByFilter()
        {
            var products = productService.GetByFilter(x => x.StoreInFridge == true);
            productProvider.Verify(x => x.GetAll(), Times.Once());
            Assert.That(() => products.Count, Is.EqualTo(2));
            Assert.True(products.First().StoreInFridge);
        }
    }
}
