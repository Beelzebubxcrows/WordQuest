using Powerups;
using UnityEngine;
using UnityEngine.Serialization;
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
    }
}