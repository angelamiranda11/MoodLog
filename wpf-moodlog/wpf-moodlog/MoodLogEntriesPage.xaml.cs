﻿using System;
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
using wpf_moodlog.Model;

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
                    string text = row[3];
                    string date = row[1];
                    string time = row[2];

                    Entry entry = new Entry(text, date, time);

                    entriesStackPanel.Children.Add(entry.BorderedUI);
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

        private void addEntryButton_Click(object sender, RoutedEventArgs e)
        {
            String text = entryTextBox.Text;
            resetEntryTextBox();

            Entry entry = new Entry(text);

            entriesStackPanel.Children.Add(entry.BorderedUI);
        }

        private void resetEntryTextBox()
        {
            entryTextBox.Text = "";
        }
    }
}
