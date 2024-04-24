using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utility.Animation;
using Random = System.Random;

namespace Transition
{
    public class TransitionManager : MonoBehaviour
    {
        [SerializeField] private List<string> hintSet1;
        [SerializeField] private List<string> hintSet2;
        [SerializeField] private List<string> hintSet3;
        
        [SerializeField] private TMP_Text hintText;
        [SerializeField] private CanvasGroup canvasGroup;

        private int _index;
        private Dictionary<int, List<string>> _hintSets;
        private int _hintSetToUse;
        private Random _random;

        private Dictionary<int, int> _hintSetLastIndex;

        void Start()
        {
            _random = new Random();
            DontDestroyOnLoad(gameObject);
            _hintSets = new Dictionary<int, List<string>>
            {
                { 1, hintSet1 },
                { 2, hintSet2 },
                { 3, hintSet3 }
            };
            _hintSetLastIndex = new Dictionary<int, int>
            {
                { 1, -1 },
                { 2, -1 },
                { 3, -1 }
            };
            
            _index = 0;
        }
        
        public IEnumerator PlayTransitionIn(Action onAnimationComplete, int hintSet)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            
            _hintSetToUse = hintSet;
            _index = _hintSetLastIndex[_hintSetToUse]==-1?_random.Next(0,_hintSets[_hintSetToUse].Count):_hintSetLastIndex[_hintSetToUse];
            OnClickNext();
            
            yield return StartCoroutine(AnimationManager.PlayFadeIn(canvasGroup, 0.5f));
            yield return new WaitForSeconds(2.5f);
            
            onAnimationComplete?.Invoke();
        }

        private void SetText(int index)
        {
            if (index >= 0 && index < _hintSets[_hintSetToUse].Count) {
                hintText.text = _hintSets[_hintSetToUse][_index];
            }
        }

        public void OnClickNext()
        {
            _index = (_index + 1)%_hintSets[_hintSetToUse].Count;
            
            _hintSetLastIndex[_hintSetToUse] = _index;
            SetText(_index);
        }

        
        public void OnClickPrevious()
        {
            _index = (_index - 1);
            if (_index < 0) {
                _index = _hintSets[_hintSetToUse].Count - 1;
            }
            
            _hintSetLastIndex[_hintSetToUse] = _index;
            SetText(_index);
        }

        public IEnumerator PlayTransitionOut(Action onTransitionComplete)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            
            StartCoroutine(AnimationManager.PlayFadeOut(canvasGroup, 0.5f));
            
            yield return new WaitForSeconds(0.3f);
            onTransitionComplete?.Invoke();
        }
        
    }
}