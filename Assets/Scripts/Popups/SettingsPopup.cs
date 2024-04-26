using Core;
using Core.Screens;
using Persistence.PersistenceManager;
using UnityEngine;
using Utility;


namespace Popups
{
    public class SettingsPopup : Popup
    {
        [SerializeField] private GameObject musicButton;
        [SerializeField] private GameObject effectButton;
        
        
        private bool _musicState;
        private bool _effectsState;
        private PlayerPersistenceManager _playerPersistenceManager;

        
        
        public override void Initialise()
        {
            _playerPersistenceManager = InstanceManager.GetInstanceAsSingle<PlayerPersistenceManager>();
            UpdateView();

            base.Initialise();
        }

        private void UpdateView()
        {
            _musicState = _playerPersistenceManager.GetMusicState();
            _effectsState = _playerPersistenceManager.GetEffectsState();
            musicButton.gameObject.SetActive(_musicState);
            effectButton.gameObject.SetActive(_effectsState);
        }

        public void OnClickClose()
        {
            UpdateStatesInPersistence();
            HandlePopupClose();
        }

        private void UpdateStatesInPersistence()
        {
            if (_playerPersistenceManager.GetMusicState() != _musicState) {
                _playerPersistenceManager.SetMusicState(_musicState);
            }

            if (_playerPersistenceManager.GetEffectsState()!= _effectsState) {
                _playerPersistenceManager.SetEffectsState(_effectsState);
            }
        }


        public void ToggleMusic()
        {
            _musicState =!_musicState;
            musicButton.gameObject.SetActive(_musicState);
        }

        public void ToggleEffects()
        {
            _effectsState =!_effectsState;
            effectButton.gameObject.SetActive(_effectsState);
        }
        
        
        public void OnClickQuit()
        {
            var screenManager = InstanceManager.GetInstanceAsSingle<ScreenManager>();
            screenManager.DoAfterTransition(()=>Application.Quit(),3);
        }
    }
}