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

        private void Btn_Play_Click(object sender, RoutedEventArgs e)
        {
            Content = new UserControl_NameInput();
        }

        private void Btn_MainMenu_Click(object sender, RoutedEventArgs e)
        {
            Content = new UserControl_MainMenu();
        }
    }
}
