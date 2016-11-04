using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpf_moodlog
{
    public static class DateTimeExtensions
    {
        static GregorianCalendar _gc = new GregorianCalendar();
        public static int GetWeekOfMonth(this DateTime time)
        {
            DateTime first = new DateTime(time.Year, time.Month, 1);
            return time.GetWeekOfYear() - first.GetWeekOfYear() + 1;
        }

        static int GetWeekOfYear(this DateTime time)
        {
            return _gc.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }
    }
    /// <summary>
    /// Interaction logic for MoodLogStatsPage.xaml
    /// </summary>
    public partial class MoodLogStatsPage : Page
    {
        private KeyValuePair<string, int>[] WeekOneData;
        private KeyValuePair<string, int>[] WeekTwoData;
        private KeyValuePair<string, int>[] WeekThreeData;
        private KeyValuePair<string, int>[] WeekFourData;

        public MoodLogStatsPage()
        {
            InitializeComponent();

            customizePage();
            loadWeeklyChartData();
        }

        private void loadWeeklyChartData()
        {
            loadWeekOneChartData();
            loadWeekTwoChartData();
            loadWeekThreeChartData();
            loadWeekFourChartData();
        }

        private void customizePage()
        {
            setDateTodayLabel();
        }

        private void loadChartData()
        {
            if (calendar.SelectedDate.HasValue)
            {
                //string month = calendar.SelectedDate.Value.ToString("MMMM");
            }

            Stream entries = getEntriesStream();

            using (CsvFileReader reader = new CsvFileReader(entries))
            {
                CsvRow row = new CsvRow();
                while (reader.ReadRow(row))
                {
                    string thisEntryMonth = getMonthFrom(row);

                    if (thisEntryMonth == selectedMonth())
                    {
                        // get emotion values
                    }
                }
            }
        }

        private string selectedMonth()
        {
            return calendar.SelectedDate.Value.ToString("MMMM");
        }

        private string getMonthFrom(CsvRow row)
        {
            return row[2];
        }

        private Stream getEntriesStream()
        {
            string filename = Global.User.Entries;

            return Assembly.GetExecutingAssembly().GetManifestResourceStream("wpf_moodlog.Data." + filename + ".csv");
        }

        private void loadWeekOneChartData()
        {
            ((ColumnSeries)weekOneChart.Series[0]).ItemsSource =
                new KeyValuePair<string, int>[]{
                new KeyValuePair<string,int>("Joy", 12),
                new KeyValuePair<string,int>("Sadness", 25),
                new KeyValuePair<string,int>("Anger", 5),
                new KeyValuePair<string,int>("Suprised", 6),
                new KeyValuePair<string,int>("Disgust", 10),
                new KeyValuePair<string,int>("Fear", 4) };
        }

        private void loadWeekTwoChartData()
        {
            ((ColumnSeries)weekTwoChart.Series[0]).ItemsSource =
                new KeyValuePair<string, int>[]{
                new KeyValuePair<string,int>("Joy", 12),
                new KeyValuePair<string,int>("Sadness", 25),
                new KeyValuePair<string,int>("Anger", 5),
                new KeyValuePair<string,int>("Suprised", 6),
                new KeyValuePair<string,int>("Disgust", 10),
                new KeyValuePair<string,int>("Fear", 4) };
        }

        private void loadWeekThreeChartData()
        {
            ((ColumnSeries)weekThreeChart.Series[0]).ItemsSource =
                new KeyValuePair<string, int>[]{
                new KeyValuePair<string,int>("Joy", 12),
                new KeyValuePair<string,int>("Sadness", 25),
                new KeyValuePair<string,int>("Anger", 5),
                new KeyValuePair<string,int>("Suprised", 6),
                new KeyValuePair<string,int>("Disgust", 10),
                new KeyValuePair<string,int>("Fear", 4) };
        }

        private void loadWeekFourChartData()
        {
            ((ColumnSeries)weekFourChart.Series[0]).ItemsSource =
                new KeyValuePair<string, int>[]{
                new KeyValuePair<string,int>("Joy", 12),
                new KeyValuePair<string,int>("Sadness", 25),
                new KeyValuePair<string,int>("Anger", 5),
                new KeyValuePair<string,int>("Suprised", 6),
                new KeyValuePair<string,int>("Disgust", 10),
                new KeyValuePair<string,int>("Fear", 4) };
        }

        private void setDateTodayLabel()
        {
            // Get the current date.
            DateTime thisDay = DateTime.Today;

            dateTodayLabel.Content = thisDay.ToString("MMM d");
        }

        private void profileButton_Click(object sender, RoutedEventArgs e)
        {
            // View Profile page
            MoodLogProfilePage moodLogProfilePage = Global.ProfilePage;
            this.NavigationService.Navigate(moodLogProfilePage);
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

        private void calendar_DisplayModeChanged(object sender, RoutedEventArgs e)
        {
            calendar.DisplayMode = CalendarMode.Year;
        }
    }
}
