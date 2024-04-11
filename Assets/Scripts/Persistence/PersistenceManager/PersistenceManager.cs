using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Persistence.Data;
using UnityEngine;
using Utility;

namespace Persistence.PersistenceManager
{
    public class PersistenceManager : IDisposable
    {
        private  const string FILE_NAME = "ta_persistence.json";
        private PersistenceData _persistenceData;
        private readonly ProgressPersistenceManager _progressPersistenceManager;
        private readonly PlayerPersistenceManager _playerPersistenceManager;

        public PersistenceManager()
        {
            _progressPersistenceManager = new ProgressPersistenceManager(this);
            _playerPersistenceManager = new PlayerPersistenceManager(this);
            
            InstanceManager.BindInstanceAsSingle(_progressPersistenceManager);
            InstanceManager.BindInstanceAsSingle(_playerPersistenceManager);
        }
        
        public async Task LoadPersistence()
        {
            if (File.Exists(GetPath())) {
                var textAsset = await File.ReadAllTextAsync(GetPath());
                _persistenceData = JsonConvert.DeserializeObject<PersistenceData>(textAsset);
            }
            
            if (_persistenceData == null) {
                _persistenceData = new PersistenceData();
            }
            
            _playerPersistenceManager.Load(_persistenceData.PlayerData);
            _progressPersistenceManager.Load(_persistenceData.ProgressData);
        }

        public void SaveContent()
        {
            var stringData = JsonConvert.SerializeObject(_persistenceData);
            Debug.LogError(stringData);
            File.WriteAllText(GetPath(), stringData);
        }


        private string GetPath()
        {
            return Path.Combine(Application.persistentDataPath, FILE_NAME);
        }


        public void Dispose()
        {
        }
    }
}