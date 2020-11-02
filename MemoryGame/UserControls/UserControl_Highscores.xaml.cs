using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Resolvers;

namespace MemoryGame.UserControls
{
    /// <summary>
    /// Interaction logic for UserControl_Highscores.xaml
    /// </summary>
    public partial class UserControl_Highscores : UserControl
    {
        public UserControl_Highscores()
        {
            InitializeComponent();
            LoadHighScores();

            //List<Info_player> Info = new List<Info_player>();
            //Info.Add(new Info_player()
            //{
            //    Name = "Bob",
            //    Score = 101,
            //    Time = 50,
                //});

                //Info.Add(new Info_player()
                //{
                //    Name = "Frank",
                //    Score = 90,
                //    Time = 70,
                //});

                //Highscore.ItemsSource = Info;


            }

        /// <summary>
        /// Loads the highscores from game.sav into the Datagrid.
        /// Made by: Duncan Dreize, Peter Jongman & Mark Hooijberg
        /// </summary>
        private void LoadHighScores()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("game.sav");
            XmlNode Highscores = xmlDoc.DocumentElement.GetElementsByTagName("highscores")[0];
            List<Info_player> Info = new List<Info_player>();
            foreach (XmlNode node in xmlDoc.SelectNodes("//highscores/player"))
            {

                string name = node["name"].InnerText.ToString();
                int score = Convert.ToInt32(node["score"].InnerText);

                Info.Add(new Info_player()
                {
                    Name = name,
                    Score = score,
                    Time = 0
                });
            }
            Highscore.ItemsSource = Info;
        }

        

        /// <summary>
        /// Returns to main menu.
        /// By: Mark Hooijberg.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            Content = new UserControl_MainMenu();
        }
        /// <summary>
        /// Gets the info of a certain player then puts it into a table on the screen
        /// </summary>
        public class Info_player
        {

            public string Name { get; set; }

            public int Score { get; set; }

            public int Time { get; set; }
        }
        

        private void Highscore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
