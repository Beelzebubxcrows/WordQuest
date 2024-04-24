using System;
using Transition;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Core
{
    public class ScreenManager: MonoBehaviour, IDisposable
    {
        [SerializeField] private GameObject transitionCanvas;
        [SerializeField] private TransitionManager transition;

        private void Start()
        {
            DontDestroyOnLoad(transitionCanvas.gameObject);
        }

        public void ShowScreenWithTransition(string sceneName, int hintSetToUse)
        {
            StartCoroutine(transition.PlayTransitionIn(()=>SceneManager.LoadSceneAsync(sceneName),hintSetToUse));
        }

        public void PlayTransitionOut()
        {
            StartCoroutine(transition.PlayTransitionOut(null));
        }

        public void Dispose()
        {
        }
    }
}