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
        private Game game;
        private DispatcherTimer timer;

        public TimeSpan time;

        public UserControl_GameField(Game _game)
        {
            InitializeComponent();
            game = _game;

            time = new TimeSpan(0, 0, 0);
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000); 
            timer.Tick += dtClockTime_Tick;
            timer.Start();
            
            lbl_player1Score.Content = game.Player1.Score;
            lbl_player2Score.Content = game.Player2.Score;

            SetActiveColors();
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
            }
            else
            {
                lbl_player1Name.Foreground = new SolidColorBrush(Colors.DarkCyan);
                lbl_player2Name.Foreground = new SolidColorBrush(Colors.Aqua);
                lbl_player1Score.Foreground = new SolidColorBrush(Colors.DarkCyan);
                lbl_player2Score.Foreground = new SolidColorBrush(Colors.Aqua);
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

            string[] files = Directory.GetFiles(currentDirectory + "\\Harold");
            List<ImageSource> images = new List<ImageSource>();

            for (int i = 0; i < cardAmount; i++)
            {
                int imageNr = i % (cardAmount / 2);
                ImageSource source = new BitmapImage(new Uri(files[imageNr]));
                images.Add(source);
            }
            return images;
        }
        private void FillPlayField()
        {
            List<ImageSource> images = GetImagesList();
            for (int row = 0; row < grd_cardGrid.RowDefinitions.Count; row++)
            {
                for (int column = 0; column < grd_cardGrid.ColumnDefinitions.Count; column++)
                {
                    Image image = new Image() {
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
                        Front = images.First(),
                        Image = image,
                        IsTurned = false,
                        Row = row
                    };
                    
                    images.RemoveAt(0);
                    grd_cardGrid.Children.Add(image);
                    //TODO: dont add to card collection but add information to the card with the same row and column.
                    game.CardCollection.Add(card);
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

            SetupPlayfield(game.Config.FieldHeight, game.Config.FieldWidth);
            FillPlayField();
            SetNames(new string[] { game.Player1.Name, game.Player2.Name});
        }

        ///<summary>
        ///Score system with time and Mulitpliers
        ///by: Jur Stedehouder, Peter Jongman
        ///Enhached by: Mark Hooijberg
        ///</summary>        
        private int BaseScore = 100;
        //private int BaseScore2 = 100;
        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image image = (Image)sender;
            Card card = game.GetCard(image);
            image.Source = card.Front;

            //CheckCard(image, card);
            CheckCardSimple(image, card);
        }
        private Card lastCard;
        private void CheckCardSimple(Image image, Card card)
        {
            //This method doesnt use the GetLastCard() method and saves space
            if (lastCard != null)
            {
                if ((card.Front as BitmapImage).UriSource == (lastCard.Front as BitmapImage).UriSource)
                {
                    int AddedScore = 0;
                    if (TimeSpan.Parse("00:" + klok.Content.ToString()).TotalSeconds > 30)
                        AddedScore -= 90;

                    else if (TimeSpan.Parse("00:" + klok.Content.ToString()).TotalSeconds > 10)
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
                    image.Source = card.Back;
                    lastCard.Image.Source = lastCard.Back;
                }
                lastCard = null;
                game.SwitchTurn();
                SetActiveColors();
            }
            else
                lastCard = card;
        }

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
                    if (TimeSpan.Parse("00:" + klok.Content.ToString()).TotalSeconds > 30)
                        AddedScore -= 90;

                    else if (TimeSpan.Parse("00:" + klok.Content.ToString()).TotalSeconds > 10)
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
            //TODO: Verfijning, als het te klein word dan crasht het
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
            Content = new UserControl_EndScreen(game);
        }
        #endregion
    }
}
