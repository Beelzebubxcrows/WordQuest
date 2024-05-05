using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Configurations;
using Core;
using Core.Firebase;
using Events;
using Persistence.PersistenceManager;
using Popups;
using TMPro;
using UnityEngine;
using Utility;
using Utility.Animation;
using Utility.Dictionary;

namespace Gameboard
{
    public class GameplayHandler : MonoBehaviour, IDisposable
    {
        [SerializeField] private Sprite scoreSprite;
        [SerializeField] private GameObject FTUEMarkOnTick;
        [SerializeField] private Color rightColor;
        
        [SerializeField] private Transform tickMark;
        
        [SerializeField] private PunchScale playTargetPunchScale;
        [SerializeField] private PunchScale movesPunchScale;
        [SerializeField] private Transform gameplayCanvas;
        [SerializeField] private TMP_Text movesLeftText;
        [SerializeField] private TMP_Text targetText;
        [SerializeField] private TMP_Text matchedWord;
        [SerializeField] private float matchDelay;

        private bool _matchOngoing;
        private DictionaryHelper _dictionaryHelper;
        private StringBuilder _stringBuilder;
        private SoundPlayer _soundPlayer;
        private int _target;
        private int _movesLeft;
        private LetterTileRegistry _tileRegistry;
        private bool _isGameOver;
        private AssetManager _assetManager;


        public void Initialise(LevelConfig levelConfig)
        {
            _levelConfig = levelConfig;
            _isGameOver = false;
            targetText.text = levelConfig.Target.ToString();
            _target = levelConfig.Target;
            movesLeftText.text = levelConfig.Moves.ToString();
            _movesLeft = levelConfig.Moves;

            _firebaseManager = InstanceManager.GetInstanceAsSingle<FirebaseManager>();
            _assetManager = InstanceManager.GetInstanceAsSingle<AssetManager>();
            _tileRegistry = InstanceManager.GetInstanceAsSingle<LetterTileRegistry>();
            _soundPlayer = InstanceManager.GetInstanceAsSingle<SoundPlayer>();
            _dictionaryHelper = InstanceManager.GetInstanceAsSingle<DictionaryHelper>();
            _stringBuilder = new StringBuilder();
        }
        

        public void OnClickCheck()
        {
            if (_stringBuilder.Length == 0) {
                return;
            }

            StartCoroutine(AnimationManager.PlayPunchScale(tickMark));

            var eventBus = InstanceManager.GetInstanceAsSingle<EventBus>();
            eventBus.Fire(new TickClicked());
            
            if (_dictionaryHelper.IsWordValid(_stringBuilder.ToString()))
            {
                _matchOngoing = true;
                
                //Caching values for animation.
                var clickedTiles = new List<LetterTile>(_tileRegistry.GetSelectedTiles());
                var score = _stringBuilder.Length;
                
                //Update data.
                UpdateDataOnMatch();
                
                //Update in view.
                StartCoroutine(OnMatch(clickedTiles, score));
            }else {
                
                _soundPlayer.PlayFailSound();
                
                var clickedTiles = _tileRegistry.GetSelectedTiles();
                foreach (var clickedLetterTile in clickedTiles)
                {
                    clickedLetterTile.ToggleOff();
                }
                
                ResetLetterTile();
                _movesLeft -= 1;
                FlyScore(tickMark,movesLeftText.transform,-1,movesPunchScale,rightColor);
                TryGameEnd();
            }
        }

        public void ClearSelectedLetters()
        {
            if (!CanUndo()) {
                return;
            }

            var clickedTiles = _tileRegistry.GetSelectedTiles();
            foreach (var clickedLetterTile in clickedTiles)
            {
                clickedLetterTile.ToggleOff();
            }

            ResetLetterTile();
        }

        public bool CanUndo()
        {
            return _stringBuilder.Length > 0;
        }
        
        
        public void AddCharacter(LetterTile clickedTile)
        {
            if (!IsInteractionEligible())
            {
                return;
            }

            clickedTile.ToggleOn();
            _tileRegistry.RegisterSelectedTile(clickedTile);
            _stringBuilder.Append(clickedTile.GetCharacter());
            matchedWord.text = _stringBuilder.ToString();
            _soundPlayer.PlayClickSound();
        }

        private void UpdateDataOnMatch()
        {
            _target -= _stringBuilder.Length;
            _movesLeft -= 1;
            _tileRegistry.AddMatchedWord(_stringBuilder.ToString());
            _stringBuilder.Clear();
            _tileRegistry.ClearSelectedTiles();
        }

        private IEnumerator OnMatch(List<LetterTile> clickedTiles, int score)
        {
            yield return new WaitForSeconds(0.1f);

            _soundPlayer.PlayMatchSound();
            PlayMatchAnimationOnTiles(clickedTiles);
            StartCoroutine(ShowMatchedWord());

            FlyScore(tickMark,targetText.transform,score,playTargetPunchScale,rightColor);

            yield return new WaitForSeconds(matchDelay);
            

            
            foreach (var clickedLetterTile in clickedTiles)
            {
                clickedLetterTile.OnMatchComplete();
            }
            
            _matchOngoing = false;

            TryGameEnd();
        }


        
        private LevelConfig _levelConfig;
        private PunchScale _punchScale;
        public bool shouldOpenSettings = true;
        public bool shouldOpenInfo = true;
        private FirebaseManager _firebaseManager;

        private async void FlyScore(Transform parent, Transform target, int score, PunchScale punchScale, Color color)
        {
            _punchScale = punchScale;
            var scoreGameObject = await _assetManager.InstantiateAsync("pf_score",parent);
            var spawnedScore = scoreGameObject.GetComponent<Score>();
            spawnedScore.Initialise(score.ToString(),scoreSprite, target, OnReached,0.6f);
        }

        private void OnReached(GameObject spawnedGameObject)
        {
            _assetManager.ReleaseAsset(spawnedGameObject);
            StartCoroutine(_punchScale.PlayPunchScale());
            targetText.text = Math.Max(0, _target).ToString();
            movesLeftText.text = Math.Max(0, _movesLeft).ToString();
        }


        private void PlayMatchAnimationOnTiles(List<LetterTile> clickedTiles)
        {
            foreach (var clickedLetterTile in clickedTiles)
            {
                clickedLetterTile.PlayMatchAnimation();
            }
        }

        private IEnumerator ShowMatchedWord()
        {
            var wordAnimated = matchedWord.text;
            var count = 2;
            while (count-- > 0)
            {
                wordAnimated += "!";
                matchedWord.text = wordAnimated;
                yield return new WaitForSeconds(0.05f);
            }

            yield return new WaitForSeconds(0.1f);
            matchedWord.text = "";
        }

        public void ResetLetterTile()
        {
            _tileRegistry.ClearSelectedTiles();
            _stringBuilder.Clear();
            matchedWord.text = "";
        }

        private void TryGameEnd()
        {
            if (_target <= 0 && _movesLeft >= 0)
            {
                _isGameOver = true;
                TriggerWin();
            }

            if (_target > 0 && _movesLeft <= 0)
            {
                _isGameOver = true;
                TriggerLose();
            }
        }

        #region LEVEL LOSS

        private async void TriggerLose()
        {
            var assetManager = InstanceManager.GetInstanceAsSingle<AssetManager>();
            var outroPopup = await assetManager.InstantiateAsync("pf_outroPopup", gameplayCanvas);
            var levelOutroPopup = outroPopup.GetComponent<OutroPopup>();
            levelOutroPopup.Initialise(false,_levelConfig);
            
            _soundPlayer.PlayMatchSound();
            
            _firebaseManager.LogPuzzleLost(_levelConfig.Level);
        }

        #endregion

        
        #region LEVEL WIN

        private void TriggerWin()
        {
            UpdateDataPostWin();
            OpenLevelOutroPostWin();
            _soundPlayer.PlayMatchSound();
            _firebaseManager.LogPuzzleWon(_levelConfig.Level);
        }

        private async void OpenLevelOutroPostWin()
        {
            var assetManager = InstanceManager.GetInstanceAsSingle<AssetManager>();
            var outroPopup = await assetManager.InstantiateAsync("pf_outroPopup", gameplayCanvas);
            var levelOutroPopup = outroPopup.GetComponent<OutroPopup>();
            levelOutroPopup.Initialise(true,_levelConfig);
        }

        private static void UpdateDataPostWin()
        {
            var persistenceManager = InstanceManager.GetInstanceAsSingle<ProgressPersistenceManager>();
            persistenceManager.IncrementLatestLevel();
        }

        #endregion

        private bool IsInteractionEligible()
        {
            return !_matchOngoing && !_isGameOver;
        }

        public void ToggleFTUEMarkOnTick(bool toggle)
        {
            FTUEMarkOnTick.gameObject.SetActive(toggle);
        }

        public void ToggleAllTilesOnBoard(bool isClickable)
        {
            var allTilesOnBoard = _tileRegistry.GetAllTilesOnBoard();
            foreach (var tile in allTilesOnBoard) {
                tile.isClickable = isClickable;
            }
        }
        
        
        public void OpenSettings()
        {
            if (!shouldOpenSettings) {
                return;
            }
            var screenManager = InstanceManager.GetInstanceAsSingle<ScreenManager>();
            screenManager.ShowPopup<SettingsPopup>("pf_settingsPopup",gameplayCanvas);
        }

        public void OpenInfoPanel()
        {
            if (!shouldOpenInfo) {
                return;
            }
            var screenManager = InstanceManager.GetInstanceAsSingle<ScreenManager>();
            screenManager.ShowPopup<HintPopup>("pf_hintPopup",gameplayCanvas);
        }

        public IEnumerator OpenInfoPanelAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            OpenInfoPanel();
        }


        public void Dispose()
        {
        }
    }
}