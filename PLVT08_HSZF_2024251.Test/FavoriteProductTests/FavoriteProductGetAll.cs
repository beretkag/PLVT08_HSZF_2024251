using Moq;
using PLVT08_HSZF_2024251.Application.Services;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLVT08_HSZF_2024251.Test.FavoriteProductTests
{
    [TestFixture]
    internal class FavoriteProductGetAll
    {
        private Mock<IFavoriteProductProvider> favoriteProductProvider;
        private IFavoriteProductService favoriteProductService;

        [SetUp]
        public void Init()
        {
            favoriteProductProvider = new Mock<IFavoriteProductProvider>(MockBehavior.Strict);
            favoriteProductService = new FavoriteProductService(favoriteProductProvider.Object);

            favoriteProductProvider.Setup(x => x.GetAll()).Returns(TestData.favoriteProducts).Verifiable();
        }
        
        [Test]
        public void TestGetAll()
        {
            var favoriteProducts = favoriteProductService.GetAll();
            favoriteProductProvider.Verify(x => x.GetAll(), Times.Once());
            Assert.That(() => favoriteProducts.Count, Is.EqualTo(2));
        }
    }
}
