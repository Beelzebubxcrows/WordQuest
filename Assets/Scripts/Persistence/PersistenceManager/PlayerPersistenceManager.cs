using Persistence.Data;

namespace Persistence.PersistenceManager
{
    public class PlayerPersistenceManager : IPersistenceManager
    {
        private PlayerData _playerData;
        private Persistence.PersistenceManager.PersistenceManager _persistenceManager;

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

        public void Dispose()
        {
        }
    }
}