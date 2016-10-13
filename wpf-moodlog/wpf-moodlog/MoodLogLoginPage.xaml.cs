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
    /// Interaction logic for MoodLogLoginPage.xaml
    /// </summary>
    public partial class MoodLogLoginPage : Page
    {
        public MoodLogLoginPage()
        {
            InitializeComponent();
        }

        private void signupButton_Click(object sender, RoutedEventArgs e)
        {
            // View Sign Up page
            MoodLogSignUpPage moodLogSignUpPage = new MoodLogSignUpPage();
            this.NavigationService.Navigate(moodLogSignUpPage);
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            // Temporary code
            MoodLogEntriesPage moodLogEntriesPage = new MoodLogEntriesPage();
            this.NavigationService.Navigate(moodLogEntriesPage);
        }
    }
}
