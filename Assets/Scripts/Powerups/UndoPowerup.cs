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
        [SerializeField] private GameObject costTag;
        [SerializeField] private TMP_Text cost;
        
        [SerializeField] private GameObject inventoryCountTag;
        [SerializeField] private TMP_Text inventoryCount;
        
        [SerializeField] private Button hintButton;
        
        private GameplayHandler _gameplayHandler;
        private PowerUpManager _powerUpManager;
        private InventorySystem _inventorySystem;

        public void Initialise(PowerUpManager powerUpManager)
        {
            _powerUpManager = powerUpManager;
            _gameplayHandler = InstanceManager.GetInstanceAsSingle<GameplayHandler>();
            _inventorySystem = InstanceManager.GetInstanceAsSingle<InventorySystem>();
            UpdateView();
        }

        public void PlayUndo()
        {

            if (!_gameplayHandler.CanUndo()) {
                return;
            }

            _powerUpManager.ProcessOnClick(InventoryType.UndoPowerUp,ExecuteUndo,UpdateView);
            
        }

        private void ExecuteUndo()
        {
            hintButton.interactable = false;
            _powerUpManager.SetPowerUpEligible(false);

            StartCoroutine(AnimationManager.PlayPunchScale(hintButton.gameObject.transform));
            _gameplayHandler.ClearSelectedLetters();
            
            _powerUpManager.SetPowerUpEligible(true);
            hintButton.interactable = true;
        }

        private void UpdateView()
        {
            var inventory = _inventorySystem.GetInventoryCount(InventoryType.UndoPowerUp);
            inventoryCountTag.gameObject.SetActive(inventory>0);
            costTag.gameObject.SetActive(inventory<=0);
            
            inventoryCount.text = inventory.ToString();
            cost.text = InstanceManager.GetInstanceAsSingle<EconomyManager>().GetCost(InventoryType.UndoPowerUp).ToString();
        }


        public void Dispose()
        {
        }
    }
}