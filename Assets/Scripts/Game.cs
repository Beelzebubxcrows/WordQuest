using System;
using System.Threading.Tasks;
using Core;
using DefaultNamespace;
using Economy;
using Level;
using Persistence.PersistenceManager;
using Splash;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;
using Utility.Dictionary;

public class Game : MonoBehaviour
{
        [SerializeField]private ScreenManager screenManager;
        [SerializeField] private SplashScene splashScene;
        [SerializeField] private SoundPlayer soundPlayer;
        private DictionaryHelper _dictionaryHelper;
        private PersistenceManager _persistentManager;

        private int _mutexCounter;
        private ScreenManager _screenManager;
        private const int TASK_COUNT = 2;

        private void Start()
        { 
                _mutexCounter = 0;
                DontDestroyOnLoad(this);
                
                BindDependencies();
                
                splashScene.PlayAnimation(OnAnimationComplete);
                ReadFiles();
        }

        private void OnAnimationComplete()
        { 
                OnMutexTaskComplete();
        }

        private async void ReadFiles()
        {
                await _dictionaryHelper.ReadFile();
                await _persistentManager.LoadPersistence();
                OnMutexTaskComplete();
        }

        private void OnMutexTaskComplete()
        {
                _mutexCounter++;
                if (_mutexCounter >= TASK_COUNT)
                {
                        OpenGameScene();
                }
        }
        
        private void BindDependencies()
        {
                InstanceManager.BindInstanceAsSingle(new EventBus());
                InstanceManager.BindInstanceAsSingle(new AssetManager());
                _screenManager = InstanceManager.BindInstanceAsSingle( screenManager);
         
                InstanceManager.BindInstanceAsSingle(new RandomCharacterSelector());
                
                _persistentManager = InstanceManager.BindInstanceAsSingle(new PersistenceManager());
                InstanceManager.BindInstanceAsSingle(new LevelManager());
                
                InstanceManager.BindInstanceAsSingle(soundPlayer);
                _dictionaryHelper = new DictionaryHelper();
                InstanceManager.BindInstanceAsSingle(_dictionaryHelper);
                
                //Inventory System depends on persistence manager
                InstanceManager.BindInstanceAsSingle(new InventorySystem());
                InstanceManager.BindInstanceAsSingle(new EconomyManager());
        }

        
        private void OpenGameScene()
        {
                var persistenceManager = InstanceManager.GetInstanceAsSingle<ProgressPersistenceManager>();
                var latestLevel = persistenceManager.GetLatestLevel();
                persistenceManager.SetCurrentLevel(latestLevel);
                
                _screenManager.ShowScreenWithTransition("Gameplay", 1);
        }

       
}