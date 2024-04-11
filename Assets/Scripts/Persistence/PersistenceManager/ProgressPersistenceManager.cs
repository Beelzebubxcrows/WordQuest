using System.Collections.Generic;
using Persistence.Data;

namespace Persistence.PersistenceManager
{
    public class ProgressPersistenceManager : IPersistenceManager
    {
        private ProgressData _progressData;
        private PersistenceManager _persistenceManager;

        public ProgressPersistenceManager(PersistenceManager persistenceManager)
        {
            _persistenceManager = persistenceManager;
        }

        public void Save()
        { 
            _persistenceManager.SaveContent();
        }

        public void Load(IPersistenceData persistenceData)
        {
            _progressData = (ProgressData)persistenceData;
        }

        public string GetPersistenceKey()
        {
            return "Progress";
        }

        #region INVENTORY

        public Dictionary<InventoryType, int> GetInventoryCount()
        {
            return _progressData.InventoryCount;
        }

        public void IncrementInventoryCount(InventoryType inventoryType, int amount)
        {
            _progressData.InventoryCount[inventoryType]+=amount;
            Save();
        }

        
        public void DecrementInventoryCount(InventoryType inventoryType, int amount)
        {
            _progressData.InventoryCount[inventoryType] -= amount;
            Save();
        }

        #endregion
        
        public void Dispose()
        {
        }
    }
}