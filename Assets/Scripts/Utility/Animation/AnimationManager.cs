using System.Collections;
using UnityEngine;


namespace Utility.Animation
{
    public abstract class AnimationManager
    {
        public static IEnumerator PlayFadeOut(CanvasGroup canvasGroup, float duration)
        {
            LeanTween.alphaCanvas(canvasGroup, 0f, duration);
            yield return new WaitForSeconds(duration);
        }
        
        public static IEnumerator PlayFadeIn(CanvasGroup canvasGroup, float duration)
        {
            LeanTween.alphaCanvas(canvasGroup, 1f, duration);
            yield return new WaitForSeconds(duration);
        }
        
        public static IEnumerator PlayScaleIn(GameObject gameObject, float duration)
        {
            LeanTween.scale(gameObject, new Vector3(1.0f,1.0f,1.0f), duration);
            yield return new WaitForSeconds(duration);
        }
        
        public static IEnumerator PlayScaleOut(GameObject gameObject, float duration)
        {
            LeanTween.scale(gameObject, new Vector3(0.0f,0.0f,0.0f), duration);
            yield return new WaitForSeconds(duration);
        }

        public static IEnumerator PlayPunchScale(Transform button)
        {
            LeanTween.scale(button.gameObject, new Vector3(1.2f,1.2f,1f), 0.1f);
            yield return new WaitForSeconds(0.1f);
            LeanTween.scale(button.gameObject, new Vector3(1.0f,1.0f,1.0f), 0.1f);
        }
    }
}