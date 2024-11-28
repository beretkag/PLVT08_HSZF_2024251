using PLVT08_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders
{
    public interface IStorageProvider
    {
        public Storage Add(Storage storage);             // C
        public ICollection<Storage> GetAll();            // R
        public void Update(bool id, Storage storage);    // U
        public void DeleteById(bool id);                 // D
    }

    public class StorageProvider : IStorageProvider
    {
        //FIELDS
        private readonly HouseholdContext householdContext;

        //CONSTRUCTORS
        public StorageProvider(HouseholdContext householdContext)
        {
            this.householdContext = householdContext;
        }

        //METHODS
        public Storage Add(Storage storage)
        {
            Storage newStorage = householdContext.Storages.Add(storage).Entity;
            householdContext.SaveChanges();
            return newStorage;
        }
        public ICollection<Storage> GetAll()
        {
            return householdContext.Storages.ToHashSet();
        }

        public void Update(bool id, Storage storage)
        {
            Storage updating = householdContext.Storages.First(x => x.Id == id);
            updating.Capacity = storage.Capacity;
            householdContext.SaveChanges();
        }

        public void DeleteById(bool id)
        {
            Storage deleting = householdContext.Storages.First(x => x.Id == id);
            householdContext.Remove(deleting);
            householdContext.SaveChanges();
        }
    }
}
