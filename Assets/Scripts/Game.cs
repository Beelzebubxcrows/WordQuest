using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;
using Utility.Dictionary;

public class Game : MonoBehaviour
{
        [SerializeField] private SoundPlayer soundPlayer;
        private DictionaryHelper _dictionaryHelper;

        private void Start()
        { 
                DontDestroyOnLoad(this);
                BindDependencies();
                ReadFiles(OpenGameScene);
        }

        private async void ReadFiles(Action onReadComplete)
        {
                await _dictionaryHelper.ReadFile();
                onReadComplete?.Invoke();
        }

        private void BindDependencies()
        {
                InstanceManager.BindInstanceAsSingle(new InventorySystem());
                InstanceManager.BindInstanceAsSingle(new AssetManager());
                InstanceManager.BindInstanceAsSingle(soundPlayer);
                _dictionaryHelper = new DictionaryHelper();
                InstanceManager.BindInstanceAsSingle(_dictionaryHelper);
        }

        
        private void OpenGameScene()
        {
                SceneManager.LoadScene("Gameplay");
        }

       
}