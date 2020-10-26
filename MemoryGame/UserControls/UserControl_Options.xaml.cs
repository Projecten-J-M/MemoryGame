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
using System.Xml;

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

        /// <summary>
        /// Return to main menu,
        /// Written by: Mark Hooijberg
        /// Implemented by: Jur Stedehouder
        /// </summary>
        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            Content = new UserControl_MainMenu();
        }

        /// <summary>
        /// Calculates the volume percentage to update the volume percentage label and updates the volume.
        /// By: Mark Hooijberg.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void slider_volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lbl_percentage.Content = Math.Round((e.NewValue * 100), 0) + "%";

            MainWindow.SetBackgroundVolume(Math.Round(e.NewValue, 2));
        }

        /// <summary>
        /// Set the current volume value on the slider after its been loaded.
        /// By: Mark Hooijberg.
        /// </summary>
        private void slider_volume_Loaded(object sender, RoutedEventArgs e)
        {
            slider_volume.Value = MainWindow.mediaPlayer.Volume;
        }

        /// <summary>
        /// Show a new dialogwindow with the credits of the game makers.
        /// By: Mark Hooijberg
        /// </summary>
        private void btn_credits_Click(object sender, RoutedEventArgs e)
        {
            Window creditsWindow = new Windows.CreditsWindow();
            creditsWindow.ShowDialog();
        }

        /// <summary>
        /// Resets the highscore list
        /// Made by: Duncan Dreize & Mark Hooijberg
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("game.sav");
            xmlDoc.SelectSingleNode("//highscores").InnerText = null;
            lbl_reset.Visibility = Visibility.Visible;

            xmlDoc.Save("game.sav");
        }
    }
}
