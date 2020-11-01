using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

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
        public string Thema { get; set; }
        public enum PlayerTurn { Player1, Player2 }

        public Game(GameConfig config)
        {
            Config = config;
            CardCollection = new List<Card>();
            Player1 = new Player() { Name = config.PlayerName1, Score = config.startScore, Time = new TimeSpan(0, 0, 0) };
            Player2 = new Player() { Name = config.PlayerName2, Score = config.startScore, Time = new TimeSpan(0, 0, 0) };
            Thema = config.Thema;
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

        public void LoadGameFromFile()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("game.sav");

            // Get root node for saving the game.
            XmlNode savedGame = xmlDoc.DocumentElement.GetElementsByTagName("savedgame")[0];

            // Get all nodes
            XmlNode height = savedGame.SelectSingleNode("//config/height");
            XmlNode width = savedGame.SelectSingleNode("//config/width");
            XmlNode cardCollection = savedGame.SelectSingleNode("//savedgame/cardcollection");
            XmlNode player1 = savedGame.SelectSingleNode("//savedgame/player1");
            XmlNode player2 = savedGame.SelectSingleNode("//savedgame/player2");
            XmlNode round = savedGame.SelectSingleNode("//savedgame/round");
            XmlNode turn = savedGame.SelectSingleNode("//savedgame/turn");

            Config.FieldHeight = Convert.ToInt32(height.InnerText);
            Config.FieldWidth = Convert.ToInt32(width.InnerText);
            Round = Convert.ToInt32(round.InnerText);
            Turn = turn.InnerText == "Player1" ? PlayerTurn.Player1 : PlayerTurn.Player2;

            Player1.Name = player1.SelectSingleNode("//savedgame/player1/name").InnerText;
            Player1.Score = Convert.ToInt32(player1.SelectSingleNode("//savedgame/player1/score").InnerText);
            string[] time = player1.SelectSingleNode("//savedgame/player1/time").InnerText.Split(':');
            Player1.Time = new TimeSpan(Convert.ToInt16(time[0]), Convert.ToInt16(time[1]), Convert.ToInt16(time[2]));

            Player2.Name = player2.SelectSingleNode("//savedgame/player2/name").InnerText;
            Player2.Score = Convert.ToInt32(player2.SelectSingleNode("//savedgame/player2/score").InnerText);
            time = player2.SelectSingleNode("//savedgame/player2/time").InnerText.Split(':');
            Player2.Time = new TimeSpan(Convert.ToInt16(time[0]), Convert.ToInt16(time[1]), Convert.ToInt16(time[2]));

            foreach (XmlNode node in xmlDoc.SelectNodes("//cardcollection/card"))
            {
                Card card = new Card();
                card.AtMove = String.IsNullOrWhiteSpace(node["atmove"].InnerText) ? (int?) null : Convert.ToInt32(node.SelectSingleNode("//atmove").InnerText);
                card.Back = new BitmapImage(new Uri(node["back"].InnerText, UriKind.Relative));
                card.Column = Convert.ToInt16(node["column"].InnerText);
                card.Front = new BitmapImage(new Uri(node["front"].InnerText, UriKind.Absolute));
                card.IsTurned = Convert.ToBoolean(node["isturned"].InnerText);
                card.Row = Convert.ToInt16(node["row"].InnerText);

                CardCollection.Add(card);
            }
        }

        public void Save()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("game.sav");

            // Get root node for saving the game.
            XmlNode savedGame = xmlDoc.DocumentElement.GetElementsByTagName("savedgame")[0];

            // Get all nodes
            XmlNode height = savedGame.SelectSingleNode("//config/height");
            XmlNode width = savedGame.SelectSingleNode("//config/width");
            XmlNode cardCollection = savedGame.SelectSingleNode("//cardcollection");
            XmlNode player1 = savedGame.SelectSingleNode("//player1");
            XmlNode player2 = savedGame.SelectSingleNode("//player2");
            XmlNode round = savedGame.SelectSingleNode("//round");
            XmlNode turn = savedGame.SelectSingleNode("//turn");

            // Clear previous cards
            cardCollection.InnerText = null;

            height.InnerText = Config.FieldHeight.ToString();
            width.InnerText = Config.FieldWidth.ToString();
            round.InnerText = Round.ToString();
            turn.InnerText = Turn.ToString();

            player1.SelectSingleNode("name").InnerText = Player1.Name;
            player1.SelectSingleNode("score").InnerText = Player1.Score.ToString();
            player1.SelectSingleNode("time").InnerText = Player1.Time.ToString();

            player2.SelectSingleNode("name").InnerText = Player2.Name;
            player2.SelectSingleNode("score").InnerText = Player2.Score.ToString();
            player2.SelectSingleNode("time").InnerText = Player2.Time.ToString();

            List<XmlNode> cards = new List<XmlNode>();

            foreach (Card card in CardCollection)
            {
                XmlNode cardNode = xmlDoc.CreateNode(XmlNodeType.Element, "card", null);

                List<XmlNode> cardChildNodes = new List<XmlNode>();

                XmlNode atMove = xmlDoc.CreateNode(XmlNodeType.Element, "atmove", null);
                XmlNode back = xmlDoc.CreateNode(XmlNodeType.Element, "back", null);
                XmlNode column = xmlDoc.CreateNode(XmlNodeType.Element, "column", null);
                XmlNode front = xmlDoc.CreateNode(XmlNodeType.Element, "front", null);
                XmlNode isTurned = xmlDoc.CreateNode(XmlNodeType.Element, "isturned", null);
                XmlNode row = xmlDoc.CreateNode(XmlNodeType.Element, "row", null);
                
                cardChildNodes.Add(atMove);
                cardChildNodes.Add(back);
                cardChildNodes.Add(column);
                cardChildNodes.Add(front);
                cardChildNodes.Add(isTurned);
                cardChildNodes.Add(row);

                atMove.InnerText = card.AtMove.ToString();
                back.InnerText = (card.Back as BitmapImage).UriSource.OriginalString;
                column.InnerText = card.Column.ToString();
                front.InnerText = (card.Front as BitmapImage).UriSource.OriginalString;
                isTurned.InnerText = card.IsTurned.ToString();
                row.InnerText = card.Row.ToString();

                foreach (XmlNode cardChild in cardChildNodes)
                    cardNode.AppendChild(cardChild);

                cardCollection.AppendChild(cardNode);
            }

            xmlDoc.Save("game.sav");
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
