using System.Collections;
using System.Text;
using Configurations;
using TMPro;
using UnityEngine;
using Utility;
using Utility.Dictionary;

namespace Gameboard
{
    public class GameplayHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text movesLeftText;
        [SerializeField] private TMP_Text targetText;
        [SerializeField] private TMP_Text matchedWord;
        [SerializeField] private float matchDelay;
        
        private DictionaryHelper _dictionaryHelper;
        private StringBuilder _stringBuilder;
        private SoundPlayer _soundPlayer;
        private int _target;
        private int _movesLeft;
        private LetterTileRegistry _tileRegistry;


        public void Initialise(LevelConfig levelConfig)
        {
            targetText.text = levelConfig.Target.ToString();
            _target = levelConfig.Target;
            movesLeftText.text = levelConfig.Moves.ToString();
            _movesLeft = levelConfig.Moves;

            _tileRegistry = InstanceManager.GetInstanceAsSingle<LetterTileRegistry>();
            _soundPlayer = InstanceManager.GetInstanceAsSingle<SoundPlayer>();
            _dictionaryHelper = InstanceManager.GetInstanceAsSingle<DictionaryHelper>();
            _stringBuilder = new StringBuilder();
        }
        
        public void AddCharacter(LetterTile clickedTile)
        {
            _tileRegistry.RegisterSelectedTile(clickedTile);
            _stringBuilder.Append(clickedTile.GetCharacter());
            matchedWord.text = _stringBuilder.ToString();
            _soundPlayer.PlayClickSound();
            if (_dictionaryHelper.IsWordValid(_stringBuilder.ToString()))
            {
                StartCoroutine(OnMatch());
            }
            
           
        }

        public void RemoveCharacter()
        {
            var clickedTiles = _tileRegistry.GetSelectedTiles();
            foreach (var clickedLetterTile in clickedTiles)
            {
                clickedLetterTile.ToggleOff();
            }

            _tileRegistry.ClearSelectedTiles();
            _stringBuilder.Clear();
            matchedWord.text = "";
            _movesLeft -= 1;
            movesLeftText.text = _movesLeft.ToString();
        }

        private IEnumerator OnMatch()
        {
            _soundPlayer.PlayMatchSound();
            PlayMatchAnimationOnTiles();
            StartCoroutine(ShowMatchedWord(_stringBuilder.ToString()));
            yield return new WaitForSeconds(matchDelay);
            
            _target  -= _stringBuilder.Length;
            targetText.text = _target.ToString();
            _movesLeft -= 1;
            movesLeftText.text = _movesLeft.ToString();

            var clickedTiles = _tileRegistry.GetSelectedTiles();
            foreach (var clickedLetterTile in clickedTiles)
            {
                clickedLetterTile.OnMatchComplete();
            }
            _stringBuilder.Clear();
            _tileRegistry.ClearSelectedTiles();
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
    }
}