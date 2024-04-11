using System.Collections.Generic;

namespace Persistence.Data
{
    public class ProgressData : IPersistenceData
    {
        public readonly Dictionary<InventoryType, int> InventoryCount;

        public ProgressData()
        {
            InventoryCount = new Dictionary<InventoryType, int> { { InventoryType.MasteryPoint, 0 } };
        }
    }
}