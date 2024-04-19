using DefaultNamespace;
using Economy;
using Gameboard;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Utility.Animation;

namespace Powerups
{
    public class UndoPowerUp : MonoBehaviour
    {
        [SerializeField] private TMP_Text inventoryCount;
        [SerializeField] private Button hintButton;
        
        private GameplayHandler _gameplayHandler;
        private PowerUpManager _powerUpManager;
        private EconomyManager _economyManager;
        private InventorySystem _inventorySystem;

        public void Initialise(PowerUpManager powerUpManager)
        {
            _powerUpManager = powerUpManager;
            _gameplayHandler = InstanceManager.GetInstanceAsSingle<GameplayHandler>();
            _economyManager = InstanceManager.GetInstanceAsSingle<EconomyManager>();
            _inventorySystem = InstanceManager.GetInstanceAsSingle<InventorySystem>();
        }

        public void PlayUndo()
        {
            if (!_powerUpManager.IsPowerUpEligible()) {
                return;
            }

            if (!_gameplayHandler.CanUndo()) {
                return;
            }

            ExecuteUndo();
            // var inventory = _inventorySystem.GetInventoryCount(InventoryType.UndoPowerUp);
            // if (inventory > 0)
            // {
            //     _inventorySystem.DeductInventory(InventoryType.UndoPowerUp,1);
            //     ExecuteUndo();
            // }
            // else if(_economyManager.TryBuy(InventoryType.UndoPowerUp))
            // {
            //     ExecuteUndo();
            // }
            // // else(check for ads)
            // // {
            // //     
            // // }
            // UpdateView();
        }

        private void ExecuteUndo()
        {
            hintButton.interactable = false;
            _powerUpManager.SetPowerUpEligible(false);

            StartCoroutine(AnimationManager.PlayButtonFeedback(hintButton.gameObject.transform));
            InstanceManager.GetInstanceAsSingle<SoundPlayer>().PlayShuffleSound();
            _gameplayHandler.ClearSelectedLetters();
            
            _powerUpManager.SetPowerUpEligible(true);
            hintButton.interactable = true;
        }

        public void UpdateView()
        {
            inventoryCount.text = _inventorySystem.GetInventoryCount(InventoryType.UndoPowerUp).ToString();
        }


        public void Dispose()
        {
        }
    }
}