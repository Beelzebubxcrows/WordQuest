using Gameboard;
using UnityEngine;
using Utility;

namespace Powerups
{
    public class ShufflePowerUp : MonoBehaviour
    {
        private LetterTileRegistry _tileRegistry;
        private GameplayHandler _gameplayHandler;

        public void Initialise()
        {
            _tileRegistry = InstanceManager.GetInstanceAsSingle<LetterTileRegistry>();
            _gameplayHandler = InstanceManager.GetInstanceAsSingle<GameplayHandler>();
        }
        
        public void Shuffle()
        {
            _gameplayHandler.ResetLetterTile();
            
            var tilesOnBoard = _tileRegistry.GetAllTilesOnBoard();
            foreach (var tile in tilesOnBoard)
            {
                tile.ToggleOff();
            }

            foreach (var tile in tilesOnBoard)
            {
                tile.AllotNewCharacter();
                tile.PlayShuffleAnimation();
            }
            InstanceManager.GetInstanceAsSingle<SoundPlayer>().PlayShuffleSound();
        }
    }
}