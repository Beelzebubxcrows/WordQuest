using System;
using DefaultNamespace;
using Economy;
using UnityEngine;
using Utility;

namespace Powerups
{
    public class PowerUpManager : MonoBehaviour, IDisposable
    {
        [SerializeField] private UndoPowerUp undoPowerUp;
        [SerializeField] private HintPowerUp hintPowerUp;
        [SerializeField] private ShufflePowerUp shufflePowerUp;
        private bool _isPowerUpEligible;
        private InventorySystem _inventorySystem;
        private EconomyManager _economyManager;
        private SoundPlayer _soundManager;

        public void Initialise()
        {
            _isPowerUpEligible = true;
            hintPowerUp.Initialise(this);
            shufflePowerUp.Initialise(this);
            undoPowerUp.Initialise(this);
            _inventorySystem = InstanceManager.GetInstanceAsSingle<InventorySystem>();
            _economyManager = InstanceManager.GetInstanceAsSingle<EconomyManager>();
            _soundManager = InstanceManager.GetInstanceAsSingle<SoundPlayer>();
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

        public void ProcessOnClick(InventoryType inventoryType, Action executePowerUp, Action updatePowerUpView)
        {
            if (!IsPowerUpEligible()) {
                return;
            }
            
            var inventory = _inventorySystem.GetInventoryCount(inventoryType);
            
            if (inventory > 0) {
                
                _inventorySystem.DeductInventory(inventoryType,1);
                _soundManager.PlayShuffleSound();
                executePowerUp?.Invoke();
                
            }
            else if(_economyManager.TryBuy(inventoryType)) {
                
                _soundManager.PlayShuffleSound();
                executePowerUp?.Invoke();
                
            }
            else {
                _soundManager.PlayFailSound();
            }
            
            updatePowerUpView?.Invoke();
        }
        
        public void Dispose()
        {
            hintPowerUp.Dispose();
            shufflePowerUp.Dispose();
            undoPowerUp.Dispose();
        }
    }
}