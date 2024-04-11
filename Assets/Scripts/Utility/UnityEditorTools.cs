using System;
using System.Collections.Generic;
using System.IO;
using Configurations;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace Utility
{
    public abstract class UnityEditorTools
    {
        [MenuItem("InHouseTools/Progression/ResetData")]
        public static void ResetData()
        {
            PlayerPrefs.DeleteAll();
            File.Delete(Path.Combine(Application.persistentDataPath, "ta_persistence.json"));
        }
        
        
        [MenuItem("InHouseTools/Configuration/GenerateConfig")]
        public static void GenerateConfig()
        {
            var randomCharacterSelector = new RandomCharacterSelector();
            var levels = new List<LevelConfig>();
            
            var level = 1;
            while (level <= 100)
            {
                var random = new Random();
                var rows = (100-level) / 10;
                var columns = (100-level) / 10;
                var target = 20 + level * 5;
                var moves = 1;
                if (level < 10)
                {
                    moves = target;
                } else if (level < 30)
                {
                    moves = target / 2;
                } else if (target < 60)
                {
                    moves = target / 3;
                }
                else
                {
                    moves = (int)Math.Floor(target / 3.5);
                }
                
                var levelConfig = new LevelConfig(level,rows, columns, target, moves);
                
                for (var i = 0; i < levelConfig.Rows; i++)
                {
                    var characters = new List<char>();
                    for (var j = 0; j < levelConfig.Columns; j++)
                    {
                        characters.Add(randomCharacterSelector.SelectRandomCharacter(random));
                    }
                    levelConfig.Characters.Add(characters);
                }
                
                levels.Add(levelConfig);
                level++;
            }
            
            var json = JsonConvert.SerializeObject(levels);
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "ta_levels.json"), json);
        }

    }
}