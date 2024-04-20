using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility.Animation;


namespace Splash
{
    public class SplashScene : MonoBehaviour, IDisposable
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text loadingText;
        [SerializeField] private Image loadingImage;
        [SerializeField] private Sprite[] sprites;
        private Action _onAnimationComplete;

        public void PlayAnimation(Action onAnimationComplete)
        {
            _onAnimationComplete = onAnimationComplete;
            
            
            StartCoroutine(ShuffleSprite());
            StartCoroutine(PlayLoadingAnimation());
        }

        private IEnumerator ShuffleSprite()
        {
            var index = 1;
            
            while (index<3)
            {
                yield return AnimationManager.PlayFadeOut(canvasGroup,0.5f);
                
                loadingImage.sprite = sprites[index% sprites.Length];
                index++;
                
                yield return AnimationManager.PlayFadeIn(canvasGroup,0.5f);
                
                yield return new WaitForSeconds(1f);
            }
            _onAnimationComplete?.Invoke();
        }
        

        private IEnumerator PlayLoadingAnimation()
        {
            var count = 0;
            var loadingTextValue = "Loading";
            
            while (_onAnimationComplete!=null)
            {
                count %= 4;
                if (count == 0) {
                    loadingTextValue = "Loading";
                }
                
                loadingText.text = loadingTextValue;
                
                yield return new WaitForSeconds(0.5f);
                count++;
                loadingTextValue += ".";
            }
        }

        

        public void Dispose()
        { 
            StopCoroutine(ShuffleSprite());
            StopCoroutine(PlayLoadingAnimation());
        }
    }
}