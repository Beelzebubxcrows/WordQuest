using System;
using DefaultNamespace;
using Utility;

namespace Economy
{
    public class EconomyManager : IDisposable
    {

        private readonly InventorySystem _inventorySystem;

        public EconomyManager()
        {
            _inventorySystem = InstanceManager.GetInstanceAsSingle<InventorySystem>();
        }

        public bool TryBuy(InventoryType inventoryType)
        {
            var cost = GetCost(inventoryType);
            var masteryPoint = _inventorySystem.GetInventoryCount(InventoryType.MasteryPoint);
            if (masteryPoint >= cost) {
                _inventorySystem.DeductInventory(InventoryType.MasteryPoint, cost);
                _inventorySystem.GrantInventory(inventoryType, 1);
                return true;
            }

            return false;
        }

        private int GetCost(InventoryType inventoryType)
        {
            switch (inventoryType)
            {
                case InventoryType.MasteryPoint:
                    return 0;
                case InventoryType.UndoPowerUp:
                    return 50;
                case InventoryType.HintPowerUp:
                    return 100;
                case InventoryType.ShufflePowerUp:
                    return 150;
            }

            return 0;
        }
        
        public void Dispose()
        {
        }
    }
}