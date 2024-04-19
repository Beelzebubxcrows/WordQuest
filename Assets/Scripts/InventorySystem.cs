using System;
using Persistence.PersistenceManager;
using Utility;

public enum InventoryType
{
    MasteryPoint,
    UndoPowerUp,
    ShufflePowerUp,
    HintPowerUp
}

namespace DefaultNamespace
{
    public class InventorySystem : IDisposable
    {
        private readonly ProgressPersistenceManager _progressPersistenceManager;
        
        public InventorySystem()
        {
            _progressPersistenceManager = InstanceManager.GetInstanceAsSingle<ProgressPersistenceManager>();
        }

        public int GetInventoryCount(InventoryType inventoryType)
        {
            return _progressPersistenceManager.GetInventoryCount()[inventoryType];
        }
        
        public void GrantInventory(InventoryType inventoryType, int inventoryDiff)
        {
            if (inventoryDiff <= 0) {
                return;
            }
            _progressPersistenceManager.IncrementInventoryCount(inventoryType, inventoryDiff);
        }
        
        public void DeductInventory(InventoryType inventoryType, int inventoryDiff)
        {
            if (inventoryDiff <= 0) {
                return;
            }
            _progressPersistenceManager.DecrementInventoryCount(inventoryType, inventoryDiff);
        }
        
        public void Dispose()
        {
        }
    }
}