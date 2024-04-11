using System.Collections;
using TMPro;
using UnityEngine;
using Utility;

namespace Popups
{
    public class OutroPopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text masteryPointText;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private GameObject retryButton;
        [SerializeField] private GameObject playButton;
        
        public void Initialise(bool isLevelWon)
        {
            SetupUI(isLevelWon);
            StartCoroutine(ShowMasteryPoint());
        }

        private IEnumerator ShowMasteryPoint()
        {
            var soundPlayer = InstanceManager.GetInstanceAsSingle<SoundPlayer>();
            
            yield return new WaitForSeconds(0.3f);
            var masteryPoint = 100;
            while (masteryPoint < 200)
            {
                soundPlayer.PlayClickSound();
                masteryPointText.text = masteryPoint.ToString();
                yield return new WaitForSeconds(0.1f);
                masteryPoint += 10;
            }
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
            
        }

        public void OnClickPlay()
        {
            
        }

        #endregion
    }
}