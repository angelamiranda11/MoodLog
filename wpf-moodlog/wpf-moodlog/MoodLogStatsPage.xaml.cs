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
        private ObservableCollection<DataPoint> Points = new ObservableCollection<DataPoint>();
        public class DataPoint
        {
            public int Day { get; set; }
            public float Joy { get; set; }
            public float Sadness { get; set; }
            public float Anger { get; set; }
            public float Surprised { get; set; }
            public float Disgust { get; set; }
            public float Fear { get; set; }
        }

        public MoodLogStatsPage()
        {
            InitializeComponent();

            customizePage();
        }

        private void customizePage()
        {
            setDateTodayLabel();

            setCalendarDisplayDate();

            loadChartData();

            setAllSeriesDataContext();
        }

        private void calendar_DisplayDateChanged(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Calendar's display date changed");
            loadChartData();
        }

        private void loadChartData()
        {
            Points.Clear();

            initDaysOfPoints();

            User user = Global.User;
            using (CsvFileReader reader = new CsvFileReader(Global.GetStreamOf(user.EntriesFilename, FileMode.Open)))
            {
                CsvRow thisRow = new CsvRow();

                while (reader.ReadRow(thisRow))
                {
                    int month = getMonthFrom(thisRow);

                    if (month == selectedMonth())
                    {
                        int day = getDayFrom(thisRow);

                        var point = getPointOnThis(day);

                        point.Joy = float.Parse(thisRow[7]);
                        point.Sadness = float.Parse(thisRow[8]);
                        point.Anger = float.Parse(thisRow[9]);
                        point.Surprised = float.Parse(thisRow[10]);
                        point.Disgust = float.Parse(thisRow[11]);
                        point.Fear = float.Parse(thisRow[12]);
                    }
                }
            }

            foreach(DataPoint point in Points)
            {
                Console.WriteLine(point.Day + "," + point.Joy + "," + point.Sadness + "," + point.Anger + "," + point.Surprised + "," + point.Disgust + "," + point.Fear);
            }
        }

        private void setAllSeriesDataContext()
        {
            joySeries.DataContext = Points;
            sadnessSeries.DataContext = Points;
            angerSeries.DataContext = Points;
            disgustSeries.DataContext = Points;
            surprisedSeries.DataContext = Points;
            fearSeries.DataContext = Points;
        }

        private DataPoint getPointOnThis(int day)
        {
            return Points.Where(X => X.Day == day).FirstOrDefault();
        }

        private void initDaysOfPoints()
        {
            int daysInMonth = getNDaysIn(selectedMonth());

            for (int i = 1; i <= daysInMonth; i++)
            {
                Points.Add(new DataPoint()
                {
                    Day = i,
                    Joy = 0,
                    Sadness = 0,
                    Anger = 0,
                    Disgust = 0,
                    Surprised = 0,
                    Fear = 0
                });
            }
        }

        private int getNDaysIn(int thisMonth)
        {
            int yearNow = DateTime.Now.Year;

            return DateTime.DaysInMonth(yearNow, thisMonth);
        }

        private int selectedMonth()
        {
            return calendar.DisplayDate.Month;
        }

        private int getMonthFrom(CsvRow row)
        {
            return Convert.ToInt32(row[2]);
        }

        private int getDayFrom(CsvRow row)
        {
            return Convert.ToInt32(row[3]);
        }

        private void setDateTodayLabel()
        {
            // Get the current date.
            DateTime thisDay = DateTime.Today;

            dateTodayLabel.Content = thisDay.ToString("MMM d");
        }

        private void setCalendarDisplayDate()
        {
            calendar.DisplayDate = DateTime.Now;
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
