using System.Collections;
using UnityEngine;

namespace Utility.Animation
{
    public class PopInOutAnimation : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float ANIMATION_TIME_TUNE;
        
        public IEnumerator StartIntroAnimation()
        {
            gameObject.SetActive(true);
            StartCoroutine(nameof(ScaleUp));
            StartCoroutine(nameof(FadeIn));
            yield return new WaitForSeconds(1.5f);
        }

        public IEnumerator StartOutroAnimation()
        {
            StartCoroutine(nameof(ScaleDown));
            StartCoroutine(nameof(FadeOut));
            yield return new WaitForSeconds(2*ANIMATION_TIME_TUNE);
        }

        public IEnumerator ScaleUp()
        {
            for (var scale = 0f; scale <= 1f; scale += 0.1f)
            {
              gameObject.transform.localScale = new Vector3(scale,scale,scale);
                yield return new WaitForSeconds(ANIMATION_TIME_TUNE);
            }
            gameObject.transform.localScale = Vector3.one;
        }
        
        public IEnumerator ScaleDown()
        {
            for (var scale = 0f; scale <= 1f; scale -= 0.1f)
            {
               gameObject.transform.localScale = new Vector3(scale,scale,scale);
                yield return new WaitForSeconds(ANIMATION_TIME_TUNE);
            }
            gameObject.transform.localScale = Vector3.zero;
        }
        
        public IEnumerator FadeIn()
        {
            for (var alpha = 0f; alpha <= 1f; alpha += 0.1f)
            {
                canvasGroup.alpha = alpha;
                yield return new WaitForSeconds(ANIMATION_TIME_TUNE);
            }
            canvasGroup.alpha = 1;
        }
        
        public IEnumerator FadeOut()
        {
            for (var alpha = 0f; alpha <= 1f; alpha -= 0.1f)
            {
                canvasGroup.alpha = alpha;
                yield return new WaitForSeconds(ANIMATION_TIME_TUNE);
            }
            canvasGroup.alpha = 0;
        }
    }
}