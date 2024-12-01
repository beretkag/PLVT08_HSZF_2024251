using Moq;
using PLVT08_HSZF_2024251.Application.Services;
using PLVT08_HSZF_2024251.Model;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;


namespace PLVT08_HSZF_2024251.Test.StorageTests
{
    [TestFixture]
    internal class StorageActualStorageCapacity
    {
        private Mock<IStorageProvider> storageProvider;
        private IStorageService storageService;

        [SetUp]
        public void Init()
        {
            storageProvider = new Mock<IStorageProvider>(MockBehavior.Strict);
            storageService = new StorageService(storageProvider.Object);
        }

        [TestCase(0, 100)]
        [TestCase(30, 100)]
        [TestCase(95, 100)]
        [TestCase(100, 100)]
        public void ActualStorageCapacityTest(double usedCapacity, int Capacity)
        {
            Storage storage = new()
            {
                Id = false,
                Name = "Pantry",
                Capacity = Capacity,
                Products = [
                    new(){
                        Id = "3",
                        Name = "Third_Product",
                        Quantity = usedCapacity
                    }
                ]
            };
            Assert.That(() => storageService.ActualStorageCapacity(storage, 10), Is.EqualTo((storage.Capacity - storage.UsedCapacity) / ((double)storage.Capacity / 100)));
        }

    }
}
