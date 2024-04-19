using System.Collections;
using System.Collections.Generic;
using Gameboard;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Utility.Animation;
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
            
            StartCoroutine(AnimationManager.PlayButtonFeedback(hintButton.gameObject.transform));
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
            var validWordFinder = InstanceManager.GetInstanceAsSingle<ValidWordFinder>();
            var letterTiles = validWordFinder.GetTilesToMakeAValidWord(3);

            foreach (var tile in letterTiles)
            {
                tile.OnClick();
                yield return new WaitForSeconds(0.35f);
            }

            _powerUpManager.SetPowerUpEligible(true);
            hintButton.interactable = true;
        }

        

        public void Dispose()
        {
        }
    }
}