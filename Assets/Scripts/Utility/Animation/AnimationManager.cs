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
        
        public static IEnumerator PlayScaleIn(GameObject gameObject, float duration, Vector3 scale)
        {
            LeanTween.scale(gameObject, scale, duration);
            yield return new WaitForSeconds(duration);
        }
        
        public static IEnumerator PlayScaleOut(GameObject gameObject, float duration, Vector3 scale)
        {
            LeanTween.scale(gameObject, scale, duration);
            yield return new WaitForSeconds(duration);
        }

        public static IEnumerator PlayPunchScale(Transform button)
        {
            LeanTween.scale(button.gameObject, new Vector3(1.2f,1.2f,1f), 0.1f);
            yield return new WaitForSeconds(0.1f);
            LeanTween.scale(button.gameObject, new Vector3(1.0f,1.0f,1.0f), 0.1f);
        }
        
        public static IEnumerator PlayPopupInAnimation(CanvasGroup canvasGroup, GameObject animationParent, MonoBehaviour monoBehaviour)
        {
            monoBehaviour.StartCoroutine(PlayScaleIn(animationParent, 0f, new Vector3(0.6f, 0.6f, 0.6f)));
            monoBehaviour.StartCoroutine(PlayFadeIn(canvasGroup, 0.2f));
            monoBehaviour.StartCoroutine(PlayScaleIn(animationParent, 0.2f, new Vector3(1.05f, 1.05f, 1.05f)));
            yield return new WaitForSeconds(0.2f);
            monoBehaviour.StartCoroutine(PlayScaleOut(animationParent, 0.2f, new Vector3(1f,1f,1f)));
        }

        
        
        
    }
}