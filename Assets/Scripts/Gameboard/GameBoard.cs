using System.Collections.Generic;
using Configurations;
using UnityEngine;
using Utility;
using Random = System.Random;

namespace Gameboard
{
    public class GameBoard : MonoBehaviour
    {
        [SerializeField]private List<GameBoardRow> letterTilesRows;
        [SerializeField] private GameplayHandler gameplayHandler;
        
        private LevelConfig _levelConfig;
        
        
        public void Initialise()
        {
            LoadConfig();
            InitialiseComponents();
            LoadUI();
            
        }
        
        private void LoadConfig()
        {
            var random = new Random();
            _levelConfig = new LevelConfig(1,random.Next(5, 6),random.Next(4, 5), random.Next(90,100),random.Next(40, 50));
            for (var i = 0; i < _levelConfig.Rows; i++)
            {
                var characters = new List<char>();
                for (var j = 0; j < _levelConfig.Columns; j++)
                {
                    characters.Add((char)(random.Next(0,25)+'A'));
                }
                _levelConfig.Characters.Add(characters);
            }
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
        }
    }
}