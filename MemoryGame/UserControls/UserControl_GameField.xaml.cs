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
        private DispatcherTimer timer;

        public TimeSpan time;

        public UserControl_GameField(Game game)
        {
            InitializeComponent();
            _game = game;

            time = new TimeSpan(0, 0, 0);
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000); 
            timer.Tick += dtClockTime_Tick;
            timer.Start();

            lbl_player1Score.Content = _game.Player1.Score;
            lbl_player1Score.Content = _game.Player2.Score;

            //TODO: per player timer.s
        }

        #region UserControl Functions
        /// <summary>
        /// Toggles pause menu visibility and timer.
        /// By Mark Hooijberg
        /// </summary>
        private void TogglePauseMenu()
        {
            if (grd_pauseMenu.Visibility == Visibility.Hidden)
            {
                // Pause timer etc.
                timer.Stop();
                grd_pauseMenu.Visibility = Visibility.Visible;
            }

            else
            {
                // Resume timer etc.
                timer.Start();
                grd_pauseMenu.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Assigns names to the player name labels.
        /// By Mark Hooijberg
        /// </summary>
        /// <param name="names">Sequence of names to be set.</param>
        private void SetNames(string[] names)
        {
            lbl_player1Name.Content = names[0];
            lbl_player2Name.Content = names[1];
        }

        /// <summary>
        /// Create a grid for the cards to be loaded in with a given size.
        /// By Mark Hooijberg.
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        private void SetupPlayfield(int rows, int columns)
        {
            for (int i = 0; i < rows; i++)
                grd_cardGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(grd_cardGrid.Height / rows) });

            for (int i = 0; i < columns; i++)
                grd_cardGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(grd_cardGrid.Width / columns)});
        }
        
        /// <summary>
        /// Fills the card grid with elements.
        /// </summary>
        private void FillPlayField()
        {
            for (int row = 0; row < grd_cardGrid.RowDefinitions.Count; row++)
            {
                for (int column = 0; column < grd_cardGrid.ColumnDefinitions.Count; column++)
                {
                    Image card = new Image();

                    card.MouseDown += Card_MouseDown;
                    card.Source = new BitmapImage(new Uri("\\mempic.png", UriKind.Relative));
                    card.Tag = new BitmapImage(new Uri("\\mempic2.png", UriKind.Relative));
                    card.Stretch = Stretch.Fill;
                    Grid.SetRow(card, row);
                    Grid.SetColumn(card, column);
                    grd_cardGrid.Children.Add(card);

                }
            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Configures the PlayingField.
        /// By: Mark Hooijberg.
        /// </summary>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += KeyPressHandler;

            SetupPlayfield(_game.Config.FieldHeight, _game.Config.FieldWidth);
            FillPlayField();
            SetNames(new string[] { _game.Config.PlayerName1, _game.Config.PlayerName2});
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image card = (Image)sender;
            card.Source = (BitmapImage)card.Tag;

            _game.Player1.Score = 0;
            _game.Player1.Score = 0;
        }

        /// <summary>
        /// Fired when clicked on Continue in pause menu. Closes the pause menu.
        /// By: Jur Stedehouder
        /// Enhanced by: Mark Hooijberg
        /// </summary>
        private void Btn_Continue_Click(object sender, RoutedEventArgs e) => TogglePauseMenu();

        /// <summary>
        /// Return to main menu.
        /// Written and enhanced by: Mark Hooijberg
        /// Implemented by: Jur Stedehouder
        /// </summary>
        private void Btn_Quit_Click(object sender, RoutedEventArgs e) => Content = new UserControl_MainMenu();

        /// <summary>
        /// Save the state of the game and return to main menu.
        /// By: Jur Stedehouder
        /// </summary>
        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            // TO DO: Put code to save game here.
            Content = new UserControl_MainMenu();
        }

        /// <summary>
        /// Dynamically changes the size of the card grid.
        /// By: Mark Hooijberg
        /// </summary>
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            grd_cardGrid.Height = e.NewSize.Height - 150;
            grd_cardGrid.Width = grd_cardGrid.Height;

            foreach (RowDefinition rowDefinition in grd_cardGrid.RowDefinitions)
                rowDefinition.Height = new GridLength(grd_cardGrid.Height / grd_cardGrid.RowDefinitions.Count());
            
            foreach (ColumnDefinition columnDefinition in grd_cardGrid.ColumnDefinitions)
                columnDefinition.Width = new GridLength(grd_cardGrid.Width / grd_cardGrid.ColumnDefinitions.Count());
        }

        /// <summary>
        /// Updates adds a second to the time and time lable.
        /// Originally witten by: Jur Stedehouder
        /// Enhanced by: Mark Hooijberg
        /// </summary>
        private void dtClockTime_Tick(object sender, EventArgs e)
        {
            time += new TimeSpan(0, 0, 1);
            klok.Content = time.Duration().ToString(@"mm\:ss");
        }

        /// <summary>
        /// Executes different functions based on the key which is pressed
        /// By: Mark Hooijberg
        /// /// </summary>
        /// <param name="e">Arguments collected by the event.</param>
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

        /// <summary>
        /// Save score in the game object and overwrites the content with a new End Screen object.
        /// This is to simulate a game ending.
        /// By: Mark Hooijberg
        /// </summary>
        private void KeyPressW()
        {
            _game.Player1.Score = Convert.ToInt32(lbl_player1Score.Content.ToString().Split(' ')[1]);
            _game.Player2.Score = Convert.ToInt32(lbl_player2Score.Content.ToString().Split(' ')[1]);
            Content = new UserControl_EndScreen(_game);
        }
        #endregion
    }
}
