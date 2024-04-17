using System;
using System.Threading.Tasks;
using DefaultNamespace;
using Level;
using Persistence.PersistenceManager;
using Splash;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;
using Utility.Dictionary;

public class Game : MonoBehaviour
{
        [SerializeField] private SplashScene splashScene;
        [SerializeField] private SoundPlayer soundPlayer;
        private DictionaryHelper _dictionaryHelper;
        private PersistenceManager _persistentManager;

        private int _mutexCounter;
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
         
                InstanceManager.BindInstanceAsSingle(new RandomCharacterSelector());
                
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