using System.Collections;
using Configurations;
using DefaultNamespace;
using Persistence.PersistenceManager;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;
using Random = System.Random;


namespace Popups
{
    public class OutroPopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text masteryPointText;
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
        }

        private IEnumerator ShowMasteryPoint()
        {
            if (!_isLevelWon) {
                yield break;
            }
            var soundPlayer = InstanceManager.GetInstanceAsSingle<SoundPlayer>();
            var inventoryManager = InstanceManager.GetInstanceAsSingle<InventorySystem>();
            var masteryPoint = inventoryManager.GetInventoryCount(InventoryType.MasteryPoint);
            var random = new Random();
            var pointsGiven = _levelConfig.Target * random.Next(_levelConfig.Level , _levelConfig.Level +3);
            
            inventoryManager.IncrementInventoryCount(InventoryType.MasteryPoint,pointsGiven);
            
            var targetMasteryPoint = masteryPoint + pointsGiven;
            yield return new WaitForSeconds(0.3f);
            while (masteryPoint < targetMasteryPoint)
            {
                soundPlayer.PlayClickSound();
                masteryPointText.text = masteryPoint.ToString();
                yield return new WaitForSeconds(0.1f);
                masteryPoint += 20;
            }
        }

        private void SetupUI(bool isLevelWon)
        {
            resultText.text = isLevelWon? "YOU WON !" : "TRY AGAIN !";
            retryButton.SetActive(!isLevelWon);
            playButton.SetActive(isLevelWon);
            
            var inventoryManager = InstanceManager.GetInstanceAsSingle<InventorySystem>();
            var masteryPoint = inventoryManager.GetInventoryCount(InventoryType.MasteryPoint);
            masteryPointText.text = masteryPoint.ToString();
        }



        #region ONCLICK

        public void OnClickRetry()
        {
            SceneManager.LoadScene("Gameplay");
        }

        public void OnClickPlay()
        {
            var persistenceManager = InstanceManager.GetInstanceAsSingle<ProgressPersistenceManager>();
            var currentLevel = persistenceManager.GetCurrentLevel();
            persistenceManager.SetCurrentLevel(currentLevel+1);

            SceneManager.LoadScene("Gameplay");
        }

        #endregion
    }
}