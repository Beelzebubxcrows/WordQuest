using Events;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utility;
using Random = System.Random;

namespace Gameboard
{
    public class LetterTile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private GameObject upperEffect;
        [SerializeField] private ParticleSystem tapParticle;
        [SerializeField] private ParticleSystem matchParticle;
        [SerializeField] private ParticleSystem shuffleParticle;
        [SerializeField] private Color unselectedColor;
        [SerializeField] private Color selectedColor;
        [SerializeField] private Color matchColor;
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text displayCharacter;
        private GameplayHandler _gameplayHandler;
        private bool _isClicked;
        private char _character;
        private Random _random;
        private EventBus _eventBus;
        public bool isClickable;


        public void Initialise(char c, GameplayHandler gameplayHandler)
        {
            isClickable = true;
            _eventBus = InstanceManager.GetInstanceAsSingle<EventBus>();
            _random = new Random();
            _character = c;
            _gameplayHandler = gameplayHandler;
            displayCharacter.text = _character.ToString();
            image.color = GetDefaultColor();
            ToggleOff();
        }
        
        public void OnClick()
        {
            if (!isClickable) {
                return;
            }
            Debug.Log($"Clicked on {_character}");
            if (!_isClicked) {
                _gameplayHandler.AddCharacter(this);
            }
            else {
                _gameplayHandler.RemoveCharacter();
            }
            
            
            _eventBus.Fire(new TileClicked(this));
        }

        public void ToggleOff()
        {
            image.color = GetDefaultColor();
            _isClicked = false;
        }

        public void ToggleOn()
        {
            image.color = GetSelectedColor();
            _isClicked = true;
        }


        public char GetCharacter()
        {
            return _character;
        }

        public void OnMatchComplete()
        {
            ToggleOff();
            AllotNewCharacter();
        }

        public void AllotNewCharacter()
        {
            var characterAllocator = InstanceManager.GetInstanceAsSingle<RandomCharacterSelector>();
            _character = characterAllocator.SelectRandomCharacter(_random);
            image.color = GetDefaultColor();
            displayCharacter.text = _character.ToString();
        }


        private Color GetDefaultColor()
        {
            return unselectedColor;
        }
        
        
        private Color GetSelectedColor()
        {
            return selectedColor;
        }

        public void PlayMatchAnimation()
        {
            image.color = matchColor;
            matchParticle.Play();
        }

        public void PlayShuffleAnimation()
        {
            image.color = unselectedColor;
            shuffleParticle.Play();
        }

        public void ToggleTutorialMark(bool isActive)
        {
            upperEffect.gameObject.SetActive(isActive);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            tapParticle.Play();
            LeanTween.scale(gameObject, new Vector3(1.2f,1.2f,1f), 0.1f);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
           // tapParticle.Stop();
            LeanTween.scale(gameObject, new Vector3(1.0f,1.0f,1.0f), 0.1f);
        }
    }
}