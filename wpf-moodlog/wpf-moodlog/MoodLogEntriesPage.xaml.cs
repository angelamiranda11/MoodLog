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
    /// Interaction logic for MoodLogEntriesPage.xaml
    /// </summary>
    public partial class MoodLogEntriesPage : Page
    {
        public static DateTime Today { get; }

        public MoodLogEntriesPage()
        {
            InitializeComponent();

            setDateTodayLabel();
            // loadPreviousEntries();
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
            MoodLogProfilePage moodLogProfilePage = new MoodLogProfilePage();
            this.NavigationService.Navigate(moodLogProfilePage);
        }

        private void addToEntryTextBox(String str)
        {
            entryTextBox.Text += str;
            entryTextBox.Focus();
            entryTextBox.Select(entryTextBox.Text.Length, 0);
        }

        private void hashtagButton_Click(object sender, RoutedEventArgs e)
        {
            addToEntryTextBox("#");
        }

        private void disableAllEmoticonButtonsExcept(Button emoticonButton)
        {
            enableAllEmoticonButtons();
            emoticonButton.Background = Brushes.DarkGray;
        }

        private void enableAllEmoticonButtons()
        {
            Button[] emoticonButtons =
            {
                joyButton,
                sadnessButton,
                disgustButton,
                angerButton,
                surpriseButton,
                fearButton
            };

            foreach(Button emoticonButton in emoticonButtons)
            {
                emoticonButton.Background = null;
            }
        }


        private void showSubEmoticonsPanel(String[] subEmoticonNames)
        {
            resetSubEmoticonsPanel();

            foreach (String subEmoticonName in subEmoticonNames)
            {
                Button subEmoticonButton = new Button();
   
                subEmoticonButton.Content = convertUriToImage(new Uri("Images/" + subEmoticonName + ".png", UriKind.Relative));
                subEmoticonButton.Width = 50;
                subEmoticonButton.Height = 40;
                subEmoticonButton.Background = null;
                subEmoticonButton.BorderThickness = new Thickness(0, 0, 1, 0);
                subEmoticonButton.ToolTip = subEmoticonName.Replace('-', ' ');
                subEmoticonButton.Name = subEmoticonName;
                subEmoticonButton.Click += new RoutedEventHandler(subEmoticonButton_Click);

                subEmoticonsPanel.Children.Add(subEmoticonButton);
            }
        }

        private void subEmoticonButton_Click(object sender, RoutedEventArgs e)
        {
            Button subEmoticonButton = e.Source as Button;
            addToEntryTextBox(":" + subEmoticonButton.Name + ":");
        }

        private Image convertUriToImage(Uri uri)
        {
            BitmapImage bitmapImage = new BitmapImage(uri);
            Image image = new Image();
            image.Source = bitmapImage;

            return image;
        }

        private void resetSubEmoticonsPanel()
        {
            subEmoticonsPanel.Children.Clear();
            subEmoticonsPanel.Background = Brushes.DarkGray;
        }

        private void joyButton_Click(object sender, RoutedEventArgs e)
        {
            disableAllEmoticonButtonsExcept(joyButton);

            String[] joyEmoticonNames = {
                "EmoticonJoy",
                "EmoticonJoy",
                "EmoticonJoy",
                "EmoticonJoy",
                "EmoticonJoy",
                "EmoticonJoy",
                "EmoticonJoy",
            };

            showSubEmoticonsPanel(joyEmoticonNames);
        }

        private void sadnessButton_Click(object sender, RoutedEventArgs e)
        {
            disableAllEmoticonButtonsExcept(sadnessButton);

            String[] sadnessEmoticonNames = {
                "EmoticonSadness",
                "EmoticonSadness",
                "EmoticonSadness",
                "EmoticonSadness",
                "EmoticonSadness",
                "EmoticonSadness",
                "EmoticonSadness",
            };

            showSubEmoticonsPanel(sadnessEmoticonNames);
        }

        private void disgustButton_Click(object sender, RoutedEventArgs e)
        {
            disableAllEmoticonButtonsExcept(disgustButton);

            String[] disgustEmoticonNames = {
                "EmoticonDisgust",
                "EmoticonDisgust",
                "EmoticonDisgust",
                "EmoticonDisgust",
                "EmoticonDisgust",
                "EmoticonDisgust",
                "EmoticonDisgust",
            };

            showSubEmoticonsPanel(disgustEmoticonNames);
        }

        private void angerButton_Click(object sender, RoutedEventArgs e)
        {
            disableAllEmoticonButtonsExcept(angerButton);

            String[] angerEmoticonNames = {
                "EmoticonAnger",
                "EmoticonAnger",
                "EmoticonAnger",
                "EmoticonAnger",
                "EmoticonAnger",
                "EmoticonAnger",
                "EmoticonAnger",
            };

            showSubEmoticonsPanel(angerEmoticonNames);
        }

        private void surpriseButton_Click(object sender, RoutedEventArgs e)
        {
            disableAllEmoticonButtonsExcept(surpriseButton);

            String[] surprisedEmoticonNames = {
                "EmoticonSurprised",
                "EmoticonSurprised",
                "EmoticonSurprised",
                "EmoticonSurprised",
                "EmoticonSurprised",
                "EmoticonSurprised",
                "EmoticonSurprised",
            };

            showSubEmoticonsPanel(surprisedEmoticonNames);
        }

        private void fearButton_Click(object sender, RoutedEventArgs e)
        {
            disableAllEmoticonButtonsExcept(fearButton);

            String[] fearEmoticonNames = {
                "EmoticonFear",
                "EmoticonFear",
                "EmoticonFear",
                "EmoticonFear",
                "EmoticonFear",
                "EmoticonFear",
                "EmoticonFear",
            };

            showSubEmoticonsPanel(fearEmoticonNames);
        }

        private void addEntryButton_Click(object sender, RoutedEventArgs e)
        {
            DockPanel newEntry = new DockPanel();

            newEntry.Background = convertHexToBrush("#ecf0f1");

            TextBlock content = new TextBlock();
            content.Text = entryTextBox.Text;

            newEntry.Children.Add(content);

            entriesStackPanel.Children.Add(newEntry);
        }

        private Brush convertHexToBrush(String hex)
        {
            BrushConverter brushConverter = new BrushConverter();
            Brush brush = (Brush)brushConverter.ConvertFrom(hex);
            brush.Freeze();

            return brush;
        }
    }
}
