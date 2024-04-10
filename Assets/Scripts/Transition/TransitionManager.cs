using System;
using System.Collections;
using UnityEngine;
using Utility.Animation;
using Utility.Animation.TextAnimations;

namespace Transition
{
    public class TransitionManager : MonoBehaviour
    {
        [SerializeField] private PopInOutAnimation popInOutAnimation;
        [SerializeField] private TextAnimator textAnimator;

        public IEnumerator PLayTransition(Action onTransitionComplete)
        {
            yield return popInOutAnimation.StartIntroAnimation();
            yield return textAnimator.PlayLoadingTextAnimation();
           // yield return popInOutAnimation.StartOutroAnimation();
            onTransitionComplete?.Invoke();
        }
        
    }
}