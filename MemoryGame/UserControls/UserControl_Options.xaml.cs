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
using MemoryGame;

namespace MemoryGame.UserControls
{
    /// <summary>
    /// Interaction logic for UserControl_Options.xaml
    /// </summary>
    public partial class UserControl_Options : UserControl
    {
        public UserControl_Options()
        {
            InitializeComponent();
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            Content = new UserControl_MainMenu();
        }

        private void slider_volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lbl_percentage.Content = Math.Round((e.NewValue * 100), 0) + "%";

            MainWindow.SetBackgroundVolume(Math.Round(e.NewValue, 2));
        }

        private void slider_volume_Loaded(object sender, RoutedEventArgs e)
        {
            slider_volume.Value = MainWindow.mediaPlayer.Volume;
        }
    }
}
