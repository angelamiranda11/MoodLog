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
        private ObservableCollection<DataPoint> MonthPoints = new ObservableCollection<DataPoint>();
        private ObservableCollection<DataPoint> WeekOnePoints = new ObservableCollection<DataPoint>();
        private ObservableCollection<DataPoint> WeekTwoPoints = new ObservableCollection<DataPoint>();
        private ObservableCollection<DataPoint> WeekThreePoints = new ObservableCollection<DataPoint>();
        private ObservableCollection<DataPoint> WeekFourPoints = new ObservableCollection<DataPoint>();
        private ObservableCollection<DataPoint> WeekFivePoints = new ObservableCollection<DataPoint>();

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

            initDaysOfMonthPoints();

            loadChartData();

            setAllSeriesDataContext();
        }

        private void calendar_DisplayDateChanged(object sender, RoutedEventArgs e)
        {
            loadChartData();
        }

        private void loadChartData()
        {
            MonthPoints.Clear();
            WeekOnePoints.Clear();
            WeekTwoPoints.Clear();
            WeekThreePoints.Clear();
            WeekFourPoints.Clear();
            WeekFivePoints.Clear();

            initDaysOfMonthPoints();

            DataPoint previousPoint = new DataPoint();
            int nEntriesOnDay = 1;
            int day = 0;

            User user = Global.User;
            using (CsvFileReader reader = new CsvFileReader(Global.GetStreamOf(user.EntriesFilename, FileMode.Open)))
            {
                CsvRow thisRow = new CsvRow();

                while (reader.ReadRow(thisRow))
                {
                    DateTime date = getDateTimeFrom(thisRow);
                    int weekNo = date.GetWeekOfMonth();

                    DataPoint point;

                    if (date.Month == selectedMonth())
                    {
                        if (day == getDayFrom(thisRow))
                        {
                            point = getPointOnThis(day);

                            nEntriesOnDay++;

                            MonthPoints.Remove(point);

                            float[] values = getValuesFrom(thisRow);

                            point.Joy += values[0];
                            point.Sadness += values[1];
                            point.Anger += values[2];
                            point.Surprised += values[3];
                            point.Disgust += values[4];
                            point.Fear += values[5];

                            MonthPoints.Add(point);

                            previousPoint = point;
                        }
                        else
                        {
                            if (nEntriesOnDay != 1)
                            {
                                MonthPoints.Remove(previousPoint);

                                previousPoint.Joy /= nEntriesOnDay;
                                previousPoint.Sadness /= nEntriesOnDay;
                                previousPoint.Anger /= nEntriesOnDay;
                                previousPoint.Surprised /= nEntriesOnDay;
                                previousPoint.Disgust /= nEntriesOnDay;
                                previousPoint.Fear /= nEntriesOnDay;

                                nEntriesOnDay = 1;

                                switch (weekNo)
                                {
                                    case 1:
                                        WeekOnePoints.Add(previousPoint);
                                        break;
                                    case 2:
                                        WeekTwoPoints.Add(previousPoint);
                                        break;
                                    case 3:
                                        WeekThreePoints.Add(previousPoint);
                                        break;
                                    case 4:
                                        WeekFourPoints.Add(previousPoint);
                                        break;
                                    case 5:
                                        WeekFivePoints.Add(previousPoint);
                                        break;
                                    default:
                                        break;
                                }

                                MonthPoints.Add(previousPoint);
                            }
                               
                            day = getDayFrom(thisRow);

                            point = getPointOnThis(day);

                            MonthPoints.Remove(point);

                            float[] values = getValuesFrom(thisRow);

                            point.Joy = values[0];
                            point.Sadness = values[1];
                            point.Anger = values[2];
                            point.Surprised = values[3];
                            point.Disgust = values[4];
                            point.Fear = values[5];

                            switch (weekNo)
                            {
                                case 1:
                                    WeekOnePoints.Add(point);
                                    break;
                                case 2:
                                    WeekTwoPoints.Add(point);
                                    break;
                                case 3:
                                    WeekThreePoints.Add(point);
                                    break;
                                case 4:
                                    WeekFourPoints.Add(point);
                                    break;
                                case 5:
                                    WeekFivePoints.Add(point);
                                    break;
                                default:
                                    break;
                            }

                            MonthPoints.Add(point);
                        }

                    }
                }
            }

            foreach (var monthPoint in MonthPoints)
            {
                Console.WriteLine(monthPoint.Day + "," + monthPoint.Joy + "," + monthPoint.Sadness + "," + monthPoint.Anger + "," + monthPoint.Surprised + "," + monthPoint.Disgust + "," + monthPoint.Fear);
            }
            Console.WriteLine("End of week one points");
        }
        
        private DateTime getDateTimeFrom(CsvRow thisRow)
        {
            int year = Convert.ToInt32(thisRow[1]);
            int month = Convert.ToInt32(thisRow[2]);
            int day = Convert.ToInt32(thisRow[3]);
            int hour = Convert.ToInt32(thisRow[4]);
            int minute = Convert.ToInt32(thisRow[5]);

            return new DateTime(year, month, day, hour, minute, 0);
        }

        private float[] getValuesFrom(CsvRow thisRow)
        {
            float[] values = new float[6];
            int firstValueColNo = 7;
            int lastValueColNo = 12;

            for(int i = firstValueColNo; i <= lastValueColNo; i++)
            {
                values[i - firstValueColNo] = float.Parse(thisRow[i]) * 100;
            }

            return values;
        }
        private void setAllSeriesDataContext()
        {
            joySeries.DataContext = MonthPoints;
            sadnessSeries.DataContext = MonthPoints;
            angerSeries.DataContext = MonthPoints;
            disgustSeries.DataContext = MonthPoints;
            surprisedSeries.DataContext = MonthPoints;
            fearSeries.DataContext = MonthPoints;

            joyWeekOneSeries.DataContext = WeekOnePoints;
            sadnessWeekOneSeries.DataContext = WeekOnePoints;
            angerWeekOneSeries.DataContext = WeekOnePoints;
            disgustWeekOneSeries.DataContext = WeekOnePoints;
            disgustWeekOneSeries.DataContext = WeekOnePoints;
            surprisedWeekOneSeries.DataContext = WeekOnePoints;
            fearWeekOneSeries.DataContext = WeekOnePoints;

            joyWeekTwoSeries.DataContext = WeekTwoPoints;
            sadnessWeekTwoSeries.DataContext = WeekTwoPoints;
            angerWeekTwoSeries.DataContext = WeekTwoPoints;
            disgustWeekTwoSeries.DataContext = WeekTwoPoints;
            disgustWeekTwoSeries.DataContext = WeekTwoPoints;
            surprisedWeekTwoSeries.DataContext = WeekTwoPoints;
            fearWeekTwoSeries.DataContext = WeekTwoPoints;

            joyWeekThreeSeries.DataContext = WeekThreePoints;
            sadnessWeekThreeSeries.DataContext = WeekThreePoints;
            angerWeekThreeSeries.DataContext = WeekThreePoints;
            disgustWeekThreeSeries.DataContext = WeekThreePoints;
            disgustWeekThreeSeries.DataContext = WeekThreePoints;
            surprisedWeekThreeSeries.DataContext = WeekThreePoints;
            fearWeekThreeSeries.DataContext = WeekThreePoints;

            joyWeekFourSeries.DataContext = WeekFourPoints;
            sadnessWeekFourSeries.DataContext = WeekFourPoints;
            angerWeekFourSeries.DataContext = WeekFourPoints;
            disgustWeekFourSeries.DataContext = WeekFourPoints;
            disgustWeekFourSeries.DataContext = WeekFourPoints;
            surprisedWeekFourSeries.DataContext = WeekFourPoints;
            fearWeekFourSeries.DataContext = WeekFourPoints;

            joyWeekFiveSeries.DataContext = WeekFivePoints;
            sadnessWeekFiveSeries.DataContext = WeekFivePoints;
            angerWeekFiveSeries.DataContext = WeekFivePoints;
            disgustWeekFiveSeries.DataContext = WeekFivePoints;
            disgustWeekFiveSeries.DataContext = WeekFivePoints;
            surprisedWeekFiveSeries.DataContext = WeekFivePoints;
            fearWeekFiveSeries.DataContext = WeekFivePoints;
        }

        private DataPoint getPointOnThis(int day)
        {
            return MonthPoints.Where(X => X.Day == day).FirstOrDefault();
        }

        private void initDaysOfMonthPoints()
        {
            int daysInMonth = getNDaysIn(selectedMonth());

            for (int i = 1; i <= daysInMonth; i++)
            {
                MonthPoints.Add(new DataPoint()
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
