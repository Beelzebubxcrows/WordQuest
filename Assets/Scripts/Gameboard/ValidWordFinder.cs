using System;
using System.Collections.Generic;
using Utility;
using Utility.Dictionary;

namespace Gameboard
{
    public class ValidWordFinder : IDisposable
    {
        private readonly LetterTileRegistry _tileRegistry;
        private readonly DictionaryHelper _dictionaryHelper;

        public ValidWordFinder()
        {
            _tileRegistry = InstanceManager.GetInstanceAsSingle<LetterTileRegistry>();
            _dictionaryHelper = InstanceManager.GetInstanceAsSingle<DictionaryHelper>();
        }
        
        public List<LetterTile> GetTilesToMakeAValidWord()
        {
            
            var allTiles = _tileRegistry.GetTilesByCharacter();
            var frequency = _tileRegistry.GetCharacterFrequency();

            var tries = 3;
            
            string validWord;
            var initialLength = 3;
            while (true) {
                validWord = GetValidWords(frequency, "", initialLength);
                
                if (validWord.Length > 0) {
                    break;
                }
                
                initialLength++;
                if (initialLength >= 7 ) {
                    // if (tries>0) {
                    //     _powerUpManager.SetPowerUpEligible(true);
                    //     _powerUpManager.PlayShufflePowerUp();
                    //     allTiles = _tileRegistry.GetTilesByCharacter();
                    //     frequency = _tileRegistry.GetCharacterFrequency();
                    //     _powerUpManager.SetPowerUpEligible(false);
                    //     initialLength = 3;
                    //     tries--;
                    // }
                    // else {
                    //     break;
                    // }
                    break;
                }
            }
            
            var tilesToMakeWord = new List<LetterTile>();
            var usedTile = new HashSet<LetterTile>();
            foreach (var  character in validWord)
            {
                var tileOfCharacter = allTiles[character];
                foreach (var tile in tileOfCharacter)
                {
                    if (!usedTile.Contains(tile)) {
                        tilesToMakeWord.Add(tile);
                        usedTile.Add(tile);
                        break;
                    }
                }
                
            }
            
            return tilesToMakeWord;
        }

        private string GetValidWords(IList<int> letters, string word, int length)
        {
            
            if (_dictionaryHelper.IsWordValid(word)) {
                return word;
            }

            if (length <= 0) {
                return string.Empty;
            }
            
            var ans = string.Empty;
            for (var i = 0; i < 26; i++)
            {
                if (letters[i] <= 0) {
                    continue;
                }
                
                letters[i]--;
                var temp = GetValidWords(letters, word + (char)(i+'A'), length - 1);
                if (!string.IsNullOrEmpty(temp) && temp.Length>ans.Length) {
                    ans = temp;
                }

                letters[i]++;
            }
            
            return ans;
        }
        public void Dispose()
        {
        }
    }
}