using System;
using System.Collections.Generic;
using Events;
using Gameboard;
using Powerups;
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
        private readonly GameplayHandler _gameplayHandler;
        private readonly PowerUpManager _powerUpManager;

        public TutorialManager(GameplayHandler gameplayHandler)
        {
            _gameplayHandler = gameplayHandler;
            _tileRegistry = InstanceManager.GetInstanceAsSingle<LetterTileRegistry>();
            _eventBus = InstanceManager.GetInstanceAsSingle<EventBus>();
            _powerUpManager = InstanceManager.GetInstanceAsSingle<PowerUpManager>();
        }
        

        public void StartTutorial()
        {
            ToggleAllTilesOnBoard(false);
            _powerUpManager.SetPowerUpEligible(false);
            _gameplayHandler.shouldOpenInfo = false;
            _gameplayHandler.shouldOpenSettings = false;
            

            _markedTiles = new List<LetterTile>();
            _eventBus.Register<TileClicked>(OnTileClick);
            _eventBus.Register<TickClicked>(OnTickClick);
            var validWordFinder = InstanceManager.GetInstanceAsSingle<ValidWordFinder>();
            
            _tilesToMakeAWord = validWordFinder.GetTilesToMakeAValidWord(3);
            _index = 0;
            
            StartSteps(_index);
        }

        private void OnTickClick(TickClicked obj)
        {
            _gameplayHandler.ToggleFTUEMarkOnTick(false);
            MarkTutorialComplete();
            _gameplayHandler.StartCoroutine(_gameplayHandler.OpenInfoPanelAfterDelay(1f));
        }

        private void ToggleAllTilesOnBoard(bool isClickable)
        {
            _gameplayHandler.ToggleAllTilesOnBoard(isClickable);
        }

        private void StartSteps(int tileIndex)
        {
            if (_tilesToMakeAWord.Count == tileIndex) {
                _gameplayHandler.ToggleFTUEMarkOnTick(true);
                return;
            }
            
            _tilesToMakeAWord[tileIndex].ToggleTutorialMark(true);
            _tilesToMakeAWord[tileIndex].isClickable = true;
            _markedTiles.Add(_tilesToMakeAWord[tileIndex]);
        }

        private void MarkTutorialComplete()
        {
            _gameplayHandler.shouldOpenInfo = true;
            _gameplayHandler.shouldOpenSettings = true;
            _powerUpManager.SetPowerUpEligible(true);
            ToggleAllTilesOnBoard(true);
            
            _eventBus.Unregister<TileClicked>(OnTileClick);
            _eventBus.Unregister<TickClicked>(OnTickClick);
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