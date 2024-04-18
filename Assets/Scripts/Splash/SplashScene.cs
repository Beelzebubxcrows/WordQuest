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

        [SerializeField] private GameObjectAnimations gameObjectAnimations;
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
            
            while (index<5)
            {
                yield return gameObjectAnimations.PlayFadeOut(0.5f);
                
                loadingImage.sprite = sprites[index% sprites.Length];
                index++;
                
                yield return gameObjectAnimations.PlayFadeIn(0.5f);
                
                yield return new WaitForSeconds(1f);
            }
            
            _onAnimationComplete?.Invoke();
            _onAnimationComplete = null;
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
            StopCoroutine(PlayLoadingAnimation());
        }
    }
}