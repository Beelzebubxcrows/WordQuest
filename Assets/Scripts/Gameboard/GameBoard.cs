using System.Collections.Generic;
using System.Threading.Tasks;
using Configurations;
using Level;
using Persistence.PersistenceManager;
using TMPro;
using UnityEngine;
using Utility;
using Random = System.Random;

namespace Gameboard
{
    public class GameBoard : MonoBehaviour
    {
        [SerializeField] private TMP_Text levelNumber;
        [SerializeField]private List<GameBoardRow> letterTilesRows;
        [SerializeField] private GameplayHandler gameplayHandler;
        
        public string LEVEL_FORMAT = "Level {0}";
        
        private LevelConfig _levelConfig;
        private LevelManager _levelManager;
        private ProgressPersistenceManager _progressPersistenceManager;

        public async Task Initialise()
        {
            _progressPersistenceManager = InstanceManager.GetInstanceAsSingle<ProgressPersistenceManager>();
            _levelManager = InstanceManager.GetInstanceAsSingle<LevelManager>();
            
            await LoadConfig();
            InitialiseComponents();
            LoadUI();
            
        }
        
        private async Task LoadConfig()
        {
            _levelConfig = await _levelManager.LoadLevel(GetLevelToLoad());
        }

        private int GetLevelToLoad()
        {
            return _progressPersistenceManager.GetCurrentLevel();
        }

        private void InitialiseComponents()
        {
            InstanceManager.BindInstanceAsSingle(gameplayHandler);
            gameplayHandler.Initialise(_levelConfig);
            foreach (var letterTilesRow in letterTilesRows)
            {
                letterTilesRow.Initialise();
            }
        }

        private void LoadUI()
        {
            for (var i = 0; i <letterTilesRows.Count; i++)
            {
                if (i >= _levelConfig.Rows)
                {
                    letterTilesRows[i].gameObject.SetActive(false);
                }
                else
                {
                    letterTilesRows[i].gameObject.SetActive(true);
                    letterTilesRows[i].SetupTiles(_levelConfig.Characters[i], gameplayHandler);
                }
            }

            
            levelNumber.text = string.Format(LEVEL_FORMAT,
                _progressPersistenceManager.GetCurrentLevel());
        }

        public void Dispose()
        {
            InstanceManager.UnbindInstanceAsSingle<GameplayHandler>();
        }
    }
}