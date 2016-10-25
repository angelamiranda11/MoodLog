using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Linq;
using System.Reflection;
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

namespace wpf_moodlog
{
    /// <summary>
    /// Interaction logic for MoodLogLoginPage.xaml
    /// </summary>
    public partial class MoodLogLoginPage : Page
    {
        public MoodLogLoginPage()
        {
            InitializeComponent();
            usernameTextBox.Focus();
        }

        private void signupButton_Click(object sender, RoutedEventArgs e)
        {
            // View Sign Up page
            MoodLogSignUpPage moodLogSignUpPage = new MoodLogSignUpPage();
            this.NavigationService.Navigate(moodLogSignUpPage);
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (isValidUser())
            {
                MoodLogEntriesPage moodLogEntriesPage = new MoodLogEntriesPage();
                this.NavigationService.Navigate(moodLogEntriesPage);
            }
            else
            {
                errorTextBlock.Visibility = Visibility.Visible;
            }
        }

        private bool isValidUser()
        {
            Stream userCsvStream = getUserCsvStream();
            bool isValidUser = false;

            using (CsvFileReader reader = new CsvFileReader(userCsvStream))
            {
                CsvRow row = new CsvRow();
                while (reader.ReadRow(row))
                {
                    string username = getUsernameFrom(row);
                    string password = getPasswordFrom(row);

                    if (username == usernameTextBox.Text && password == passwordBox.Password)
                    {
                        isValidUser = true;
                        Global.User = new User(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7]);
                    }
                }
            }

            return isValidUser;
        }

        private Stream getUserCsvStream()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream("wpf_moodlog.Data.user.csv");
        }

        private string getUsernameFrom(CsvRow row)
        {
            return row[5];
        }

        private string getPasswordFrom(CsvRow row)
        {
            return row[6];
        }
    }
}
