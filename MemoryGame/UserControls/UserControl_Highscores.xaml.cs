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
using MemoryGame.Classes;

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
        }

        /// <summary>
        /// Retrieve the saved highscores and display them onto the datagrid.
        /// Created by: Duncan Dreize, Peter Jongman & Mark Hooijberg
        /// </summary>
        private void LoadHighScores()
        {
            List<Highscore> highscores = new List<Highscore>();
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load("game.sav");

            foreach (XmlNode node in xmlDoc.SelectNodes("//highscores/player"))
            {
                highscores.Add(new Highscore()
                {
                    Name = node["name"].InnerText.ToString(),
                    Score = Convert.ToInt32(node["score"].InnerText)
                });
            }

            Highscore.ItemsSource = highscores;
        }

        /// <summary>
        /// Assigns a new Mainmenu usercontrol object to the content.
        /// Created by: Mark Hooijberg.
        /// </summary>
        private void Btn_Back_Click(object sender, RoutedEventArgs e) => Content = new UserControl_MainMenu();
    }
}
