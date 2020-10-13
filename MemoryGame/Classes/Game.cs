using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace MemoryGame.Classes
{
    public class Card
    {
        public int? AtMove { get; set; }
        public ImageSource Back { get; set; }
        public int Column { get; set; }
        public ImageSource Front { get; set; }
        public Image Image { get; set; }
        public bool IsTurned { get; set; }
        public int Row { get; set; }
        public Player TurnedBy { get; set; }

        public void SetAtMove(Game game)
        {
            List<Card> cards = game.CardCollection.Where(x => x.AtMove != null).ToList();
            if (cards.Count() == 0)
                AtMove = 0;

            else
            {
                cards = cards.OrderBy(x => x.AtMove).ToList();
                AtMove = cards.Last().AtMove + 1;
            }
        }
    }

    public class Game
    {
        public GameConfig Config;
        public List<Card> CardCollection { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public int Round { get; set; }
        public PlayerTurn Turn { get; set; }
        public enum PlayerTurn { Player1, Player2 }

        public Game(GameConfig config)
        {
            Config = config;
            CardCollection = new List<Card>();
            Player1 = new Player() { Name = config.PlayerName1, Score = config.startScore, Time = new TimeSpan(0, 0, 0) };
            Player2 = new Player() { Name = config.PlayerName2, Score = config.startScore, Time = new TimeSpan(0, 0, 0) };
            Turn = config.StartPlayer;
            Round = 0;
        }

        public Player GetActivePlayer()
        {
            if (Turn == PlayerTurn.Player1)
                return Player1;

            else if (Turn == PlayerTurn.Player2)
                return Player2;

            else
                throw new Exception("There's no player at turn.", null);
        }

        public Card GetCard(Image image)
        {
            return CardCollection.Where(x => x.Image == image).First();
        }

        public Card GetLastCard()
        {
            List<Card> cards = CardCollection.Where(x => x.AtMove != null).OrderBy(x => x.AtMove).ToList();
            return cards[cards.Count() - 2];
        }

        public void SwitchTurn()
        {
            if (Turn == PlayerTurn.Player1)
                Turn = PlayerTurn.Player2;
            else
                Turn = PlayerTurn.Player1;
            Round++;
        }
    }

    public class Player
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public TimeSpan Time { get; set; }
    }
}
