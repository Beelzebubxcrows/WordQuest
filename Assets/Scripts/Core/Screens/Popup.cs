using System.Collections;
using UnityEngine;
using Utility;
using Utility.Animation;

namespace Core.Screens
{
    public abstract class Popup : MonoBehaviour
    {
        [SerializeField] private GameObject animationParent;
        [SerializeField] private CanvasGroup canvasGroup;

        public virtual void Initialise()
        {
            PlayPopupInAnimation();
        }
        
        private void PlayPopupInAnimation()
        {
            StartCoroutine(AnimationManager.PlayPopupInAnimation(canvasGroup, animationParent, this));
        }


        protected IEnumerator HandlePopupClose()
        {
            StartCoroutine(AnimationManager.PlayFadeOut(canvasGroup, 0.2f));
            StartCoroutine(AnimationManager.PlayScaleOut( animationParent, 0.2f,Vector3.zero));
            yield return new WaitForSeconds(0.2f);
            Dispose();
            
        }


        private void Dispose()
        {
            var assetManager = InstanceManager.GetInstanceAsSingle<AssetManager>();
            assetManager.ReleaseAsset(gameObject);
        }
    }
}