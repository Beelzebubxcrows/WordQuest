using Core;
using Core.Firebase;
using Powerups;
using UnityEngine;
using Utility;

namespace Gameboard
{
    public class GameScene : MonoBehaviour
    {
        [SerializeField] private PowerUpManager powerUpManager;
        [SerializeField] private SelectionHandler selectionHandler;
        [SerializeField] private GameBoard gameBoard;

        private void Awake()
        {
            Initialise();
        }

        private async void Initialise()
        {
            InstanceManager.BindInstanceAsSingle(powerUpManager);
            InstanceManager.BindInstanceAsSingle(new LetterTileRegistry());
            selectionHandler.Initialise();
            InstanceManager.GetInstanceAsSingle<SoundPlayer>().PlayAmbientMusic();
            
            await gameBoard.Initialise();
            
            powerUpManager.Initialise();
            
            OnInitialiseComplete();
        }
        
        private void OnInitialiseComplete()
        {
            var screenManager = InstanceManager.GetInstanceAsSingle<ScreenManager>();
            screenManager.PlayTransitionOut();
        }
        

        private void OnDestroy()
        {
            Dispose();
        }

        private void Dispose()
        {
            InstanceManager.UnbindInstanceAsSingle<PowerUpManager>();
            InstanceManager.UnbindInstanceAsSingle<LetterTileRegistry>();
            powerUpManager.Dispose();
            selectionHandler.Dispose();
            gameBoard.Dispose();
        }
    }
}