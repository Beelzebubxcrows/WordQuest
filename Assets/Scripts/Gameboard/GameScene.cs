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
            InstanceManager.BindInstanceAsSingle(new LetterTileRegistry());
            selectionHandler.Initialise();
            InstanceManager.GetInstanceAsSingle<SoundPlayer>().PlayAmbientMusic();
            Initialise();
            powerUpManager.Initialise();
            selectionHandler.PrintPosition();
        }

        private void Initialise()
        {
            gameBoard.Initialise();
        }

        public void OpenSettings()
        {
            
        }

        public void OpenInfoPanel()
        {
            
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Dispose()
        {
            InstanceManager.UnbindInstanceAsSingle<LetterTileRegistry>();
            powerUpManager.Dispose();
            selectionHandler.Dispose();
            gameBoard.Dispose();
        }
    }
}