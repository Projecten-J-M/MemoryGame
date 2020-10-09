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

        

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            Content = new UserControl_MainMenu();
        }

        private void Btn_continue_click(object sender, RoutedEventArgs e)
        {
            //TODO: Continue game
            
        }

        private void Btn_newgame_click(object sender, RoutedEventArgs e)
        {
            Content = new UserControl_NameInput();
        }
    }
}
