using Moq;
using PLVT08_HSZF_2024251.Application.Services;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLVT08_HSZF_2024251.Test.PersonTests
{
    [TestFixture]
    internal class PersonGetAll
    {
        private Mock<IPersonProvider> personProvider;
        private IPersonService personService;

        [SetUp]
        public void Init()
        {
            personProvider = new Mock<IPersonProvider>(MockBehavior.Strict);
            personService = new PersonService(personProvider.Object);

            personProvider.Setup(x => x.GetAll()).Returns(TestData.persons).Verifiable();
        }

        [Test]
        public void TestGetAll()
        {
            var persons = personService.GetAll();
            personProvider.Verify(x => x.GetAll(), Times.Once());
            Assert.That(() => persons.Count, Is.EqualTo(2));
        }
    }
}
