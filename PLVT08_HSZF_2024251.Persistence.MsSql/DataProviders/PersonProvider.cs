using Microsoft.EntityFrameworkCore;
using PLVT08_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders
{
    public interface IPersonProvider
    {
        public Person Add(Person person);           // C
        public ICollection<Person> GetAll();        // R
        public void Update(string id, Person person);  // U
        public void DeleteById(string id);             // D
    }

    public class PersonProvider : IPersonProvider
    {
        //FIELDS
        private readonly HouseholdContext householdContext;

        //CONSTRUCTORS
        public PersonProvider(HouseholdContext householdContext)
        {
            this.householdContext = householdContext;
        }

        //METHODS
        public Person Add(Person person)
        {
            Person newPerson = householdContext.Persons.Add(person).Entity;
            householdContext.SaveChanges();
            return newPerson;
        }
        public ICollection<Person> GetAll()
        {
            return householdContext.Persons.Include(x => x.Mails).Include(x => x.FavoriteProducts).ToHashSet();
        }

        public void Update(string id, Person person)
        {
            Person updating = householdContext.Persons.First(x=>x.Id == id);
            updating = person;
            householdContext.SaveChanges();
        }

        public void DeleteById(string id)
        {
            Person deleting = householdContext.Persons.First(x => x.Id == id);
            householdContext.Remove(deleting);
            householdContext.SaveChanges();
        }

    }
}
