using System.Collections.Generic;

namespace Configurations
{
    public class LevelConfig
    {
        public int Level { get; set; }  
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int Target { get; set; }
        public int Moves { get; set; }
        public readonly List<List<char>> Characters;
        
        public LevelConfig(int level, int rows, int columns, int target, int moves)
        {
            Level = level;
            Rows = rows;
            Columns = columns;
            Target = target;
            Moves = moves;
            Characters = new List<List<char>>();
        }
        
    }
}