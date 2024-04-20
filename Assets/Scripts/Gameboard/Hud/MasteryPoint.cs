using System.Collections;
using DefaultNamespace;
using Events;
using TMPro;
using UnityEngine;
using Utility;

namespace Gameboard.Hud
{
    public class MasteryPoint : MonoBehaviour
    {

        [SerializeField] private TMP_Text inventory;

        private InventorySystem _inventorySystem;
        private SoundPlayer _soundPlayer;
        private EventBus _eventBus;
        private int _masteryPoints;

        public void Initialise()
        {
            _inventorySystem = InstanceManager.GetInstanceAsSingle<InventorySystem>();
            _soundPlayer = InstanceManager.GetInstanceAsSingle<SoundPlayer>();
            _eventBus = InstanceManager.GetInstanceAsSingle<EventBus>();
            _eventBus.Register<InventoryGranted>(OnInventoryGranted);
            _eventBus.Register<InventoryDeducted>(OnInventoryDeducted);
            
            SyncDataViewWithPersistence();
        }
        
        private void SyncDataViewWithPersistence()
        {
            _masteryPoints = _inventorySystem.GetInventoryCount(InventoryType.MasteryPoint);
            inventory.text = _masteryPoints.ToString();
        }

        
        #region INVENTORY CHANGE


        private void OnInventoryDeducted(InventoryDeducted obj)
        {
            if (obj.InventoryType == InventoryType.MasteryPoint)
            {
                StartCoroutine(PlayDeductionAnimation(obj.Amount));
            }
        }
        

        private void OnInventoryGranted(InventoryGranted obj)
        {
            if (obj.InventoryType == InventoryType.MasteryPoint)
            {
                StartCoroutine(PlayGrantAnimation(obj.Amount));
            }
        }

        
        private IEnumerator PlayGrantAnimation(int pointsGiven)
        {
            var timeDuration = 2;
            var fromMasteryPoint = _masteryPoints;
            
            var targetMasteryPoint = fromMasteryPoint + pointsGiven;
            var diff = pointsGiven / timeDuration;
            while (fromMasteryPoint < targetMasteryPoint)
            {
                _soundPlayer.PlayClickSound();
                inventory.text = fromMasteryPoint.ToString();
                yield return new WaitForSeconds(0.1f);
                fromMasteryPoint += diff;
            }
            
            SyncDataViewWithPersistence();
        }
        
        
        private IEnumerator PlayDeductionAnimation(int pointsDeduced)
        {
            var timeDuration = 2;
            var fromMasteryPoint = _masteryPoints;
            
            var targetMasteryPoint = fromMasteryPoint - pointsDeduced;
            var diff = pointsDeduced / timeDuration;
            while (fromMasteryPoint > targetMasteryPoint)
            {
                inventory.text = fromMasteryPoint.ToString();
                yield return new WaitForSeconds(0.1f);
                fromMasteryPoint -= diff;
            }
            
            SyncDataViewWithPersistence();
        }
        

        #endregion

        
        public void Dispose()
        {
            _eventBus.Unregister<InventoryGranted>(OnInventoryGranted);
            _eventBus.Unregister<InventoryDeducted>(OnInventoryDeducted);
        }

    }
}