using Moq;
using PLVT08_HSZF_2024251.Application.Services;
using PLVT08_HSZF_2024251.Model;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;


namespace PLVT08_HSZF_2024251.Test.PersonTests
{
    [TestFixture]
    internal class PersonAdd
    {
        private Mock<IPersonProvider> personProvider;
        private IPersonService personService;

        [SetUp]
        public void Init()
        {
            personProvider = new Mock<IPersonProvider>(MockBehavior.Strict);
            personService = new PersonService(personProvider.Object);

            personProvider.Setup(x => x.Add(It.IsAny<Person>())).Returns((Person x) => x).Verifiable();
        }

        [Test]
        public void AddTest()
        {
            Person person = TestData.persons.First();

            personService.Add(person);
            personProvider.Verify(x => x.Add(person), Times.Once());
        }
    }
}
