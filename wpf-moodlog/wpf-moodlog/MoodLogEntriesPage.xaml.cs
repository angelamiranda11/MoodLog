using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Reflection;
using System.IO;
using System.Collections;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Diagnostics;

namespace wpf_moodlog
{
    public enum Emotion { Joy, Surprised, Sadness, Disgust, Anger, Fear };

    public static class EmotionExtensions
    {
        public static SolidColorBrush GetColor(this Emotion e)
        {
            switch (e)
            {
                case Emotion.Joy:
                    return Brushes.Green;
                case Emotion.Surprised:
                    return Brushes.DarkGoldenrod;
                case Emotion.Sadness:
                    return Brushes.Blue;
                case Emotion.Disgust:
                    return Brushes.Purple;
                case Emotion.Anger:
                    return Brushes.Red;
                case Emotion.Fear:
                    return Brushes.DarkSlateGray;
                default:
                    return Brushes.Black;
            }
        }

        public static string GetName(this Emotion e)
        {
            return Enum.GetName(typeof(Emotion), e);
        }
    }
    /// <summary>
    /// Interaction logic for MoodLogEntriesPage.xaml
    /// </summary>
    public partial class MoodLogEntriesPage : Page
    {

        public MoodLogEntriesPage()
        {
            InitializeComponent();

            customizePage();
            
            loadPreviousEntries();
        }

        private void loadPreviousEntries()
        {
            Stream entries = getEntriesStream();

            using (CsvFileReader reader = new CsvFileReader(entries))
            {
                CsvRow row = new CsvRow();
                while (reader.ReadRow(row))
                {
                    string date = row[1];
                    string time = row[2];
                    string text = row[3];

                    string dateTime = date + " " + time;
                    Border previousEntry = createEntryFrom(text, true, dateTime);

                    entriesStackPanel.Children.Add(previousEntry);
                }
            }
        }

        public Stream getEntriesStream()
        {
            string filename = Global.User.Entries;

            return Assembly.GetExecutingAssembly().GetManifestResourceStream("wpf_moodlog.Data." + filename + ".csv");
        }

        private void customizePage()
        {
            setDateTodayLabel();
            setHiUserLabel();
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

        private void joyButton_Click(object sender, RoutedEventArgs e)
        {
            enableAllMainEmoticonButtonsExcept(joyButton);

            showSubEmoticons(new string[] {
                "smiling_face",
                "smiling_face_with_smiling_eyes",
                "smiling_face_with_heart_shaped_eyes",
                "face_with_tears_of_joy",
                "smiling_face_with_open_mouth",
                "grinning_face_with_smiling_eyes",
                "smiling_face_with_halo",
                "heavy_black_heart",
                "winking_face",
            });
        }

        private void sadnessButton_Click(object sender, RoutedEventArgs e)
        {
            enableAllMainEmoticonButtonsExcept(sadnessButton);

            showSubEmoticons(new string[] {
                "sad_face",
                "pensive_face",
                "disappointed_face",
                "loudly_crying_face",
                "crying_face",
                "worried_face",
                "broken_hearts",
                "purple_heart",
            });
        }

        private void disgustButton_Click(object sender, RoutedEventArgs e)
        {
            enableAllMainEmoticonButtonsExcept(disgustButton);

            showSubEmoticons(new string[] {
                "face_with_stucked_out_tongue",
                "face_with_stucked_out_tongue_and_tightly_closed_eyes",
            });
        }

        private void angerButton_Click(object sender, RoutedEventArgs e)
        {
            enableAllMainEmoticonButtonsExcept(angerButton);

            showSubEmoticons(new string[] {
                "pouting_face",
                "face_with_look_of_triumph",
                "angry_face",
                "smiling_face_with_horns"
            });
        }

        private void surpriseButton_Click(object sender, RoutedEventArgs e)
        {
            enableAllMainEmoticonButtonsExcept(surpriseButton);

            showSubEmoticons(new string[] {
                "face_with_open_mouth",
                "astonished_face",
                "hushed_face",
                "face_screaming_in_fear",
                
            });
        }

        private void fearButton_Click(object sender, RoutedEventArgs e)
        {
            enableAllMainEmoticonButtonsExcept(fearButton);

            showSubEmoticons(new string[] {
                "face_screaming_in_fear",
                "face_with_cold_sweat",
                "fearful_face",
            });
        }

        private void hashtagButton_Click(object sender, RoutedEventArgs e)
        {
            appendToEntryTextBox("#");
        }

        private void subEmoticonButton_Click(object sender, RoutedEventArgs e)
        {
            Button subEmoticonButton = e.Source as Button;
            appendToEntryTextBox(":" + subEmoticonButton.Name + ":");
        }

        private void setDateTodayLabel()
        {
            // Get the current date.
            DateTime thisDay = DateTime.Today;

            dateTodayLabel.Content = thisDay.ToString("MMM d");
        }

        private void setHiUserLabel()
        {
            hiUserLabel.Content = "Hi, " + Global.User.FirstName;
        }

        private void appendToEntryTextBox(String str)
        {
            entryTextBox.Text += str;
            entryTextBox.Focus();

            // Place cursor on end of text
            entryTextBox.Select(entryTextBox.Text.Length, 0);
        }


        private void enableAllMainEmoticonButtonsExcept(Button mainEmoticonButtonToDisable)
        {
            Button[] mainEmoticonButtons =
            {
                joyButton,
                sadnessButton,
                disgustButton,
                angerButton,
                surpriseButton,
                fearButton
            };

            foreach(Button mainEmoticonButton in mainEmoticonButtons)
            {
                enableEmoticonButton(mainEmoticonButton);
            }

            disableEmoticonButton(mainEmoticonButtonToDisable);
        }

        private void enableEmoticonButton(Button emoticonButton)
        {
            emoticonButton.Background = Brushes.Transparent;
        }

        private void disableEmoticonButton(Button emoticonButton)
        {
            emoticonButton.Background = Brushes.LightGray;
        }

        private void showSubEmoticons(String[] subEmoticonNames)
        {
            resetSubEmoticons();

            foreach (String subEmoticonName in subEmoticonNames)
            {
                Button subEmoticonButton = createSubEmoticonButtonWithName(subEmoticonName);

                showSubEmoticon(subEmoticonButton);
            }
        }

        private void showSubEmoticon(Button subEmoticonButton)
        {
            subEmoticonsPanel.Children.Add(subEmoticonButton);
        }

        private Button createSubEmoticonButtonWithName(String subEmoticonName)
        {
            Button newSubEmoticonButton = new Button();

            setPropertiesOfSubEmoticonButton(newSubEmoticonButton, subEmoticonName);

            return newSubEmoticonButton;
        }

        private Button setPropertiesOfSubEmoticonButton(Button subEmoticonButton, String subEmoticonName)
        {
            subEmoticonButton.Content = convertUriToImage(new Uri("Images/" + subEmoticonName + ".png", UriKind.Relative));
            subEmoticonButton.Width = 50;
            subEmoticonButton.Height = 40;
            subEmoticonButton.Background = Brushes.Transparent;
            subEmoticonButton.BorderBrush = Brushes.DarkGray;
            subEmoticonButton.BorderThickness = new Thickness(0, 0, 1, 0);
            subEmoticonButton.ToolTip = subEmoticonName.Replace('-', ' ');
            subEmoticonButton.Name = subEmoticonName;
            subEmoticonButton.Click += new RoutedEventHandler(subEmoticonButton_Click);

            return subEmoticonButton;
        }

        private Image convertUriToImage(Uri uri)
        {
            BitmapImage bitmapImage = new BitmapImage(uri);
            Image image = new Image();
            image.Source = bitmapImage;

            return image;
        }

        private void resetSubEmoticons()
        {
            subEmoticonsPanel.Children.Clear();
        }

        private SolidColorBrush convertHexToBrush(String hex)
        {
            BrushConverter brushConverter = new BrushConverter();
            SolidColorBrush brush = (SolidColorBrush)brushConverter.ConvertFrom(hex);
            brush.Freeze();

            return brush;
        }

        private void addEntryButton_Click(object sender, RoutedEventArgs e)
        {
            Border newEntry = createEntryFrom(entryTextBox.Text, false, "");

            entriesStackPanel.Children.Add(newEntry);
            
            resetEntryTextBox();
        }

        private void resetEntryTextBox()
        {
            entryTextBox.Text = "";
        }

        private Border addBorderToPanel(StackPanel panel)
        {
            Border borderedPanel = createBorderedPanel();

            borderedPanel.Child = panel;

            return borderedPanel;
        }

        private Border createBorderedPanel()
        {
            Border borderedPanel = new Border();

            setPropertiesOfBorderedPanel(borderedPanel);

            return borderedPanel;
        }

        private void setPropertiesOfBorderedPanel(Border borderedPanel)
        {
            borderedPanel.BorderBrush = Brushes.DarkGray;
            borderedPanel.BorderThickness = new Thickness(1);
        }

        private StackPanel combineSummaryAndContentOfEntry(DockPanel summary, TextBlock content)
        {
            StackPanel entry = new StackPanel();
            entry.Children.Add(summary);
            entry.Children.Add(content);

            return entry;
        }

        private TextBlock createSummaryDateTime(bool isPreviousEntry, string dateTime)
        {
            DateTime thisDay = DateTime.Now;

            TextBlock summaryDateTime = new TextBlock();

            if (isPreviousEntry)
            {
                summaryDateTime.Text = dateTime;
            }
            else
            {
                summaryDateTime.Text = thisDay.ToString("dddd, MMMM dd, h:mm tt");
            }

            return summaryDateTime;
        }

        private Dictionary<Emotion, float> getEmotionsFrom(String text, bool isPreviousEntry)
        {
            var emotions = new Dictionary<Emotion, float>();

            if (isPreviousEntry)
            {
                emotions.Add(Emotion.Joy, 0);
                emotions.Add(Emotion.Sadness, 0);
                emotions.Add(Emotion.Anger, 0);
                emotions.Add(Emotion.Surprised, 0);
                emotions.Add(Emotion.Disgust, 0);
                emotions.Add(Emotion.Fear, 0);
            }
            else
            {
                Program p = new Program();
                float[] results = p.processText(text);

                emotions.Add(Emotion.Joy, results[0]);
                emotions.Add(Emotion.Sadness, results[1]);
                emotions.Add(Emotion.Anger, results[2]);
                emotions.Add(Emotion.Surprised, results[3]);
                emotions.Add(Emotion.Disgust, results[4]);
                emotions.Add(Emotion.Fear, results[5]);
            }

            return emotions;
        }

        private TextBlock createSummaryDominantEmotionFrom(Dictionary<Emotion, float> emotions)
        {
            Emotion dominantEmotion = getDominantEmotionIn(emotions);

            TextBlock summaryDominantEmotion = new TextBlock()
            {
                FontWeight = FontWeights.Bold,
                Foreground = dominantEmotion.GetColor(),
                Text = dominantEmotion.GetName().ToUpper(),
            };

            return summaryDominantEmotion;
        }

        private PieSeries createSummaryEmotionsChartFrom(Dictionary<Emotion, float> emotions)
        {
            var allEmotionsChart = new Chart();
            var pieSeries = new PieSeries()
            {
                DependentValueBinding = new Binding("Value"),
                Height = 60,
                IndependentValueBinding = new Binding("Key"),
                ItemsSource = emotions.ToList(),
                Margin = new Thickness(5, 0, 20, 0),
                Width = 60,
                Palette = Application.Current.Resources["ChartPalette"] as Collection<ResourceDictionary>,
            };

            allEmotionsChart.Series.Add(pieSeries);

            return pieSeries;
        }

        private Emotion getDominantEmotionIn(Dictionary<Emotion, float> emotions)
        {
            Emotion maxEmotion = 0;
            float maxValue = 0;

            foreach (var x in emotions)
            {
                if (x.Value > maxValue)
                {
                    maxEmotion = x.Key;
                    maxValue = x.Value;
                }
            }

            return maxEmotion;
        }

        private Grid createCircleWithText(string str, SolidColorBrush color)
        {
            Grid circleWithText = new Grid();

            var ellipse = new Ellipse() {
                Fill = color,
                Height = 25,
                Width = 25,
            };

            var text = new Label()
            {
                Content = str,
                FontSize = 8,
                Foreground = Brushes.White,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Width = 25,
            };

            circleWithText.Children.Add(ellipse);
            circleWithText.Children.Add(text);

            return circleWithText;
        }

        private StackPanel createSummaryEmotionsTextFrom(Dictionary<Emotion, float> emotions)
        {
            StackPanel summaryEmotionsText = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0,5,0,10)
            };

            foreach(var emotion in emotions)
            {
                var valueInCircle = createCircleWithText((emotion.Value) * 100 + "%", emotion.Key.GetColor());
                var emotionInText = new TextBlock()
                {
                    Margin = new Thickness(3,0,20,0),
                    Text = emotion.Key.GetName(),
                    VerticalAlignment = VerticalAlignment.Center,
                };

                summaryEmotionsText.Children.Add(valueInCircle);
                summaryEmotionsText.Children.Add(emotionInText);
            }

            return summaryEmotionsText;
        }

        private DockPanel createEntrySummaryFrom(String text, bool isPreviousEntry, string dateTime)
        {
            DockPanel summary = new DockPanel()
            {
                Background = convertHexToBrush("#ecf0f1"),
            };  

            Dictionary<Emotion, float> emotions = getEmotionsFrom(text, isPreviousEntry);

            PieSeries allEmotionsChart = createSummaryEmotionsChartFrom(emotions);
            TextBlock dateTimeText = createSummaryDateTime(isPreviousEntry, dateTime);
            TextBlock dominantEmotion = createSummaryDominantEmotionFrom(emotions);
            StackPanel allEmotionsText = createSummaryEmotionsTextFrom(emotions);

            DockPanel.SetDock(allEmotionsChart, Dock.Left);
            DockPanel.SetDock(dateTimeText, Dock.Top);
            DockPanel.SetDock(dominantEmotion, Dock.Top);
            DockPanel.SetDock(allEmotionsText, Dock.Top);

            summary.Children.Add(allEmotionsChart);
            summary.Children.Add(dateTimeText);
            summary.Children.Add(dominantEmotion);
            summary.Children.Add(allEmotionsText);

            return summary;
        }

        private TextBlock createEntryContentFrom(String text)
        {
            TextBlock content = new TextBlock()
            {
                Margin = new Thickness(5),
                Text = text,
                TextWrapping = TextWrapping.WrapWithOverflow,
            };

            return content;
        }

        private void setPropertiesOfEntryWithBorder(Border entryWithBorder)
        {
            entryWithBorder.LayoutTransform = new RotateTransform(180);
            entryWithBorder.Margin = new Thickness(0, 10, 0, 10);
        }
        
        private Border createEntryFrom(String text, bool isPreviousEntry, string dateTime)
        {
            DockPanel summary = createEntrySummaryFrom(text, isPreviousEntry, dateTime);
            TextBlock content = createEntryContentFrom(text);

            StackPanel newEntry = combineSummaryAndContentOfEntry(summary, content);

            Border newEntryWithBorder = addBorderToPanel(newEntry);

            setPropertiesOfEntryWithBorder(newEntryWithBorder);

            return newEntryWithBorder;  
        }
    }
}
