using Moq;
using PLVT08_HSZF_2024251.Application.Services;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;


namespace PLVT08_HSZF_2024251.Test.ProductTests
{
    [TestFixture]
    internal class ProductGetAll
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
        public void TestGetAll()
        {
            var products = productService.GetAll();
            productProvider.Verify(x => x.GetAll(), Times.Once());
            Assert.That(() => products.Count, Is.EqualTo(3));
        }
    }
}
