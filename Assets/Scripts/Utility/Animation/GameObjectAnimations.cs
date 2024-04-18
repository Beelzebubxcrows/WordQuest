using System.Collections;
using UnityEngine;


namespace Utility.Animation
{
    public class GameObjectAnimations : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        public IEnumerator PlayFadeOut(float duration)
        {
            LeanTween.alphaCanvas(canvasGroup, 0f, duration);
            yield return new WaitForSeconds(duration);
        }
        
        public IEnumerator PlayFadeIn(float duration)
        {
            LeanTween.alphaCanvas(canvasGroup, 1f, duration);
            yield return new WaitForSeconds(duration);
        }
    }
}