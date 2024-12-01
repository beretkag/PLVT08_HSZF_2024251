using PLVT08_HSZF_2024251.Model;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;


namespace PLVT08_HSZF_2024251.Application.Services
{
    public interface IStorageService
    {
        public ICollection<Storage> GetAll();
        public ICollection<Storage> GetByFilter(Func<Storage, bool> filter);
        public bool ChangeQuantity(Storage storage, int quantity);
        public void AddBaseStorages();
        public double ActualStorageCapacity(Storage storage, double criticalLevelPercent);

        public event Action<Storage, double> CriticalLevelEvent;
    }



    public class StorageService : IStorageService
    {
        private readonly IStorageProvider StorageProvider;
        public event Action<Storage, double> CriticalLevelEvent;

        public StorageService(IStorageProvider storageProvider)
        {
            this.StorageProvider = storageProvider;
        }

        public void AddBaseStorages()
        {
            if (StorageProvider.GetAll().Count == 0)
            {
                Storage Pantry = new Storage()
                {
                    Id = false,
                    Name = "Kamra",
                    Capacity = 100,
                };
                Storage Fridge = new Storage()
                {
                    Id = true,
                    Name = "Hűtő",
                    Capacity = 50,
                };
                StorageProvider.Add(Pantry);
                StorageProvider.Add(Fridge);
            }
        }

        public ICollection<Storage> GetAll()
        {
            return StorageProvider.GetAll();
        }

        public ICollection<Storage> GetByFilter(Func<Storage, bool> filter)
        {
            return StorageProvider.GetAll().Where(filter).ToHashSet();
        }
        public bool ChangeQuantity(Storage storage, int quantity)
        {
            if (quantity < 0 || storage.UsedCapacity >= quantity)
            {
                return false;
            }
            storage.Capacity = quantity;
            StorageProvider.Update(storage.Id, storage);
            return true;
        }


        public double ActualStorageCapacity(Storage storage, double criticalLevelPercent)
        {
            double empty = storage.Capacity - storage.UsedCapacity;
            if (empty <= criticalLevelPercent)
            {
                CriticalLevelEvent?.Invoke(storage, empty);
            }
            return empty/((double)(storage.Capacity)/100);
        }
    }
}
