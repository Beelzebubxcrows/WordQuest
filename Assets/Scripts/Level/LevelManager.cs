using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Configurations;
using Newtonsoft.Json;
using UnityEngine;
using Utility;

namespace Level
{
    public class LevelManager : IDisposable
    {
        private readonly Dictionary<int, LevelConfig> _loadedLevels;
        private readonly AssetManager _assetManager;
        private string LEVEL_SET_NAME = "LevelConfigSet_{0}";
        
        public LevelManager()
        {
            _loadedLevels = new Dictionary<int, LevelConfig>();
            _assetManager = InstanceManager.GetInstanceAsSingle<AssetManager>();
        }

        public async Task<LevelConfig> LoadLevel(int levelToLoad)
        {
            if (_loadedLevels.ContainsKey(levelToLoad))
            {
                return _loadedLevels[levelToLoad];
            }

            var assetName = string.Format(LEVEL_SET_NAME, GetSetForLevel(levelToLoad));
            var levelSet = await _assetManager.LoadAssetAsync<TextAsset>(assetName);
            var loadedLevels = JsonConvert.DeserializeObject<List<LevelConfig>>(levelSet.ToString());

            foreach (var levelConfig in loadedLevels)
            {
                _loadedLevels.Add(levelConfig.Level, levelConfig);
            }

            return _loadedLevels[levelToLoad];
        }
        

        private int GetSetForLevel(int level)
        {
            return (int)Math.Ceiling(level / 100.0f);
        }


        public void Dispose()
        {
        }
    }
}