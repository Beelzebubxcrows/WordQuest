using System;
using System.Collections;
using System.Text;
using Configurations;
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

            StartCoroutine(AnimationManager.PlayButtonFeedback(tickMark));

            var eventBus = InstanceManager.GetInstanceAsSingle<EventBus>();
            eventBus.Fire(new TickClicked());
            
            if (_dictionaryHelper.IsWordValid(_stringBuilder.ToString()))
            {
                _matchOngoing = true;
                StartCoroutine(OnMatch());
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

        private IEnumerator OnMatch()
        {
            yield return new WaitForSeconds(0.1f);

            _soundPlayer.PlayMatchSound();
            PlayMatchAnimationOnTiles();
            StartCoroutine(ShowMatchedWord(_stringBuilder.ToString()));

            FlyScore(tickMark,targetText.transform,_stringBuilder.Length,playTargetPunchScale,rightColor);

            yield return new WaitForSeconds(matchDelay);


            _target -= _stringBuilder.Length;
            _movesLeft -= 1;

            var clickedTiles = _tileRegistry.GetSelectedTiles();
            foreach (var clickedLetterTile in clickedTiles)
            {
                clickedLetterTile.OnMatchComplete();
            }

            _stringBuilder.Clear();
            _tileRegistry.ClearSelectedTiles();
            _matchOngoing = false;

            TryGameEnd();
        }


        private Score _spawnedScore;
        private LevelConfig _levelConfig;
        private PunchScale _punchScale;

        private async void FlyScore(Transform parent, Transform target, int score, PunchScale punchScale, Color color)
        {
            _punchScale = punchScale;
            var scoreGameObject = await _assetManager.InstantiateAsync("pf_score",parent);
            _spawnedScore = scoreGameObject.GetComponent<Score>();
            _spawnedScore.Initialise(score, target, OnReached);
            _spawnedScore.SetColor(color);
        }

        private void OnReached()
        {
            _assetManager.ReleaseAsset(_spawnedScore.gameObject);
            StartCoroutine(_punchScale.PlayPunchScale());
            targetText.text = Math.Max(0, _target).ToString();
            movesLeftText.text = Math.Max(0, _movesLeft).ToString();
        }


        private void PlayMatchAnimationOnTiles()
        {
            var clickedTiles = _tileRegistry.GetSelectedTiles();
            foreach (var clickedLetterTile in clickedTiles)
            {
                clickedLetterTile.PlayMatchAnimation();
            }
        }

        private IEnumerator ShowMatchedWord(string wordMatched)
        {
            var wordAnimated = wordMatched;
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

        private async void TriggerLose()
        {
            var assetManager = InstanceManager.GetInstanceAsSingle<AssetManager>();
            var gameObject = await assetManager.InstantiateAsync("pf_outroPopup", gameplayCanvas);
            var levelOutroPopup = gameObject.GetComponent<OutroPopup>();
            levelOutroPopup.Initialise(false,_levelConfig);
        }

        private async void TriggerWin()
        {
            var persistenceManager = InstanceManager.GetInstanceAsSingle<ProgressPersistenceManager>();
            persistenceManager.IncrementLatestLevel();

            var assetManager = InstanceManager.GetInstanceAsSingle<AssetManager>();
            var gameObject = await assetManager.InstantiateAsync("pf_outroPopup", gameplayCanvas);
            var levelOutroPopup = gameObject.GetComponent<OutroPopup>();
            levelOutroPopup.Initialise(true,_levelConfig);
        }

        private bool IsInteractionEligible()
        {
            return !_matchOngoing && !_isGameOver;
        }

        public void ToggleFTUEMarkOnTick(bool toggle)
        {
            FTUEMarkOnTick.gameObject.SetActive(toggle);
        }

        public void Dispose()
        {
        }
    }
}