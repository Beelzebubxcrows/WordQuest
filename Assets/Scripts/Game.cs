using System;
using DefaultNamespace;
using Level;
using Persistence.PersistenceManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;
using Utility.Dictionary;

public class Game : MonoBehaviour
{
        [SerializeField] private SoundPlayer soundPlayer;
        private DictionaryHelper _dictionaryHelper;
        private PersistenceManager _persistentManager;

        private void Start()
        { 
                DontDestroyOnLoad(this);
                BindDependencies();
                ReadFiles(OpenGameScene);
        }

        private async void ReadFiles(Action onReadComplete)
        {
                await _dictionaryHelper.ReadFile();
                await _persistentManager.LoadPersistence();
                onReadComplete?.Invoke();
        }

        private void BindDependencies()
        {
                InstanceManager.BindInstanceAsSingle(new AssetManager());
                
                _persistentManager = InstanceManager.BindInstanceAsSingle(new PersistenceManager());
                InstanceManager.BindInstanceAsSingle(new LevelManager());
                
                InstanceManager.BindInstanceAsSingle(soundPlayer);
                _dictionaryHelper = new DictionaryHelper();
                InstanceManager.BindInstanceAsSingle(_dictionaryHelper);
                
                //Inventory System depends on persistence manager
                InstanceManager.BindInstanceAsSingle(new InventorySystem());
        }

        
        private void OpenGameScene()
        {
                var persistenceManager = InstanceManager.GetInstanceAsSingle<ProgressPersistenceManager>();
                var latestLevel = persistenceManager.GetLatestLevel();
                persistenceManager.SetCurrentLevel(latestLevel);
                
                SceneManager.LoadScene("Gameplay");
        }

       
}