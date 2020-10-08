using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Classes
{
    public class Game
    {
        public GameConfig Config;
        public int PlayerScore1 { get; set; }
        public int PlayerScore2 { get; set; }
        public TimeSpan PlayerTime1 { get; set; }
        public TimeSpan PlayerTime2 { get; set; }

        public Game.PlayerTurn Turn { get; set; }

        public enum PlayerTurn { Player1, Player2 }

        public Game(GameConfig config)
        {
            Config = config;
        }
    }
}
