using UnityEngine;
using UnityEngine.EventSystems;
using Utility;

namespace Gameboard
{
    public class SelectionHandler : MonoBehaviour, IDragHandler, IPointerClickHandler
    {
        private LetterTileRegistry _tileRegistry;
        
        public void Initialise()
        {
            _tileRegistry = InstanceManager.GetInstanceAsSingle<LetterTileRegistry>();
        }

        public void PrintPosition()
        {
            var tilesOnBoard = _tileRegistry.GetAllTilesOnBoard();
            foreach (var tile in tilesOnBoard)
            {
                var position = Camera.main.WorldToScreenPoint(tile.transform.localPosition);
                Debug.Log($"{tile.GetCharacter()} => {position} : {tile.transform.localPosition}");
            }
        }


        public void OnDrag(PointerEventData eventData)
        {
            var tilesOnBoard = _tileRegistry.GetAllTilesOnBoard();
            foreach (var tile in tilesOnBoard)
            {
                if (IsPointerOnTile(tile, eventData))
                {
                   Debug.Log($"Clicked on {tile.GetCharacter()}");
                }
            }
        }

        private bool IsPointerOnTile(LetterTile tile, PointerEventData eventData)
        {
            var position = Camera.main.WorldToScreenPoint(tile.transform.position);
            
            return eventData.position.x>=position.x-0.5f && eventData.position.x<=position.x+0.5f && eventData.position.y>=position.y-0.5f && eventData.position.y<=position.y+0.5f;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log(eventData.position+" "+Camera.main.WorldToScreenPoint(transform.position));
        }
    }
}