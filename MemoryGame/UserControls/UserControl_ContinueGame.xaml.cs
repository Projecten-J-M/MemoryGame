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
    /// Interaction logic for ContinueGame.xaml
    /// </summary>
    public partial class ContinueGame : UserControl
    {
        public ContinueGame()
        {
            InitializeComponent();
        }

        
        /// <summary>
        /// Returns to main menu
        /// Written by: Mark Hooijberg
        /// Implemented by: Duncan Dreize
        /// </summary>
        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            Content = new UserControl_MainMenu();
        }

        /// <summary>
        /// Continues the last saved game.
        /// </summary>
        private void Btn_continue_click(object sender, RoutedEventArgs e)
        {
            Game game = new Game(new GameConfig());
            game.LoadGameFromFile();

            Content = new UserControl_GameField(game);
        }

        /// <summary>
        /// Assigns a new game configuration screen to the content.
        /// Written by: Mark Hooijberg
        /// Implemented by: Duncan Dreize
        /// </summary>
        private void Btn_newgame_click(object sender, RoutedEventArgs e)
        {
            Content = new UserControl_NameInput();
        }
    }
}
