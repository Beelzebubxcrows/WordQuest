using System;
using UnityEngine;

public enum InventoryType
{
    MasteryPoint
}

namespace DefaultNamespace
{
    public class InventorySystem : IDisposable
    {
        private const string InventoryPlayerPrefString = "Inventory_{0}";

        public int GetInventoryCount(InventoryType inventoryType)
        {
            return PlayerPrefs.GetInt(string.Format(InventoryPlayerPrefString, inventoryType), 0);
        }
        
        public void IncrementInventoryCount(InventoryType inventoryType, int inventoryDiff)
        {
            var inventoryCount = GetInventoryCount(inventoryType);
            PlayerPrefs.SetInt(string.Format(InventoryPlayerPrefString, inventoryType), inventoryCount+inventoryDiff);
        }
        
        public void DecrementInventoryCount(InventoryType inventoryType, int inventoryDiff)
        {
            var inventoryCount = GetInventoryCount(inventoryType);
            PlayerPrefs.SetInt(string.Format(InventoryPlayerPrefString, inventoryType), inventoryCount-inventoryDiff);
        }
        
        public void Dispose()
        {
        }
    }
}