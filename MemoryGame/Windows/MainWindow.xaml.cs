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

namespace MemoryGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Start volume
        public static MediaPlayer mediaPlayer;
        public double mediaPlayer_Volume = 0.25;

        public MainWindow()
        {
            InitializeComponent();
            Content = new UserControls.UserControl_MainMenu();

            mediaPlayer = new MediaPlayer();
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            SetBackgroundVolume(mediaPlayer_Volume);
            mediaPlayer.Open(new Uri("Background music.mp3", UriKind.Relative));
            mediaPlayer.Play();
        }

        /// <summary>
        /// Loops the background music.
        /// By: Niels Essink
        /// </summary>
        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            mediaPlayer.Position = TimeSpan.Zero;
            mediaPlayer.Play();
        }

        public static void SetBackgroundVolume(double volume)
        {
            mediaPlayer.Volume = volume;
        }
    }
}
