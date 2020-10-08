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

        private void Btn_Play_Click(object sender, RoutedEventArgs e)
        {
            Content = new UserControl_NameInput();
        }

        private void Btn_Highscores_Click(object sender, RoutedEventArgs e)
        {
            Content = new UserControl_Highscores();
        }

        private void Btn_Options_Click(object sender, RoutedEventArgs e)
        {
            Content = new UserControl_Options();
        }

        private void Btn_Quit_Click(object sender, RoutedEventArgs e)
        {
            // Close the application.
            Application.Current.Shutdown();
        }
    }
}
