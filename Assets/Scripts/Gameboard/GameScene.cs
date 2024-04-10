using UnityEngine;
using Utility;

namespace Gameboard
{
    public class GameScene : MonoBehaviour
    {
        [SerializeField] private SelectionHandler selectionHandler;
        [SerializeField] private GameBoard gameBoard;

        private void Awake()
        {
            InstanceManager.BindInstanceAsSingle(new LetterTileRegistry());
            selectionHandler.Initialise();
            InstanceManager.GetInstanceAsSingle<SoundPlayer>().PlayAmbientMusic();
            Initialise();
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