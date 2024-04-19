using Gameboard;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Utility.Animation;

namespace Powerups
{
    public class ShufflePowerUp : MonoBehaviour
    {
        [SerializeField] private Button shuffleButton;
        
        private LetterTileRegistry _tileRegistry;
        private GameplayHandler _gameplayHandler;
        private PowerUpManager _powerUpManager;

        public void Initialise(PowerUpManager powerUpManager)
        {
            _powerUpManager = powerUpManager;
            _tileRegistry = InstanceManager.GetInstanceAsSingle<LetterTileRegistry>();
            _gameplayHandler = InstanceManager.GetInstanceAsSingle<GameplayHandler>();
        }
        
        public void Shuffle()
        {
            if (!_powerUpManager.IsPowerUpEligible()) {
                return;
            }
            
            _powerUpManager.SetPowerUpEligible(false);
            StartCoroutine(AnimationManager.PlayButtonFeedback(shuffleButton.gameObject.transform));
            _gameplayHandler.ResetLetterTile();
            
            var tilesOnBoard = _tileRegistry.GetAllTilesOnBoard();
            foreach (var tile in tilesOnBoard)
            {
                tile.ToggleOff();
            }

            foreach (var tile in tilesOnBoard)
            {
                tile.AllotNewCharacter();
                tile.PlayShuffleAnimation();
            }
            InstanceManager.GetInstanceAsSingle<SoundPlayer>().PlayShuffleSound();
            _powerUpManager.SetPowerUpEligible(true);
        }

        public void Dispose()
        {
        }
    }
}