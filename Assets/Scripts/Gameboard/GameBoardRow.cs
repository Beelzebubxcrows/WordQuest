using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Gameboard
{
    public class GameBoardRow : MonoBehaviour
    {
        [SerializeField] private List<LetterTile> tiles;

        private LetterTileRegistry _tileRegistry;
        
        public void Initialise()
        {
            _tileRegistry = InstanceManager.GetInstanceAsSingle<LetterTileRegistry>();
        }
        
        public void SetupTiles(List<char> levelConfigCharacter, GameplayHandler gameplayHandler)
        {
            for (var i = 0; i < tiles.Count; i++)
            {
                if (i >= levelConfigCharacter.Count)
                {
                    tiles[i].gameObject.SetActive(false);
                }
                else
                {
                    tiles[i].Initialise(levelConfigCharacter[i], gameplayHandler);
                    _tileRegistry.RegisterTileOnBoard(tiles[i]);
                }
            }
        }
    }
}