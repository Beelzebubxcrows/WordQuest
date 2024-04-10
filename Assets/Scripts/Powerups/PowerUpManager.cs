using System;
using UnityEngine;

namespace Powerups
{
    public class PowerUpManager : MonoBehaviour, IDisposable
    {
        [SerializeField] private HintPowerUp hintPowerUp;
        [SerializeField] private ShufflePowerUp shufflePowerUp;
        
        public void Initialise()
        {
            hintPowerUp.Initialise();
            shufflePowerUp.Initialise();
        }
        
        public void Dispose()
        {
        }
    }
}