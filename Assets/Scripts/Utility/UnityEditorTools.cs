#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using Configurations;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Random = System.Random;
#endif

namespace Utility
{
    public abstract class UnityEditorTools
    {
        #if UNITY_EDITOR
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
                var rows = random.Next((100-level)/15, (100-level)/20 + 3);
                var columns = random.Next((100-level)/15, (100-level)/20 + 3);
                var target = 5 + level * 5;
                var moves = 0;
                if (level < 2)
                {
                    moves = target;
                } else if (level < 5)
                {
                    moves = target / 2;
                } 
                else if (level < 15)
                {
                    moves = (int)Math.Floor( target / 2.5f);
                }
                else if (level < 30)
                {
                    moves = (int)Math.Floor( target / 3f);
                } else if (target < 60)
                {
                    moves = (int)Math.Floor( target / 3.5f);
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
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "LevelConfigSet_1.json"), json);
        }
#endif
    }
}