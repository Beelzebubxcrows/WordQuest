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
}