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
    public class ShufflePowerUp : MonoBehaviour
    {
        [SerializeField] private GameObject costTag;
        [SerializeField] private TMP_Text cost;
        
        [SerializeField] private GameObject inventoryCountTag;
        [SerializeField] private TMP_Text inventoryCount;
        
        [SerializeField] private Button shuffleButton;
        
        private LetterTileRegistry _tileRegistry;
        private GameplayHandler _gameplayHandler;
        private PowerUpManager _powerUpManager;
        private InventorySystem _inventorySystem;

        public void Initialise(PowerUpManager powerUpManager)
        {
            _powerUpManager = powerUpManager;
            _tileRegistry = InstanceManager.GetInstanceAsSingle<LetterTileRegistry>();
            _gameplayHandler = InstanceManager.GetInstanceAsSingle<GameplayHandler>();
            _inventorySystem = InstanceManager.GetInstanceAsSingle<InventorySystem>();
            
            UpdateView();
        }
        
        public void Shuffle()
        {
            _powerUpManager.ProcessOnClick(InventoryType.ShufflePowerUp,ExecuteShuffle,UpdateView);
        }

        private void ExecuteShuffle()
        {
            _powerUpManager.SetPowerUpEligible(false);
            StartCoroutine(AnimationManager.PlayPunchScale(shuffleButton.gameObject.transform));
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

        private void UpdateView()
        {
            var inventory = _inventorySystem.GetInventoryCount(InventoryType.ShufflePowerUp);
            inventoryCountTag.gameObject.SetActive(inventory>0);
            costTag.gameObject.SetActive(inventory<=0);
            
            inventoryCount.text = inventory.ToString();
            cost.text = InstanceManager.GetInstanceAsSingle<EconomyManager>().GetCost(InventoryType.ShufflePowerUp).ToString();
        }
        

        public void Dispose()
        {
        }
    }
}