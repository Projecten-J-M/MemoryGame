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
        public Card[] CardCollection { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        public PlayerTurn Turn { get; set; }

        public enum PlayerTurn { Player1, Player2}
        
        public Game(GameConfig config)
        {
            Config = config;
            Player1 = new Player() { Score = config.startScore, Time = new TimeSpan(0, 0, 0) };
            Player2 = new Player() { Score = config.startScore, Time = new TimeSpan(0, 0, 0) };
            Turn = config.StartPlayer;
        }
    }

    public class Player
    {
        public int Score { get; set; }
        public TimeSpan Time { get; set; }

    }

    public class Card
    {
        public bool IsTurned { get; set; }
        public Uri FrontImageURI { get; set; }
        public Uri BackImageURI { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
