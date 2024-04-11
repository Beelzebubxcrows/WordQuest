using System;
using UnityEngine;

namespace Powerups
{
    public class PowerUpManager : MonoBehaviour, IDisposable
    {
        [SerializeField] private HintPowerUp hintPowerUp;
        [SerializeField] private ShufflePowerUp shufflePowerUp;
        private bool _isPowerUpEligible;

        public void Initialise()
        {
            _isPowerUpEligible = true;
            hintPowerUp.Initialise(this);
            shufflePowerUp.Initialise(this);
        }

        public bool IsPowerUpEligible()
        {
            return _isPowerUpEligible;
        }

        public void SetPowerUpEligible(bool isPowerUpEligible)
        {
            _isPowerUpEligible = isPowerUpEligible;
        }

        public void PlayShufflePowerUp()
        {
            shufflePowerUp.Shuffle();
        }

        public void PlayHintPowerUp()
        {
            hintPowerUp.PlayHint();
        }
        
        public void Dispose()
        {
        }
    }
}