using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Gameboard
{
    public class LetterTile : MonoBehaviour
    {
        [SerializeField] private ParticleSystem clickParticle;
        [SerializeField] private ParticleSystem matchParticle;
        [SerializeField] private Color unselectedColor;
        [SerializeField] private Color selectedColor;
        [SerializeField] private Color matchColor;
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text displayCharacter;
        private GameplayHandler _gameplayHandler;
        private bool _isClicked;
        private char _character;
       
        
        public void Initialise(char c, GameplayHandler gameplayHandler)
        {
            _character = c;
            _gameplayHandler = gameplayHandler;
            displayCharacter.text = _character.ToString();
            image.color = GetDefaultColor();
            ToggleOff();
        }
        
        public void OnClick()
        {
            Debug.Log($"Clicked on {_character}");
            if (!_isClicked) {
                ToggleOn();
                _gameplayHandler.AddCharacter(this);
            }
            else {
                _gameplayHandler.RemoveCharacter();
            }
            
        }

        public void ToggleOff()
        {
            image.color = GetDefaultColor();
            _isClicked = false;
        }

        private void ToggleOn()
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
            var random = new Random();
            _character = (char)random.Next(65, 91);
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

    }
}