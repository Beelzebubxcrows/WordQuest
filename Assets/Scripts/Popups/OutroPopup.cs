using System.Collections;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;
using Random = Unity.Mathematics.Random;

namespace Popups
{
    public class OutroPopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text masteryPointText;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private GameObject retryButton;
        [SerializeField] private GameObject playButton;
        private bool _isLevelWon;

        public void Initialise(bool isLevelWon)
        {
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
            
            var pointsGiven = 275;
            
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
            SceneManager.LoadScene("Gameplay");
        }

        #endregion
    }
}