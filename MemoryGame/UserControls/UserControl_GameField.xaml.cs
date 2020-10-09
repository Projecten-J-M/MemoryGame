using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MemoryGame.Classes;

namespace MemoryGame.UserControls
{
    /// <summary>
    /// Interaction logic for UserControl_GameField.xaml
    /// </summary>
    public partial class UserControl_GameField : UserControl
    {
        private Game _game;
        public UserControl_GameField(Game game)
        {
            _game = game;
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            
            timer.Interval = new TimeSpan(0, 0, 1); 
            timer.Tick += dtClockTime_Tick;

            laatste_tijd = DateTime.Now;
            timer.Start();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += KeyPressHandler;

            SetupPlayfield(_game.Config.FieldHeight, _game.Config.FieldWidth);
            SetNames(new string[] { _game.Config.PlayerName1, _game.Config.PlayerName2});
        }

        private void SetNames(string[] names)
        {
            lbl_player1Name.Content = names[0];
            lbl_player2Name.Content = names[1];
        }

        private void SetupPlayfield(int height, int width)
        {
            for (int i = 0; i < height; i++)
            {
                grd_cardGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(75) });
            }

            for (int i = 0; i < width; i++)
            {
                grd_cardGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(75) });
            }

            grd_cardGrid.ShowGridLines = true;
        }


        // Controls what code 
        private void KeyPressHandler(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Escape:
                    TogglePauseMenu();
                    break;

                case Key.W:
                    KeyPressW();
                    break;
            }
        }
        private void TogglePauseMenu()
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
        private void KeyPressW()
        {
            Content = new UserControl_EndScreen();
        }

        #region PauseMenuButtonEvents
        // Knoppen om terug naar het hoofdmenu te gaan.
        private void Btn_Quit_Click(object sender, RoutedEventArgs e)
        {
            Content = new UserControl_MainMenu();
        }
        // Hieronder geschreven door Jur Stedehouder
        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            // TO DO: Put code to save game here.
            Content = new UserControl_MainMenu();
        }

        private void Btn_Continue_Click(object sender, RoutedEventArgs e) => TogglePauseMenu();
        #endregion

        //Code hieronder geschreven door Jur Stedehouder

        private TimeSpan tijd = new TimeSpan(0);
        private DateTime laatste_tijd = DateTime.MinValue;

        
        private void dtClockTime_Tick(object sender, EventArgs e)
        {
            var volgende_tijd = DateTime.Now;
            tijd = tijd.Add(DateTime.Now - laatste_tijd);
            laatste_tijd = volgende_tijd;
            klok.Content = tijd.ToString(@"mm\:ss");

        }

    }
}
