using System.Collections.Generic;

namespace Persistence.Data
{
    public class ProgressData : IPersistenceData
    {
        public readonly Dictionary<InventoryType, int> InventoryCount;
        public int LatestLevel;
        public int CurrentLevel;

        public ProgressData()
        {
            LatestLevel = 1;
            CurrentLevel = 1;
            InventoryCount = new Dictionary<InventoryType, int>
            {
                { InventoryType.MasteryPoint, 100 },
                { InventoryType.HintPowerUp , 5},
                { InventoryType.ShufflePowerUp ,5},
                { InventoryType.UndoPowerUp, 5}
            };
        }
    }
}