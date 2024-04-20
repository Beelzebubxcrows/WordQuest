using System.Collections;
using Configurations;
using DefaultNamespace;
using Gameboard;
using Gameboard.Hud;
using Persistence.PersistenceManager;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;


namespace Popups
{
    public class OutroPopup : MonoBehaviour
    {
        [SerializeField] private MasteryPoint masteryPoint;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private GameObject retryButton;
        [SerializeField] private GameObject playButton;
        private bool _isLevelWon;
        private LevelConfig _levelConfig;

        public void Initialise(bool isLevelWon, LevelConfig levelConfig)
        {
            _levelConfig = levelConfig;
            _isLevelWon = isLevelWon;
            SetupUI(isLevelWon);
            StartCoroutine(ShowMasteryPoint());
            InitialiseComponents();
        }

        private void InitialiseComponents()
        {
            masteryPoint.Initialise();
        }

        private IEnumerator ShowMasteryPoint()
        {
            if (!_isLevelWon) {
                yield break;
            }
            
            var inventoryManager = InstanceManager.GetInstanceAsSingle<InventorySystem>();
            var tileRegistry = InstanceManager.GetInstanceAsSingle<LetterTileRegistry>();
            var wordsMatched = tileRegistry.GetMatchedWordsInLevel();
            
            var pointsGiven = 0;
            foreach (var word in wordsMatched)
            {
                var multiplier = 1;
                switch (word.Length)
                {
                    case < 4:
                        multiplier = 1;
                        break;
                    case < 6:
                        multiplier = 2;
                        break;
                    default:
                        multiplier = 4;
                        break;
                }
                
                pointsGiven += word.Length * multiplier + _levelConfig.Level * (int)math.ceil(_levelConfig.Level/10.0f);
            }
            
            inventoryManager.GrantInventory(InventoryType.MasteryPoint,pointsGiven);
        }

        private void SetupUI(bool isLevelWon)
        {
            resultText.text = isLevelWon? "YOU WON !" : "TRY AGAIN !";
            retryButton.SetActive(!isLevelWon);
            playButton.SetActive(isLevelWon);
        }



        #region ONCLICK

        public void OnClickRetry()
        {
            Dispose();
            SceneManager.LoadScene("Gameplay");
        }

        public void OnClickPlay()
        {
            Dispose();
            var persistenceManager = InstanceManager.GetInstanceAsSingle<ProgressPersistenceManager>();
            var currentLevel = persistenceManager.GetCurrentLevel();
            persistenceManager.SetCurrentLevel(currentLevel+1);

            SceneManager.LoadScene("Gameplay");
        }

        #endregion


        private void Dispose()
        {
            masteryPoint.Dispose();
        }
    }
}