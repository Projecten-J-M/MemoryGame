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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;



namespace MemoryGame.UserControls
{
    /// <summary>
    /// Interaction logic for UserControl_Highscores.xaml
    /// </summary>
    public partial class UserControl_Highscores : UserControl
    {
        public UserControl_Highscores()
        {
            InitializeComponent();


            List<User> users = new List<User>();

            User user = new User() { Name = "John Doe" };
            users.Add(user);

            user = new User() { Name = "Sammy Doe" };
            users.Add(user);

            Highscore.ItemsSource = users;

        }

        /// <summary>
        /// Returns to main menu.
        /// By: Mark Hooijberg.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            Content = new UserControl_MainMenu();
        }

        public class User
        {

            public string Name { get; set; }

            public DateTime Birthday { get; set; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Highscore.ItemsSource = null;
        }
    }
}
