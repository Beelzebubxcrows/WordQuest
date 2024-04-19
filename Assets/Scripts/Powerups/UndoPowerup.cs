using Gameboard;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Utility.Animation;

namespace Powerups
{
    public class UndoPowerUp : MonoBehaviour
    {
        [SerializeField] private Button hintButton;
        
        private GameplayHandler _gameplayHandler;
        private PowerUpManager _powerUpManager;

        public void Initialise(PowerUpManager powerUpManager)
        {
            _powerUpManager = powerUpManager;
            _gameplayHandler = InstanceManager.GetInstanceAsSingle<GameplayHandler>();
        }

        public void PlayUndo()
        {
            if (!_powerUpManager.IsPowerUpEligible()) {
                return;
            }

            if (!_gameplayHandler.CanUndo()) {
                return;
            }
            
            hintButton.interactable = false;
            _powerUpManager.SetPowerUpEligible(false);

            StartCoroutine(AnimationManager.PlayButtonFeedback(hintButton.gameObject.transform));
            InstanceManager.GetInstanceAsSingle<SoundPlayer>().PlayShuffleSound();
            _gameplayHandler.ClearSelectedLetters();
            
            _powerUpManager.SetPowerUpEligible(true);
            hintButton.interactable = true;
        }
        

        public void Dispose()
        {
        }
    }
}