using System.Collections.Generic;
using Persistence.Data;

namespace Persistence.PersistenceManager
{
    public class ProgressPersistenceManager : IPersistenceManager
    {
        private ProgressData _progressData;
        private readonly PersistenceManager _persistenceManager;

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
            var currAmount = _progressData.InventoryCount[inventoryType];
            if (currAmount - amount < 0) {
                return;
            }
            
            _progressData.InventoryCount[inventoryType] -= amount;
            Save();
        }

        #endregion

        #region LEVELS

        public int GetCurrentLevel()
        {
            return _progressData.CurrentLevel;
        }

        public int GetLatestLevel()
        {
            return _progressData.LatestLevel;
        }
        
        public void SetCurrentLevel(int level)
        {
            _progressData.CurrentLevel = level;
            Save();
        }
        
        public void IncrementLatestLevel()
        {
            if (GetCurrentLevel() == GetLatestLevel()) {
                _progressData.LatestLevel++;
            }
            Save();
        }


        #endregion
        
        public void Dispose()
        {
        }
    }
}