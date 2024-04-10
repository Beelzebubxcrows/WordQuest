using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gameboard;
using UnityEngine;
using Utility;
using Utility.Dictionary;

namespace Powerups
{
    public class HintPowerUp : MonoBehaviour
    {
        private LetterTileRegistry _tileRegistry;
        private GameplayHandler _gameplayHandler;
        private DictionaryHelper _dictionaryHelper;

        public void Initialise()
        {
            _tileRegistry = InstanceManager.GetInstanceAsSingle<LetterTileRegistry>();
            _gameplayHandler = InstanceManager.GetInstanceAsSingle<GameplayHandler>();
            _dictionaryHelper = InstanceManager.GetInstanceAsSingle<DictionaryHelper>();
        }

        public void PlayHint()
        {
            _gameplayHandler.ResetLetterTile();
            var tilesOnBoard = _tileRegistry.GetAllTilesOnBoard();
            foreach (var tile in tilesOnBoard)
            {
                tile.ToggleOff();
            }

            StartCoroutine(ExecuteHintOnBoard());
            
        }

        private IEnumerator ExecuteHintOnBoard()
        {
            var allTilesOnBoard = _tileRegistry.GetAllTilesOnBoard();
            var letterTiles = GetTilesToMakeAValidWord(allTilesOnBoard);

            foreach (var tile in letterTiles)
            {
                tile.OnClick();
                yield return new WaitForSeconds(0.35f);
            }
        }

        private List<LetterTile> GetTilesToMakeAValidWord(List<LetterTile> allTilesOnBoard)
        {
            
            var allTiles = new Dictionary<char, List<LetterTile>>();
            var frequency = new int[26];
            foreach (var tile in allTilesOnBoard)
            {
                frequency[tile.GetCharacter()-'A']++;
                if (!allTiles.ContainsKey(tile.GetCharacter())) {
                    allTiles.Add(tile.GetCharacter(),new List<LetterTile>(){tile});
                }
                else {
                    allTiles[tile.GetCharacter()].Add(tile);
                }
            }

            var tilesToMakeWord = new List<LetterTile>();
            var validWord = GetValidWords(frequency,"", 4);
            Debug.LogError(validWord);
            
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

        private string GetValidWords(int[] letters, string word, int length)
        {
            
            if (_dictionaryHelper.IsWordValid(word)) {
                return word;
            }

            if (length <= 0) {
                return string.Empty;
            }
            
            for (var i = 0; i < 26; i++)
            {
                if (letters[i] <= 0) {
                    continue;
                }
                
                letters[i]--;
                var temp = GetValidWords(letters, word + (char)(i+'A'), length - 1);
                if (!string.IsNullOrEmpty(temp)) {
                    return temp;
                }
                
                letters[i]++;
            }
            
            return string.Empty;
        }
    }
}