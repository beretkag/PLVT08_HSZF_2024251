using Moq;
using PLVT08_HSZF_2024251.Application.Services;
using PLVT08_HSZF_2024251.Model;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;


namespace PLVT08_HSZF_2024251.Test.FavoriteProductTests
{
    [TestFixture]
    internal class FavoriteProductAdd
    {
        private Mock<IFavoriteProductProvider> favoriteProductProvider;
        private IFavoriteProductService favoriteProductService;

        [SetUp]
        public void Init()
        {
            favoriteProductProvider = new Mock<IFavoriteProductProvider>(MockBehavior.Strict);
            favoriteProductService = new FavoriteProductService(favoriteProductProvider.Object);

            favoriteProductProvider.Setup(x => x.Add(It.IsAny<FavoriteProduct>())).Returns((FavoriteProduct x) => x).Verifiable();
        }

        [Test]
        public void AddTest()
        {
            Person person = TestData.persons.First();
            Product product = TestData.products.First();

            favoriteProductService.Add(person, product);
            favoriteProductProvider.Verify(x => x.Add(It.IsAny<FavoriteProduct>()), Times.Once());
        }
    }
}
