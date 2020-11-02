using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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

namespace MemoryGame.UserControls
{
    /// <summary>
    /// Interaction logic for UserControl_MainMenu.xaml
    /// </summary>
    public partial class UserControl_MainMenu : UserControl
    {
        public UserControl_MainMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Assigns a new ContinueGame usercontrol object to the content.
        /// Created by: Mark Hooijberg.
        /// Updated by: Duncan Dreize.
        /// </summary>
        private void Btn_Play_Click(object sender, RoutedEventArgs e) => Content = new ContinueGame();

        /// <summary>
        /// Assigns a new Highscores usercontrol object to the content.
        /// Created by: Mark Hooijberg.
        /// </summary>
        private void Btn_Highscores_Click(object sender, RoutedEventArgs e) => Content = new UserControl_Highscores();

        /// <summary>
        /// Assigns a new Options usercontrol object to the content.
        /// Created by: Mark Hooijberg.
        /// </summary>
        private void Btn_Options_Click(object sender, RoutedEventArgs e) => Content = new UserControl_Options();

        /// <summary>
        /// Closes the application.
        /// Created by: Mark Hooijberg.
        /// </summary>
        private void Btn_Quit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    }
}
