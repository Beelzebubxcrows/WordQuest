using System;
using System.Collections.Generic;
using Events;
using Gameboard;
using Utility;

namespace Tutorial
{
    public class TutorialManager : IDisposable
    {
        private List<LetterTile> _markedTiles;
        private int _index;
        private List<LetterTile> _tilesToMakeAWord;
        private readonly EventBus _eventBus;
        private readonly LetterTileRegistry _tileRegistry;

        public TutorialManager()
        {
            _tileRegistry = InstanceManager.GetInstanceAsSingle<LetterTileRegistry>();
            _eventBus = InstanceManager.GetInstanceAsSingle<EventBus>();
        }
        

        public void StartTutorial()
        {
            ToggleAllTilesOnBoard(false);

            _markedTiles = new List<LetterTile>();
            _eventBus.Register<TileClicked>(OnTileClick);
            var validWordFinder = InstanceManager.GetInstanceAsSingle<ValidWordFinder>();
            
            _tilesToMakeAWord = validWordFinder.GetTilesToMakeAValidWord();
            _index = 0;
            
            StartSteps(_index);
        }

        private void ToggleAllTilesOnBoard(bool isClickable)
        {
            var allTilesOnBoard = _tileRegistry.GetAllTilesOnBoard();
            foreach (var tile in allTilesOnBoard) {
                tile.isClickable = isClickable;
            }
        }

        private void StartSteps(int tileIndex)
        {
            if (_tilesToMakeAWord.Count == tileIndex) {
                MarkTutorialComplete();
                return;
            }
            
            _tilesToMakeAWord[tileIndex].ToggleTutorialMark(true);
            _tilesToMakeAWord[tileIndex].isClickable = true;
            _markedTiles.Add(_tilesToMakeAWord[tileIndex]);
        }

        private void MarkTutorialComplete()
        {
            ToggleAllTilesOnBoard(true);
            _eventBus.Unregister<TileClicked>(OnTileClick);
            _tilesToMakeAWord.Clear();
            _index = 0;
        }

        private void OnTileClick(TileClicked signalData)
        {
            foreach (var tile in _markedTiles) {
                tile.ToggleTutorialMark(false);
                tile.isClickable = false;
            }
            
            _index++;
            StartSteps(_index);
        }

        public void Dispose()
        {
        }
    }
}