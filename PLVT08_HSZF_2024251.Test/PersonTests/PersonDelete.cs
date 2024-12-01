using Moq;
using PLVT08_HSZF_2024251.Application.Services;
using PLVT08_HSZF_2024251.Model;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLVT08_HSZF_2024251.Test.PersonTests
{
    [TestFixture]
    internal class PersonDelete
    {
        private Mock<IPersonProvider> personProvider;
        private IPersonService personService;

        [SetUp]
        public void Init()
        {
            personProvider = new Mock<IPersonProvider>(MockBehavior.Strict);
            personService = new PersonService(personProvider.Object);

            personProvider.Setup(x => x.DeleteById(It.IsAny<string>())).Verifiable();
        }

        [Test]
        public void DeleteTest()
        {
            Person person = TestData.persons.First();

            personService.Delete(person);
            personProvider.Verify(x => x.DeleteById(person.Id), Times.Once());
        }
    }
}
