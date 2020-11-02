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
        private bool canClick = true;
        private Card firstCard;
        private Card lastCard;
        private Game game;
        private DispatcherTimer timer;
        private DispatcherTimer viewCardTimer;

        public TimeSpan time;

        public UserControl_GameField(Game _game)
        {
            game = null;
            InitializeComponent();
            game = _game;

            time = new TimeSpan(0, 0, 0);
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000); 
            timer.Tick += dtClockTime_Tick;
            timer.Start();

            viewCardTimer = new DispatcherTimer();
            viewCardTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            viewCardTimer.Tick += ViewCardTimer_Tick;



            SetActiveColors();
        }

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
        /// By Mark Hooijberg
        /// </summary>
        private void TogglePauseMenu()
        {
            if (grd_pauseMenu.Visibility == Visibility.Hidden)
            {
                // Pause timer etc.
                timer.Stop();
                viewCardTimer.Stop();
                grd_pauseMenu.Visibility = Visibility.Visible;
            }

            else
            {
                // Resume timer etc.
                timer.Start();
                if (lastCard != null & firstCard != null)
                    viewCardTimer.Start();
                grd_pauseMenu.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Toggles pause menu visibility and timer.
        /// By Mark Hooijberg
        /// </summary>
        ///
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
        /// By: Duncan Dreize and Niels Essink.
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
                            Source = new BitmapImage(new Uri("\\mempic.png", UriKind.Relative)),
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
                        //TODO: dont add to card collection but add information to the card with the same row and column.
                        game.CardCollection.Add(card);
                    }
                }
            }
            else
            {
                foreach (Card card in game.CardCollection)
                {
                    Image image = new Image()
                    {
                        Stretch = Stretch.Fill,
                        Source = card.Back,
                        Tag = new int[] { card.Row, card.Column }
                    };

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
        /// Configures the PlayingField.
        /// By: Mark Hooijberg.
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

        ///<summary>
        ///Score system with time and Mulitpliers
        ///by: Jur Stedehouder, Peter Jongman
        ///Enhanced by: Mark Hooijberg
        ///</summary>        
        private int BaseScore = 100;
        //private int BaseScore2 = 100;
        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (canClick == true)
            {
                Image image = (Image)sender;
                firstCard = game.GetCard(image);
                image.Source = firstCard.Front;

                //CheckCard(image, card);
                CheckCardSimple(image, firstCard);
            }
        }

        private void CheckCardSimple(Image image, Card card)
        {
            //This method doesnt use the GetLastCard() method and saves space
            if (lastCard != null && lastCard != card)
            {
                canClick = false;
                viewCardTimer.Start();
                
            }
            else
                lastCard = card;
        }

        /// <summary>
        /// Checks the card
        /// </summary>
        /// <param name="image">some description</param>
        /// <param name="card">another description</param>
        private void CheckCard(Image image, Card card)
        { 
            card.SetAtMove(game);
            card.IsTurned = true;

            if ((game.CardCollection.Where(x => x.IsTurned == true).Count() - 1) % 2 > 0)
            {                
                Card lastCard = game.GetLastCard();
                if ((card.Front as BitmapImage).UriSource == (lastCard.Front as BitmapImage).UriSource)
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

                    grd_cardGrid.Children.Remove(image);
                    grd_cardGrid.Children.Remove(lastCard.Image);

                    if (grd_cardGrid.Children.Count == 0)
                        Content = new UserControl_EndScreen(game);
                }
                else
                {
                    card.AtMove = null;
                    lastCard.AtMove = null;
                    card.IsTurned = false;
                    lastCard.IsTurned = false;


                    image.Source = card.Back;
                    lastCard.Image.Source = lastCard.Back;
                }
                game.SwitchTurn();
                SetActiveColors();
            }
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
        private void Btn_Quit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.PlayMusic(new Uri("MenuMusic.mp3", UriKind.Relative));
            Content = new UserControl_MainMenu();
        }

        /// <summary>
        /// Save the state of the game and return to main menu.
        /// By: Jur Stedehouder
        /// </summary>
        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            // TO DO: Put code to save game here.
            game.Save();
            MainWindow.PlayMusic(new Uri("MenuMusic.mp3", UriKind.Relative));
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
            //TODO: Verfijning, als het te klein wordt dan crasht het
            foreach (RowDefinition rowDefinition in grd_cardGrid.RowDefinitions)
                rowDefinition.Height = new GridLength(grd_cardGrid.Height / grd_cardGrid.RowDefinitions.Count());
            
            foreach (ColumnDefinition columnDefinition in grd_cardGrid.ColumnDefinitions)
                columnDefinition.Width = new GridLength(grd_cardGrid.Width / grd_cardGrid.ColumnDefinitions.Count());
        }

        /// <summary>
        /// Updates adds a second to the time and time label.
        /// Originally witten by: Jur Stedehouder
        /// Enhanced by: Mark Hooijberg
        /// Modified by: Duncan Dreize, added a second timer and keeps track of time per player.
        /// </summary>
        private void dtClockTime_Tick(object sender, EventArgs e)
        {
            game.GetActivePlayer().Time += new TimeSpan(0, 0, 1);


            if (game.Turn == Game.PlayerTurn.Player1)
            { 
                lbl_player1Time.Content = game.GetActivePlayer().Time.Duration().ToString(@"mm\:ss");
            }
             else if (game.Turn == Game.PlayerTurn.Player2)
            {
                lbl_player2Time.Content = game.GetActivePlayer().Time.Duration().ToString(@"mm\:ss");
            }
            

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
            MainWindow.PlayMusic(new Uri("MenuMusic.mp3", UriKind.Relative));
            Content = new UserControl_EndScreen(this.game);
        }
        #endregion
    }
}
