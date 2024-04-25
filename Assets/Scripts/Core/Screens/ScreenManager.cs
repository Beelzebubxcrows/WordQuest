using System;
using Core.Screens;
using Transition;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;


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
        
        public async void ShowPopup<T>(string popupAssetId, Transform canvas) where T : Popup
        {
            var assetManager = InstanceManager.GetInstanceAsSingle<AssetManager>();
            var popupGameObject = await assetManager.InstantiateAsync(popupAssetId, canvas);
            var popup = popupGameObject.GetComponent<T>();
            popup.Initialise();
        }
        
        public void Dispose()
        {
        }
    }
}