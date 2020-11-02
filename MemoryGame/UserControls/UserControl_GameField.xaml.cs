using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
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
        // Private global variables:
        private int BaseScore = 100;
        private bool canClick = true;
        private Card firstCard;
        private Card lastCard;
        private Game game;
        private DispatcherTimer timer;
        private DispatcherTimer viewCardTimer;

        public UserControl_GameField(Game _game)
        {
            InitializeComponent();

            game = _game;

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000); 
            timer.Tick += dtClockTime_Tick;
            timer.Start();

            viewCardTimer = new DispatcherTimer();
            viewCardTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            viewCardTimer.Tick += ViewCardTimer_Tick;

            SetActiveColors();
        }

        /// <summary>
        /// Game logic to deal with the selected cards after the user viewed them,
        /// Including: turning / removing cards, adding points, stopping the game and switch player turn.
        /// </summary>
        private void ViewCardTimer_Tick(object sender, EventArgs e)
        {
            viewCardTimer.Stop();           

            if ((firstCard.Front as BitmapImage).UriSource == (lastCard.Front as BitmapImage).UriSource)
            {
                int AddedScore = 0;
                if (TimeSpan.Parse("00:" + game.GetActivePlayer().Time.Duration().ToString()).TotalSeconds > 30)
                    AddedScore -= 90;

                else if (TimeSpan.Parse("00:" + game.GetActivePlayer().Time.Duration().ToString()).TotalSeconds > 10)
                    AddedScore -= 50;

                Player activePlayer = game.GetActivePlayer();
                game.GetActivePlayer().Score += BaseScore + AddedScore;

                if (activePlayer == game.Player1)
                    lbl_player1Score.Content = activePlayer.Score;
                else
                    lbl_player2Score.Content = activePlayer.Score;

                grd_cardGrid.Children.Remove(firstCard.Image);
                grd_cardGrid.Children.Remove(lastCard.Image);

                if (grd_cardGrid.Children.Count == 0)
                {
                    MainWindow.PlayMusic(new Uri("MenuMusic.mp3", UriKind.Relative));
                    Content = new UserControl_EndScreen(game);
                }
            }
            else
            {
                firstCard.AtMove = null;
                lastCard.AtMove = null;
                firstCard.IsTurned = false;
                lastCard.IsTurned = false;

                firstCard.Image.Source = firstCard.Back;
                lastCard.Image.Source = lastCard.Back;
            }
            lastCard = null;
            game.SwitchTurn();
            SetActiveColors();
            canClick = true;
        }

        #region UserControl Functions
        /// <summary>
        /// Toggles pause menu visibility and timer.
        /// Created by: Mark Hooijberg.
        /// </summary>
        private void TogglePauseMenu()
        {
            if (grd_pauseMenu.Visibility == Visibility.Hidden)
            {
                timer.Stop();
                viewCardTimer.Stop();
                grd_pauseMenu.Visibility = Visibility.Visible;
            }

            else
            {
                timer.Start();
                if (lastCard != null & firstCard != null)
                    viewCardTimer.Start();
                grd_pauseMenu.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Changes the label color of the player info,
        /// according to which player is at turn.
        /// </summary>
        private void SetActiveColors()
        {
            if (game.Turn == Game.PlayerTurn.Player1)
            {
                lbl_player2Name.Foreground = new SolidColorBrush(Colors.DarkCyan);
                lbl_player1Name.Foreground = new SolidColorBrush(Colors.Aqua);
                lbl_player2Score.Foreground = new SolidColorBrush(Colors.DarkCyan);
                lbl_player1Score.Foreground = new SolidColorBrush(Colors.Aqua);
                lbl_player1Time.Foreground = new SolidColorBrush(Colors.Aqua);
                lbl_player2Time.Foreground = new SolidColorBrush(Colors.DarkCyan);
            }
            else
            {
                lbl_player1Name.Foreground = new SolidColorBrush(Colors.DarkCyan);
                lbl_player2Name.Foreground = new SolidColorBrush(Colors.Aqua);
                lbl_player1Score.Foreground = new SolidColorBrush(Colors.DarkCyan);
                lbl_player2Score.Foreground = new SolidColorBrush(Colors.Aqua);
                lbl_player2Time.Foreground = new SolidColorBrush(Colors.Aqua);
                lbl_player1Time.Foreground = new SolidColorBrush(Colors.DarkCyan);
            }
        }

        /// <summary>
        /// Assigns names to the player name labels.
        /// Created by: Mark Hooijberg.
        /// </summary>
        /// <param name="playerNames">Sequence of player names.</param>
        private void SetNames(string[] playerNames)
        {
            lbl_player1Name.Content = playerNames[0];
            lbl_player2Name.Content = playerNames[1];
        }

        /// <summary>
        /// Create row and column definitions in the card grid.
        /// Created by: Mark Hooijberg.
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
        /// Returns a list of random images according to the current theme.
        /// </summary>
        private List<ImageSource> GetImagesList()
        {
            int cardAmount = game.Config.FieldHeight * game.Config.FieldWidth;
            string currentDirectory = Directory.GetCurrentDirectory();
            string[] files = Directory.GetFiles(currentDirectory + "\\" + game.Thema);
            List<ImageSource> images = new List<ImageSource>();

            for (int i = 0; i < cardAmount; i++)
            {
                int imageNr = i % (cardAmount / 2);
                ImageSource source = new BitmapImage(new Uri(files[imageNr], UriKind.Absolute));
                images.Add(source);
            }
            return images;
        }

        /// <summary>
        /// Create Images and fill the card grid.
        /// Created by: Niels Essink & Mark Hooijberg.
        /// </summary>
        private void FillPlayField()
        {
            if (game.CardCollection.Count == 0)
            {
                Random randomNumberGenerator = new Random();
                List<ImageSource> images = GetImagesList();

                for (int row = 0; row < grd_cardGrid.RowDefinitions.Count; row++)
                {
                    for (int column = 0; column < grd_cardGrid.ColumnDefinitions.Count; column++)
                    {
                        int randomNumber = randomNumberGenerator.Next(0, images.Count);
                        Image image = new Image()
                        {
                            Stretch = Stretch.Fill,
                            Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\" + game.Thema + "Back.png", UriKind.Absolute)),
                            Tag = new int[] { row, column }
                        };

                        Grid.SetRow(image, row);
                        Grid.SetColumn(image, column);

                        image.MouseDown += Card_MouseDown;

                        Card card = new Card()
                        {
                            Back = image.Source,
                            Column = column,
                            Front = images[randomNumber],
                            Image = image,
                            IsTurned = false,
                            Row = row
                        };

                        images.RemoveAt(randomNumber);
                        grd_cardGrid.Children.Add(image);
                        game.CardCollection.Add(card);
                    }
                }
            }
            else
            {
                List<Card> loadedCards = new List<Card>();
                loadedCards = game.CardCollection.Where(x => x.IsTurned == false).ToList();

                if (game.CardCollection.Where(x => x.IsTurned == true).Count() % 2 > 0)
                    loadedCards.Add(game.CardCollection.Where(x => x.AtMove != null).OrderBy(x => x.AtMove).ToList().Last());

                foreach (Card card in loadedCards)
                {
                    Image image = new Image()
                    {
                        Stretch = Stretch.Fill,
                        Source = card.Back,
                        Tag = new int[] { card.Row, card.Column }
                    };

                    if (card.IsTurned == true)
                    {
                        image.Source = card.Front;
                        lastCard = card;
                    }

                    Grid.SetRow(image, card.Row);
                    Grid.SetColumn(image, card.Column);

                    image.MouseDown += Card_MouseDown;

                    card.Image = image;
                    grd_cardGrid.Children.Add(image);
                }

            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Setup the playfield, play music and add a KeyPressHandler.
        /// Created by: Mark Hooijberg.
        /// </summary>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += KeyPressHandler;

            MainWindow.PlayMusic(new Uri(game.Thema + "Music.mp3", UriKind.Relative));
            SetupPlayfield(game.Config.FieldHeight, game.Config.FieldWidth);
            FillPlayField();

            SetNames(new string[] { game.Player1.Name, game.Player2.Name});

            lbl_player1Score.Content = game.Player1.Score;
            lbl_player1Time.Content = game.Player1.Time.Duration().ToString(@"mm\:ss");

            lbl_player2Score.Content = game.Player2.Score;
            lbl_player2Time.Content = game.Player2.Time.Duration().ToString(@"mm\:ss");
        }

        /// <summary>
        /// Load clicked card and pass this to the card validation function.
        /// Created by: Jur Stedehouder, Peter Jongman
        /// </summary>
        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (canClick == true)
            {
                Image image = (Image)sender;
                firstCard = game.GetCard(image);

                firstCard.SetAtMove(game);
                firstCard.IsTurned = true;

                image.Source = firstCard.Front;
                CheckCardSimple(firstCard);
            }
        }

        /// <summary>
        /// Checks if the second last and last card clicked are not the same and view them.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="card"></param>
        private void CheckCardSimple(Card card)
        {
            if (lastCard != null && lastCard != card)
            {
                canClick = false;
                viewCardTimer.Start();                
            }

            else
                lastCard = card;
        }

        /// <summary>
        /// Closes the pause menu.
        /// Created by: Jur Stedehouder.
        /// </summary>
        private void Btn_Continue_Click(object sender, RoutedEventArgs e) => TogglePauseMenu();

        /// <summary>
        /// Assigns a new Mainmenu usercontrol object to the content.
        /// Created by: Jur Stedehouder.
        /// </summary>
        private void Btn_Quit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.PlayMusic(new Uri("MenuMusic.mp3", UriKind.Relative));
            Content = new UserControl_MainMenu();
        }

        /// <summary>
        /// Save the state of the game and assigns a new Mainmenu usercontrol object to the content.
        /// Created by: Jur Stedehouder
        /// </summary>
        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            game.Save();
            MainWindow.PlayMusic(new Uri("MenuMusic.mp3", UriKind.Relative));
            Content = new UserControl_MainMenu();
        }

        /// <summary>
        /// Dynamically changes the size of the card grid.
        /// Created by: Mark Hooijberg
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
        /// Adds a second to the player time and the player time label.
        /// Created by: Jur Stedehouder
        /// Modified by: Duncan Dreize, added a second timer and keeps track of time per player.
        /// </summary>
        private void dtClockTime_Tick(object sender, EventArgs e)
        {
            game.GetActivePlayer().Time += new TimeSpan(0, 0, 1);

            if (game.Turn == Game.PlayerTurn.Player1)
                lbl_player1Time.Content = game.GetActivePlayer().Time.Duration().ToString(@"mm\:ss");

            else
                lbl_player2Time.Content = game.GetActivePlayer().Time.Duration().ToString(@"mm\:ss");
        }

        /// <summary>
        /// Executes different functions based on the key which is pressed
        /// Created by: Mark Hooijberg
        /// </summary>
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
        /// Save score in the game object and assigns a new endscreen usercontrol object to the content.
        /// (This is to simulate a game ending.)
        /// Created by: Mark Hooijberg
        /// </summary>
        private void KeyPressW()
        {
            MainWindow.PlayMusic(new Uri("MenuMusic.mp3", UriKind.Relative));
            Content = new UserControl_EndScreen(this.game);
        }
        #endregion
    }
}