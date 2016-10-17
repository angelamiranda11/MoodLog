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

        private void joyButton_Click(object sender, RoutedEventArgs e)
        {
            enableAllMainEmoticonButtonsExcept(joyButton);

            showSubEmoticons(new string[] {
                "EmoticonJoy",
                "EmoticonJoy",
                "EmoticonJoy",
                "EmoticonJoy",
                "EmoticonJoy",
                "EmoticonJoy",
                "EmoticonJoy",
            });
        }

        private void sadnessButton_Click(object sender, RoutedEventArgs e)
        {
            enableAllMainEmoticonButtonsExcept(sadnessButton);

            showSubEmoticons(new string[] {
                "EmoticonSadness",
                "EmoticonSadness",
                "EmoticonSadness",
                "EmoticonSadness",
                "EmoticonSadness",
                "EmoticonSadness",
                "EmoticonSadness",
            });
        }

        private void disgustButton_Click(object sender, RoutedEventArgs e)
        {
            enableAllMainEmoticonButtonsExcept(disgustButton);

            showSubEmoticons(new string[] {
                "EmoticonDisgust",
                "EmoticonDisgust",
                "EmoticonDisgust",
                "EmoticonDisgust",
                "EmoticonDisgust",
                "EmoticonDisgust",
                "EmoticonDisgust",
            });
        }

        private void angerButton_Click(object sender, RoutedEventArgs e)
        {
            enableAllMainEmoticonButtonsExcept(angerButton);

            showSubEmoticons(new string[] {
                "EmoticonAnger",
                "EmoticonAnger",
                "EmoticonAnger",
                "EmoticonAnger",
                "EmoticonAnger",
                "EmoticonAnger",
                "EmoticonAnger",
            });
        }

        private void surpriseButton_Click(object sender, RoutedEventArgs e)
        {
            enableAllMainEmoticonButtonsExcept(surpriseButton);

            showSubEmoticons(new string[] {
                "EmoticonSurprised",
                "EmoticonSurprised",
                "EmoticonSurprised",
                "EmoticonSurprised",
                "EmoticonSurprised",
                "EmoticonSurprised",
                "EmoticonSurprised",
            });
        }

        private void fearButton_Click(object sender, RoutedEventArgs e)
        {
            enableAllMainEmoticonButtonsExcept(fearButton);

            showSubEmoticons(new string[] {
                "EmoticonFear",
                "EmoticonFear",
                "EmoticonFear",
                "EmoticonFear",
                "EmoticonFear",
                "EmoticonFear",
                "EmoticonFear",
            });
        }

        private void profileButton_Click(object sender, RoutedEventArgs e)
        {
            // View Profile page
            MoodLogProfilePage moodLogProfilePage = new MoodLogProfilePage();
            this.NavigationService.Navigate(moodLogProfilePage);
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

        private void appendToEntryTextBox(String str)
        {
            entryTextBox.Text += str;
            entryTextBox.Focus();
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
            emoticonButton.Background = null;
        }

        private void disableEmoticonButton(Button emoticonButton)
        {
            emoticonButton.Background = Brushes.DarkGray;
        }

        private void showSubEmoticons(String[] subEmoticonNames)
        {
            resetSubEmoticonsPanel();

            foreach (String subEmoticonName in subEmoticonNames)
            {
                Button subEmoticonButton = createSubEmoticonButtonWithName(subEmoticonName);

                subEmoticonsPanel.Children.Add(subEmoticonButton);
            }
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
            subEmoticonButton.Background = null;
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

        private void resetSubEmoticonsPanel()
        {
            subEmoticonsPanel.Children.Clear();
            subEmoticonsPanel.Background = Brushes.DarkGray;
        }

        private Brush convertHexToBrush(String hex)
        {
            BrushConverter brushConverter = new BrushConverter();
            Brush brush = (Brush)brushConverter.ConvertFrom(hex);
            brush.Freeze();

            return brush;
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

    }
}
