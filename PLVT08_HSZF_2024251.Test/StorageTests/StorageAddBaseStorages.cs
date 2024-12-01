using Moq;
using PLVT08_HSZF_2024251.Application.Services;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;
using PLVT08_HSZF_2024251.Model;


namespace PLVT08_HSZF_2024251.Test.StorageTests
{
    [TestFixture]
    internal class StorageAddBaseStorages
    {
        private Mock<IStorageProvider> storageProvider;
        private IStorageService storageService;

        [SetUp]
        public void Init()
        {
            storageProvider = new Mock<IStorageProvider>(MockBehavior.Strict);
            storageService = new StorageService(storageProvider.Object);

            storageProvider.Setup(x => x.GetAll()).Returns([]).Verifiable();
            storageProvider.Setup(x => x.Add(It.IsAny<Storage>())).Returns((Storage x) => x).Verifiable();
        }

        [Test]
        public void AddBaseStoragesTest()
        {
            storageService.AddBaseStorages();
            storageProvider.Verify(x => x.Add(It.IsAny<Storage>()), Times.Exactly(2));
        }
    }
}
