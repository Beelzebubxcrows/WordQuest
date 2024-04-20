using System.Collections;
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
    public class HintPowerUp : MonoBehaviour
    {
        [SerializeField] private GameObject costTag;
        [SerializeField] private TMP_Text cost;
        
        [SerializeField] private GameObject inventoryCountTag;
        [SerializeField] private TMP_Text inventoryCount;
        
        [SerializeField] private Button hintButton;
        
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

        public void PlayHint()
        {
            _powerUpManager.ProcessOnClick(InventoryType.HintPowerUp,ExecuteHint,UpdateView);
        }

        private void ExecuteHint()
        {
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
            
            _gameplayHandler.OnClickCheck();

            _powerUpManager.SetPowerUpEligible(true);
            hintButton.interactable = true;
        }
        
        
        private void UpdateView()
        {
            var inventory = _inventorySystem.GetInventoryCount(InventoryType.HintPowerUp);
            inventoryCountTag.gameObject.SetActive(inventory>0);
            costTag.gameObject.SetActive(inventory<=0);
            
            inventoryCount.text = inventory.ToString();
            cost.text = InstanceManager.GetInstanceAsSingle<EconomyManager>().GetCost(InventoryType.HintPowerUp).ToString();
        }


        public void Dispose()
        {
        }
    }
}