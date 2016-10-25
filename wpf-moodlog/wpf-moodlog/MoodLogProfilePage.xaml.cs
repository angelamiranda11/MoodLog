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

namespace wpf_moodlog
{
    /// <summary>
    /// Interaction logic for MoodLogProfilePage.xaml
    /// </summary>
    public partial class MoodLogProfilePage : Page
    {
        public MoodLogProfilePage()
        {
            InitializeComponent();

            customizePage();
        }

        private void customizePage()
        {
            setDateTodayLabel();
            setUserDetails();
        }

        private void setUserDetails()
        {
            lastName.Text = Global.User.LastName;
            firstName.Text = Global.User.FirstName;
            username.Text = Global.User.Username;
            password.Text = getHiddenPassword();
            birthday.Text = Global.User.Birthday;
            gender.Text = Global.User.GenderCode;
        }

        private string getHiddenPassword()
        {
            string password = Global.User.Password;

            return password[0] + new String('x', password.Length - 1);
        }

        private void setDateTodayLabel()
        {
            // Get the current date.
            DateTime thisDay = DateTime.Today;

            dateTodayLabel.Content = thisDay.ToString("MMM d");
        }

        private void profileButton_MouseEnter(object sender, RoutedEventArgs e)
        {
            profileLabel.Foreground = Brushes.White;
            profileImage.Source = new BitmapImage(new Uri("Images/profile-white.png", UriKind.Relative));
        }

        private void profileButton_MouseLeave(object sender, RoutedEventArgs e)
        {
            profileLabel.Foreground = App.Current.Resources["DarkPurple"] as SolidColorBrush;
            profileImage.Source = new BitmapImage(new Uri("Images/profile-black.png", UriKind.Relative));
        }

        private void entriesButton_Click(object sender, RoutedEventArgs e)
        {
            // View Entries page
            MoodLogEntriesPage moodlogEntriesPage = Global.EntriesPage;
            this.NavigationService.Navigate(moodlogEntriesPage);
        }

        private void entriesButton_MouseEnter(object sender, RoutedEventArgs e)
        {
            entriesLabel.Foreground = Brushes.White;
            entriesImage.Source = new BitmapImage(new Uri("Images/entries-white.png", UriKind.Relative));
        }

        private void entriesButton_MouseLeave(object sender, RoutedEventArgs e)
        {
            entriesLabel.Foreground = App.Current.Resources["DarkViolet"] as SolidColorBrush;
            entriesImage.Source = new BitmapImage(new Uri("Images/entries-black.png", UriKind.Relative));
        }

        private void statsButton_Click(object sender, RoutedEventArgs e)
        {
            // View Stats page
            MoodLogStatsPage moodLogStatsPage = Global.StatsPage;
            this.NavigationService.Navigate(moodLogStatsPage);
        }

        private void statsButton_MouseEnter(object sender, RoutedEventArgs e)
        {
            statsLabel.Foreground = Brushes.White;
            statsImage.Source = new BitmapImage(new Uri("Images/stats-white.png", UriKind.Relative));
        }

        private void statsButton_MouseLeave(object sender, RoutedEventArgs e)
        {
            statsLabel.Foreground = App.Current.Resources["LightPurple"] as SolidColorBrush;
            statsImage.Source = new BitmapImage(new Uri("Images/stats-black.png", UriKind.Relative));
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            // View Login page
            MessageBoxResult result = MessageBox.Show("Are you sure you want to go?", "Logout", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                MoodLogLoginPage moodLogLoginPage = new MoodLogLoginPage();
                this.NavigationService.Navigate(moodLogLoginPage);
            }
        }

        private void logoutButton_MouseEnter(object sender, RoutedEventArgs e)
        {
            logoutLabel.Foreground = Brushes.White;
            logoutImage.Source = new BitmapImage(new Uri("Images/logout-white.png", UriKind.Relative));
        }

        private void logoutButton_MouseLeave(object sender, RoutedEventArgs e)
        {
            logoutLabel.Foreground = App.Current.Resources["Green"] as SolidColorBrush;
            logoutImage.Source = new BitmapImage(new Uri("Images/logout-black.png", UriKind.Relative));
        }
    }
}
