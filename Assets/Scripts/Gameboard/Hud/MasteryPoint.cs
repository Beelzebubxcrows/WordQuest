using System.Collections;
using System.Globalization;
using DefaultNamespace;
using Events;
using TMPro;
using Unity.Mathematics;
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
                StartCoroutine(PlayDeductionAnimation(obj.Amount,2));
            }
        }
        

        private void OnInventoryGranted(InventoryGranted obj)
        {
        }

        
        public IEnumerator PlayGrantAnimation(int pointsGiven, float timeDuration)
        {
            var fromMasteryPoint = _masteryPoints * 1.0f;
            
            var targetMasteryPoint = fromMasteryPoint + pointsGiven;
            var diff = (int)math.floor(pointsGiven / timeDuration * 1.0f);
            while (fromMasteryPoint < targetMasteryPoint)
            {
                inventory.text = fromMasteryPoint.ToString(CultureInfo.InvariantCulture);
                yield return new WaitForSeconds(0.1f);
                fromMasteryPoint += diff;
            }
            
            SyncDataViewWithPersistence();
        }


        private IEnumerator PlayDeductionAnimation(int pointsDeduced, int timeDuration)
        {
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