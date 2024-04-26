using System;
using Persistence.Data;
using Utility;

namespace Persistence.PersistenceManager
{
    public class PlayerPersistenceManager : IPersistenceManager
    {
        public Action SoundStateChanged;
        private PlayerData _playerData;
        private readonly Persistence.PersistenceManager.PersistenceManager _persistenceManager;

        public PlayerPersistenceManager(PersistenceManager persistenceManager)
        {
            _persistenceManager = persistenceManager;
        }
        
        public void Save()
        {
            _persistenceManager.SaveContent();
        }

        public void Load(IPersistenceData persistenceData)
        {
            _playerData = (PlayerData)persistenceData;
        }

        public string GetPersistenceKey()
        {
            return "Player";
        }


        public bool GetMusicState()
        {
            return _playerData.MusicState;
        }
        
        public bool GetEffectsState()
        {
            return _playerData.EffectsState;
        }
        
        public void SetMusicState(bool state)
        {
            _playerData.MusicState = state;
            
            var soundPlayer = InstanceManager.GetInstanceAsSingle<SoundPlayer>();
            soundPlayer.HandleStateChanged();
            Save();
        }
        
        public void SetEffectsState(bool state)
        {
            _playerData.EffectsState = state;
            
            var soundPlayer = InstanceManager.GetInstanceAsSingle<SoundPlayer>();
            soundPlayer.HandleStateChanged();
            Save();
        }

        public void Dispose()
        {
        }
    }
}