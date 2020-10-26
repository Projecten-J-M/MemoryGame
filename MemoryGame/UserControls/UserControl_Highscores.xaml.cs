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


            List<Info_player> Info = new List<Info_player>();
            Info.Add(new Info_player()
            {
                Name = "Bob",
                Score = 101,
                Time = 50,
            });

            Info.Add(new Info_player()
            {
                Name = "Frank",
                Score = 90,
                Time = 70,
            });

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

        public class Info_player
        {

            public string Name { get; set; }

            public int Score { get; set; }

            public int Time { get; set; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Highscore.ItemsSource = null;
        }

        private void Highscore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
