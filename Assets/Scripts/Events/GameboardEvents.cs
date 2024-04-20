using Gameboard;

namespace Events
{
    public struct TileClicked
    {
        public LetterTile Tile;

        public TileClicked(LetterTile tile)
        {
            Tile = tile;
        }
    }
    public struct TickClicked
    {
    }

    public struct InventoryGranted
    {
        public readonly InventoryType InventoryType;
        public readonly int Amount;
        public InventoryGranted(InventoryType inventoryInventoryType, int amount)
        {
            InventoryType = inventoryInventoryType;
            Amount = amount;
        }
    }
    
    public struct InventoryDeducted
    {
        public readonly InventoryType InventoryType;
        public readonly int Amount;
        public InventoryDeducted(InventoryType inventoryInventoryType, int amount)
        {
            InventoryType = inventoryInventoryType;
            Amount = amount;
        }
    }
}