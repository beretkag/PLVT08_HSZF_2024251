using PLVT08_HSZF_2024251.Model;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;

namespace PLVT08_HSZF_2024251.Application.Services
{
    public interface IPersonService
    {
        public Person Add(Person person);
        public ICollection<Person> GetAll();
        public ICollection<Person> GetByFilter(Func<Person, bool> filter);
        public void Delete(Person person);

    }


    public class PersonService : IPersonService
    {
        private readonly IPersonProvider personProvider;

        public PersonService(IPersonProvider personProvider)
        {
            this.personProvider = personProvider;
        }

        public Person Add(Person person)
        {
            return personProvider.Add(person);
        }
        public ICollection<Person> GetAll()
        {
            return personProvider.GetAll();
        }

        public ICollection<Person> GetByFilter(Func<Person, bool> filter)
        {
            return personProvider.GetAll().Where(filter).ToHashSet();
        }
        public void Delete(Person person)
        {
            personProvider.DeleteById(person.Id);
        }
    }
}
