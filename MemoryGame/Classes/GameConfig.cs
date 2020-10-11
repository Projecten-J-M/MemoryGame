using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Classes
{
    public class GameConfig
    {
        public int FieldHeight { get; set; }
        public int FieldWidth { get; set; }
        public String PlayerName1 { get; set; }
        public String PlayerName2 { get; set; }
        public int startScore { get; set; }
        public Game.PlayerTurn StartPlayer { get; set; }
    }
}
