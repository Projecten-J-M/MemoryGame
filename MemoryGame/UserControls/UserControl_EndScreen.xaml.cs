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
    /// Interaction logic for UserControl_EndScreen.xaml
    /// </summary>
    public partial class UserControl_EndScreen : UserControl
    {
        private Game _game;
        public UserControl_EndScreen(Game game)
        {
            InitializeComponent();
            _game = game;
        }

        /// <summary>
        /// Assigns a new game configuration screen to the content.
        /// Written by: Mark Hooijberg
        /// Implemented by: TODO: figure out who implemented the functionality.
        /// </summary>
        private void Btn_Play_Click(object sender, RoutedEventArgs e)
        {
            Content = new UserControl_NameInput();
        }

        /// <summary>
        /// Returns to main menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_MainMenu_Click(object sender, RoutedEventArgs e)
        {
            Content = new UserControl_MainMenu();
        }
    }
}
