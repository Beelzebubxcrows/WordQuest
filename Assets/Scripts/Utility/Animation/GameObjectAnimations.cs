using System.Collections;
using UnityEngine;


namespace Utility.Animation
{
    public class GameObjectAnimations : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        public IEnumerator PlayFadeOut(float diff)
        {
            var alpha = canvasGroup.alpha;
            while (alpha > 0)
            {
                canvasGroup.alpha = alpha;
                yield return new WaitForEndOfFrame();
                alpha -= diff;
            }

            canvasGroup.alpha = 0f;
        }
        
        public IEnumerator PlayFadeIn(float diff)
        {
            var alpha = canvasGroup.alpha;
            while (alpha < 1f)
            {
                canvasGroup.alpha = alpha;
                yield return new WaitForEndOfFrame();
                alpha += diff;
            }

            canvasGroup.alpha = 1f;
        }
    }
}