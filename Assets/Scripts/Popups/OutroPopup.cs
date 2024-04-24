using System;
using System.Threading.Tasks;
using Configurations;
using Core;
using DefaultNamespace;
using Gameboard;
using Gameboard.Hud;
using Persistence.PersistenceManager;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Utility;
using Utility.Animation;


namespace Popups
{
    public class OutroPopup : MonoBehaviour
    {
        [SerializeField] private Transform masteryPointFlyTarget;
        [SerializeField] private Sprite masteryPointSprite;
        
        [SerializeField] private PrefabFlyHelper prefabFlyHelper;
        [SerializeField] private MasteryPoint masteryPoint;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private TMP_Text levelNumber;
        [SerializeField] private GameObject retryButton;
        [SerializeField] private GameObject playButton;
        private bool _isLevelWon;
        private LevelConfig _levelConfig;
        private AssetManager _assetProvider;
        private SoundPlayer _soundPlayer;
        private int _pointsGiven;
        private bool _isAnimationPlaying;

        public void Initialise(bool isLevelWon, LevelConfig levelConfig)
        {
            _levelConfig = levelConfig;
            _isLevelWon = isLevelWon;
            
            InitialiseComponents();
            
            SetupUI(isLevelWon);
            
            GrantMasteryPoints();
            FlyPointToMasteryPointHud();
        }

        private void InitialiseComponents()
        {
            _soundPlayer = InstanceManager.GetInstanceAsSingle<SoundPlayer>();
            masteryPoint.Initialise();
        }

        private async void FlyPointToMasteryPointHud()
        {
            _isAnimationPlaying = true;
            await Task.Delay(TimeSpan.FromSeconds(0.5f));
            _assetProvider = InstanceManager.GetInstanceAsSingle<AssetManager>();

            var scoresToFly = Mathf.Clamp(_pointsGiven/50.0f,2,7);
            
            FlyScoresToHud(scoresToFly);
            
            PlayHudGrantAnimation(scoresToFly);
        }

        private async void FlyScoresToHud(float scoresToFly)
        {
            while (scoresToFly-- > 0)
            {
                InstantiateAndFlyScore();
                await Task.Delay(TimeSpan.FromSeconds(0.3f));
                if (scoresToFly <= 0) {
                    _isAnimationPlaying = false;
                }
            }
        }

        private async void PlayHudGrantAnimation(float scoresToFly)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.8f));
            StartCoroutine(masteryPoint.PlayGrantAnimation(_pointsGiven,  (scoresToFly)));
        }

        private async void InstantiateAndFlyScore()
        {
            var spawnedObject = await _assetProvider.InstantiateAsync("pf_score", transform);
            var scoreScript = spawnedObject.GetComponent<Score>();
            scoreScript.Initialise("", masteryPointSprite, masteryPointFlyTarget.transform, OnReached,0.5f);
        }

        private void OnReached(GameObject spawnedObject)
        {
            _soundPlayer.PlayClickSound();
            StartCoroutine(AnimationManager.PlayPunchScale(masteryPoint.transform));
            _assetProvider.ReleaseAsset(spawnedObject);
        }

        private void GrantMasteryPoints()
        {
            if (!_isLevelWon) {
                return;
            }
            
            var inventoryManager = InstanceManager.GetInstanceAsSingle<InventorySystem>();
            var tileRegistry = InstanceManager.GetInstanceAsSingle<LetterTileRegistry>();
            var wordsMatched = tileRegistry.GetMatchedWordsInLevel();
            
            _pointsGiven = 0;
            foreach (var word in wordsMatched)
            {
                var multiplier = 1;
                switch (word.Length)
                {
                    case < 4:
                        multiplier = 1;
                        break;
                    case < 6:
                        multiplier = 2;
                        break;
                    default:
                        multiplier = 4;
                        break;
                }
                
                _pointsGiven += word.Length * multiplier + _levelConfig.Level * (int)math.ceil(_levelConfig.Level/10.0f);
            }
            
            inventoryManager.GrantInventory(InventoryType.MasteryPoint,_pointsGiven);
        }

        private void SetupUI(bool isLevelWon)
        {
            resultText.text = isLevelWon? "YOU WON !" : "TRY AGAIN !";
            retryButton.SetActive(!isLevelWon);
            playButton.SetActive(isLevelWon);
            levelNumber.text = "Level "+_levelConfig.Level;
        }



        #region ONCLICK

        public void OnClickRetry()
        {
            if (_isAnimationPlaying) {
                return;
            }
            
            Dispose();
            var screenManager = InstanceManager.GetInstanceAsSingle<ScreenManager>();
            screenManager.ShowScreenWithTransition("Gameplay",3);
        }

        public void OnClickPlay()
        {
            if (_isAnimationPlaying) {
                return;
            }
            
            Dispose();
            var persistenceManager = InstanceManager.GetInstanceAsSingle<ProgressPersistenceManager>();
            var currentLevel = persistenceManager.GetCurrentLevel();
            persistenceManager.SetCurrentLevel(currentLevel+1);

            var screenManager = InstanceManager.GetInstanceAsSingle<ScreenManager>();
            screenManager.ShowScreenWithTransition("Gameplay",2);
        }

        #endregion


        private void Dispose()
        {
            masteryPoint.Dispose();
        }
    }
}