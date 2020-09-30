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
    /// Interaction logic for UserControl_GameField.xaml
    /// </summary>
    public partial class UserControl_GameField : UserControl
    {
        public UserControl_GameField()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += KeyPressHandler;
        }

        private void KeyPressHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {

                if (grd_pauseMenu.Visibility == Visibility.Hidden)
                {
                    // Pause timer etc.
                    grd_pauseMenu.Visibility = Visibility.Visible;
                }

                else
                {
                    // Resume timer etc.
                    grd_pauseMenu.Visibility = Visibility.Hidden;

                }
            }
        }
    }
}
