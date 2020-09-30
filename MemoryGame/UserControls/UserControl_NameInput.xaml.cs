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
    /// Interaction logic for UserControl_NameInput.xaml
    /// </summary>
    public partial class UserControl_NameInput : UserControl
    {
        public UserControl_NameInput()
        {
            InitializeComponent();
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            Content = new UserControl_MainMenu();
        }

        private void Btn_Continue_Click(object sender, RoutedEventArgs e)
        {
            Content = new UserControl_GameField();
        }
    }
}
