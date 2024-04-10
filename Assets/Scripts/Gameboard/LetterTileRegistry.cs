using System;
using System.Collections.Generic;

namespace Gameboard
{
    public class LetterTileRegistry : IDisposable
    {
        private readonly List<LetterTile> _tilesOnBoard = new();
        private readonly List<LetterTile> _selectedTiles = new();

        #region TILES ON BOARD

        public void RegisterTileOnBoard(LetterTile tile)
        {
            _tilesOnBoard.Add(tile);
        }
        
        public void UnregisterTileOnBoard(LetterTile tile)
        {
            _tilesOnBoard.Remove(tile);
        }
        
        public List<LetterTile> GetAllTilesOnBoard()
        {
            return _tilesOnBoard;
        }

        #endregion
        

        #region SELECTED TILES

        public void RegisterSelectedTile(LetterTile tile)
        {
            _selectedTiles.Add(tile);
        }
        
        public void UnregisterSelectedTile(LetterTile tile)
        {
            _selectedTiles.Remove(tile);
        }

        public List<LetterTile> GetSelectedTiles()
        {
            return _selectedTiles;
        }
        public void ClearSelectedTiles()
        {
            _selectedTiles.Clear();
        }

        #endregion
        
        
        public void Dispose()
        {
        }

        
    }
}