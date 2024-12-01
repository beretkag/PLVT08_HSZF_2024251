using Moq;
using PLVT08_HSZF_2024251.Application.Services;
using PLVT08_HSZF_2024251.Model;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLVT08_HSZF_2024251.Test.FavoriteProductTests
{
    [TestFixture]
    internal class FavoriteProductDelete
    {
        private Mock<IFavoriteProductProvider> favoriteProductProvider;
        private IFavoriteProductService favoriteProductService;

        [SetUp]
        public void Init()
        {
            favoriteProductProvider = new Mock<IFavoriteProductProvider>(MockBehavior.Strict);
            favoriteProductService = new FavoriteProductService(favoriteProductProvider.Object);

            favoriteProductProvider.Setup(x => x.GetAll()).Returns(TestData.favoriteProducts).Verifiable();
            favoriteProductProvider.Setup(x => x.DeleteById(It.IsAny<int>())).Verifiable();
        }

        [Test]
        public void DeleteTest()
        {
            Person person = TestData.persons.First();
            Product product = TestData.products.First();

            favoriteProductService.Delete(product, person);
            favoriteProductProvider.Verify(x => x.DeleteById(It.IsAny<int>()), Times.Once());
        }
    }
}
