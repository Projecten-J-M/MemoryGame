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
using MemoryGame.Classes;

namespace MemoryGame.UserControls
{
    /// <summary>
    /// Interaction logic for UserControl_NameInput.xaml
    /// </summary>
    public partial class UserControl_NameInput : UserControl
    {
        public UserControl_NameInput()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Assigns a new ContinueGame usercontrol object to the content.
        /// Created by: Duncan Dreize.
        /// </summary>
        private void Btn_Back_Click(object sender, RoutedEventArgs e) => Content = new ContinueGame();

        /// <summary>
        /// Loads the given input into a configuration and start a new game.
        /// Created by: Mark Hooijberg.
        /// </summary>
        private void Btn_Continue_Click(object sender, RoutedEventArgs e)
        {
            int size = rbtn_difficultyNormal.IsChecked == true ? 4 : 6;

            Game game = new Game(new GameConfig()
            {
                FieldHeight = size,
                FieldWidth = size,
                PlayerName1 = tbx_player1.Text,
                PlayerName2 = tbx_player2.Text,
                startScore = 100,
                StartPlayer = Game.PlayerTurn.Player1,
                Thema = (string) cbbx_thema.SelectedItem
            });

            Content = new UserControl_GameField(game);
        }
    }
}
