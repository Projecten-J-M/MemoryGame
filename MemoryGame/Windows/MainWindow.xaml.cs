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
            SetBackgroundVolume(mediaPlayer_Volume);
            mediaPlayer.Open(new Uri(@"C:\Users\Admin\Source\Repos\MemoryGame\MemoryGame\Background_Music.wav"));
            mediaPlayer.Play();
        }

        public static void SetBackgroundVolume(double volume)
        {
            mediaPlayer.Volume = volume;
        }
    }
}
