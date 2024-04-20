using System;
using System.Collections.Generic;

namespace Gameboard
{
    public class LetterTileRegistry : IDisposable
    {
        private readonly List<string> _wordsMatched = new();
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


        public int[] GetCharacterFrequency()
        {
            var frequency = new int[26];
            foreach (var tile in _tilesOnBoard)
            {
                frequency[tile.GetCharacter()-'A']++;
            }

            return frequency;
        }

        public Dictionary<char, List<LetterTile>> GetTilesByCharacter()
        {
            var allTiles = new Dictionary<char, List<LetterTile>>();
            foreach (var tile in _tilesOnBoard)
            {
                if (!allTiles.ContainsKey(tile.GetCharacter())) {
                    allTiles.Add(tile.GetCharacter(),new List<LetterTile>(){tile});
                }
                else {
                    allTiles[tile.GetCharacter()].Add(tile);
                }
            }

            return allTiles;
        }

        #region MATCHED WORDS

        public List<string> GetMatchedWordsInLevel()
        {
            return _wordsMatched;
        }

        public void AddMatchedWord(string word)
        {
            _wordsMatched.Add(word);
        }    

        #endregion    
        
        public void Dispose()
        {
            _wordsMatched.Clear();
            _tilesOnBoard.Clear();
            _selectedTiles.Clear();
        }

        
    }
}