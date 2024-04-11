using System;
using Persistence.PersistenceManager;
using UnityEngine;
using Utility;

public enum InventoryType
{
    MasteryPoint
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
        
        public void IncrementInventoryCount(InventoryType inventoryType, int inventoryDiff)
        {
            _progressPersistenceManager.IncrementInventoryCount(inventoryType, inventoryDiff);
        }
        
        public void DecrementInventoryCount(InventoryType inventoryType, int inventoryDiff)
        {
            _progressPersistenceManager.DecrementInventoryCount(inventoryType, inventoryDiff);
        }
        
        public void Dispose()
        {
        }
    }
}