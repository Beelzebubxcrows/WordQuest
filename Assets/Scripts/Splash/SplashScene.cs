using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Splash
{
    public class SplashScene : MonoBehaviour, IDisposable
    {
        
        [SerializeField] private TMP_Text loadingText;
        
        private Action _onAnimationComplete;

        public void PlayAnimation(Action onAnimationComplete)
        {
            _onAnimationComplete = onAnimationComplete;
            StartCoroutine(PlayLoadingAnimation());
        }

      
        

        private IEnumerator PlayLoadingAnimation()
        {
            var count = 0;
            var loadingTextValue = "Loading";
            
            while (true)
            {
                count %= 4;
                if (count == 0) {
                    loadingTextValue = "Loading";
                    _onAnimationComplete?.Invoke();
                }
                
                loadingText.text = loadingTextValue;
                
                yield return new WaitForSeconds(0.2f);
                count++;
                loadingTextValue += ".";
            }
        }

        

        public void Dispose()
        { 
            StopCoroutine(PlayLoadingAnimation());
        }
    }
}