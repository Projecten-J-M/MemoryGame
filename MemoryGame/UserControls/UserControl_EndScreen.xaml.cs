using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using MemoryGame.Classes;

namespace MemoryGame.UserControls
{
    /// <summary>
    /// Interaction logic for UserControl_EndScreen.xaml
    /// </summary>
    public partial class UserControl_EndScreen : UserControl
    {
        private Game game;

        public UserControl_EndScreen(Game _game)
        {
            InitializeComponent();

            game = _game;

            ShowScore(new string[] { game.Player1.Name, game.Player2.Name});
            SaveScore();
        }
        /// <summary>
        /// Start a new game with the current configuration.
        /// Created by: Duncan Dreize.
        /// </summary>
        private void Btn_Play_Click(object sender, RoutedEventArgs e) => Content = new UserControl_GameField(new Game(game.Config));

        /// <summary>
        /// Assigns a new Mainmenu usercontrol object to the content.
        /// Created by: Duncan Dreize.
        /// </summary>
        private void Btn_MainMenu_Click(object sender, RoutedEventArgs e) => Content = new UserControl_MainMenu();

        /// <summary>
        /// Writes the winning player's score on a text file so it can be used for showing the Highscores.
        /// Also shows the players at the end who won and with which amount of points.
        /// Created by: Duncan Dreize.
        /// </summary>
        /// <param name="names">Player Names</param>
        private void ShowScore(string[] names)
        {
            if (game.Player1.Score > game.Player2.Score)
            {
                lbl_winner_select.Content = names[0] + " has won with " + game.Player1.Score + " points!";
                lbl_loser_select.Content = names[1] + " has lost with " + game.Player2.Score + " points!";
            }
            else if (game.Player1.Score < game.Player2.Score)
            { 
                lbl_winner_select.Content = names[1] + " has won with " + game.Player2.Score + " points!";
                lbl_loser_select.Content = names[0] + " has lost with " + game.Player1.Score + " points!";
            }
            else if (game.Player1.Score == game.Player2.Score)
                lbl_tie_select.Content = names[0] + " and " + names[1] + " have tied with " + game.Player1.Score + " points!";

            else
                lbl_winner_select.Content = "Something went wrong!";
        }

        /// <summary>
        /// Saves the score in the highscore list of the save file.
        /// Created by: Duncan Dreize and Mark Hooijberg
        /// </summary>
        private void SaveScore()
        {
            Player player = new Player();

            if (game.Player1.Score > game.Player2.Score)
                player = game.Player1;

            else if (game.Player1.Score < game.Player2.Score)
                player = game.Player2;

            else if (game.Player1.Score == game.Player2.Score)
            {
                player.Name = $"{game.Player1.Name} and {game.Player2.Name}";
                player.Score = game.Player1.Score;
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("game.sav");
            XmlNode root = xmlDoc.DocumentElement;

            XmlElement newHighscore = xmlDoc.CreateElement("player");
            XmlElement name = xmlDoc.CreateElement("name");
            name.InnerText = player.Name;
            XmlElement score = xmlDoc.CreateElement("score");
            score.InnerText = player.Score.ToString();

            newHighscore.AppendChild(name);
            newHighscore.AppendChild(score);

            root.SelectSingleNode("//highscores").AppendChild(newHighscore);

            xmlDoc.Save("game.sav");
        }
    }
}
