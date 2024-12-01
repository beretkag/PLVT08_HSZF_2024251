

using Moq;
using PLVT08_HSZF_2024251.Application.Services;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;
using PLVT08_HSZF_2024251.Model;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace PLVT08_HSZF_2024251.Test.StorageTests
{
    [TestFixture]
    internal class StorageChangeQuantity
    {
        private Mock<IStorageProvider> storageProvider;
        private IStorageService storageService;

        [SetUp]
        public void Init()
        {
            storageProvider = new Mock<IStorageProvider>(MockBehavior.Strict);
            storageService = new StorageService(storageProvider.Object);

            storageProvider.Setup(x => x.Update(It.IsAny<bool>(), It.IsAny<Storage>())).Verifiable();
        }

        [TestCase(0, 70)]       // 0 = usedCapacity < newCapacity
        [TestCase(40, 30)]      // 0 < newCapacity < usedCapacity
        [TestCase(-40, -10)]    // usedCapacity < newCapacity < 0
        public void ChangeQuantityTest(int usedCapacity, int newCapacity)
        {
            Storage storage = new()
            {
                Id = true,
                Name = "Firdge",
                Capacity = 50,
                Products = [
                    new(){
                        Id = "0",
                        Name = "testProduct",
                        Quantity = usedCapacity
                    }
                    ]
            };
            bool result = (newCapacity > 0 && newCapacity >= usedCapacity);
            Assert.That(() => storageService.ChangeQuantity(storage, newCapacity), Is.EqualTo(result));
            storageProvider.Verify(x => x.Update(It.IsAny<bool>(), It.IsAny<Storage>()), result ? Times.Once() : Times.Never());
        }
    }
}
