using Moq;
using PLVT08_HSZF_2024251.Application.Services;
using PLVT08_HSZF_2024251.Model;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLVT08_HSZF_2024251.Test.FileTests
{
    [TestFixture]
    internal class FileImport
    {
        private Mock<IPersonProvider> personProvider;
        private Mock<IStorageProvider> storageProvider;
        private Mock<IProductProvider> productProvider;
        private Mock<IFavoriteProductProvider> favoriteProductProvider;

        private IFileService fileService;

        [SetUp]
        public void Init()
        {
            personProvider = new Mock<IPersonProvider>(MockBehavior.Strict);
            storageProvider = new Mock<IStorageProvider>(MockBehavior.Strict);
            productProvider = new Mock<IProductProvider>(MockBehavior.Strict);
            favoriteProductProvider = new Mock<IFavoriteProductProvider>(MockBehavior.Strict);

            fileService = new FileService(storageProvider.Object, productProvider.Object, personProvider.Object, favoriteProductProvider.Object);

            storageProvider.Setup(x => x.Update(It.IsAny<bool>(), It.IsAny<Storage>())).Verifiable();

            storageProvider.Setup(x => x.GetAll()).Returns(TestData.empty_storages).Verifiable();

            productProvider.Setup(x => x.GetAll()).Returns([]).Verifiable();
            productProvider.Setup(x => x.Update(It.IsAny<string>(), It.IsAny<Product>())).Verifiable();
            productProvider.Setup(x => x.Add(It.IsAny<Product>())).Returns((Product x) => x).Verifiable();

            personProvider.Setup(x => x.Add(It.IsAny<Person>())).Returns((Person x) => x).Verifiable();

            favoriteProductProvider.Setup(x => x.GetAll()).Returns([]).Verifiable();
            favoriteProductProvider.Setup(x => x.Add(It.IsAny<FavoriteProduct>())).Returns((FavoriteProduct x) => x).Verifiable();
        }

        [Test]
        public void Successful()
        {
            fileService.Import("./TestFiles/good.json");

            storageProvider.Verify(x => x.Update(It.IsAny<bool>(), It.IsAny<Storage>()), Times.Exactly(2));
            storageProvider.Verify(x => x.GetAll(), Times.Exactly(6));

            productProvider.Verify(x => x.GetAll(), Times.Exactly(6));
            productProvider.Verify(x => x.Update(It.IsAny<string>(), It.IsAny<Product>()), Times.Never());
            productProvider.Verify(x => x.Add(It.IsAny<Product>()), Times.Exactly(6));

            personProvider.Verify(x => x.Add(It.IsAny<Person>()), Times.Exactly(2));

            favoriteProductProvider.Verify(x => x.GetAll(), Times.Exactly(4));
        }

        [Test]
        public void MissingProducts()
        {
            Assert.Throws<FormatException>(() => fileService.Import("./TestFiles/missing.json"));
        }

        [Test]
        public void TooMuch()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => fileService.Import("./TestFiles/toomuch.json"));
        }




    }
}
