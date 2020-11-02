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
        // Public variables:

        /// <summary>
        /// Public variable to change the volume in a save way.
        /// Created by: Mark Hooijberg
        /// </summary>
        public static double MediaPlayerVolume { 
            get { return mediaPlayer.Volume; }
            set { mediaPlayer.Volume = value; }
        }
        
        // Private variables:
        private static MediaPlayer mediaPlayer;
        private double Volume = 0.25;

        public MainWindow()
        {
            InitializeComponent();
            Content = new UserControls.UserControl_MainMenu();

            mediaPlayer = new MediaPlayer();
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            MediaPlayerVolume = Volume;
            mediaPlayer.Open(new Uri("MenuMusic.mp3", UriKind.Relative));
            mediaPlayer.Play();
        }

        /// <summary>
        /// Loops the background music.
        /// Created by: Niels Essink
        /// </summary>
        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            mediaPlayer.Position = TimeSpan.Zero;
            mediaPlayer.Play();
        }

        /// <summary>
        /// Loads a audio file from an URI and plays it.
        /// Created by: Mark Hooijberg
        /// </summary>
        /// <param name="uri">The Uri of the audio file to be played.</param>
        public static void PlayMusic(Uri uri)
        {
            mediaPlayer.Open(uri);
            mediaPlayer.Play();
        }

    }
}
