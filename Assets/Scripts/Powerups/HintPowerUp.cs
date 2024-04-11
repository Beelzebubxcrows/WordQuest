using System.Collections;
using System.Collections.Generic;
using Gameboard;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Utility.Dictionary;

namespace Powerups
{
    public class HintPowerUp : MonoBehaviour
    {
        [SerializeField] private Button hintButton;
        
        private LetterTileRegistry _tileRegistry;
        private GameplayHandler _gameplayHandler;
        private DictionaryHelper _dictionaryHelper;
        private PowerUpManager _powerUpManager;

        public void Initialise(PowerUpManager powerUpManager)
        {
            _powerUpManager = powerUpManager;
            _tileRegistry = InstanceManager.GetInstanceAsSingle<LetterTileRegistry>();
            _gameplayHandler = InstanceManager.GetInstanceAsSingle<GameplayHandler>();
            _dictionaryHelper = InstanceManager.GetInstanceAsSingle<DictionaryHelper>();
        }

        public void PlayHint()
        {
            if (!_powerUpManager.IsPowerUpEligible()) {
                return;
            }

            hintButton.interactable = false;
            _powerUpManager.SetPowerUpEligible(false);
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

            _powerUpManager.SetPowerUpEligible(true);
            hintButton.interactable = true;
        }

        private List<LetterTile> GetTilesToMakeAValidWord(List<LetterTile> allTilesOnBoard)
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
                    if (tries>0) {
                        _powerUpManager.SetPowerUpEligible(true);
                        _powerUpManager.PlayShufflePowerUp();
                        allTiles = _tileRegistry.GetTilesByCharacter();
                        frequency = _tileRegistry.GetCharacterFrequency();
                        _powerUpManager.SetPowerUpEligible(false);
                        initialLength = 3;
                        tries--;
                    }
                    else {
                        break;
                    }
                   
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

        private string GetValidWords(int[] letters, string word, int length)
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