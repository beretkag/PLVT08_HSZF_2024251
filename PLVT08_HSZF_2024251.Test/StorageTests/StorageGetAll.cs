using Moq;
using PLVT08_HSZF_2024251.Application.Services;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;


namespace PLVT08_HSZF_2024251.Test.StorageTests
{
    [TestFixture]
    internal class StorageGetAll
    {
        private Mock<IStorageProvider> storageProvider;
        private IStorageService storageService;

        [SetUp]
        public void Init()
        {
            storageProvider = new Mock<IStorageProvider>(MockBehavior.Strict);
            storageService = new StorageService(storageProvider.Object);

            storageProvider.Setup(x => x.GetAll()).Returns(TestData.storages).Verifiable();
        }

        [Test]
        public void TestGetAll()
        {
            var storages = storageService.GetAll();
            storageProvider.Verify(x => x.GetAll(), Times.Once());
            Assert.That(() => storages.Count, Is.EqualTo(2));
        }
    }
}
