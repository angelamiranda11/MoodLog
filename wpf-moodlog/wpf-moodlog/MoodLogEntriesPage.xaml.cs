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

        private void hashtagButton_Click(object sender, RoutedEventArgs e)
        {
            entryTextBox.Text += "#";
            entryTextBox.Focus();
            entryTextBox.Select(entryTextBox.Text.Length, 0);
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


        private void showSubEmoticonsPanel(String[] uris)
        {
            resetSubEmoticonsPanel();

            foreach (String uri in uris)
            {
                Button subEmoticonButton = new Button();

                BitmapImage emoticonBitmap = new BitmapImage(new Uri("Images/" + uri, UriKind.Relative));
                Image subEmoticonImage = new Image();
                subEmoticonImage.Source = emoticonBitmap;
   
                subEmoticonButton.Content = subEmoticonImage;
                subEmoticonButton.Width = 50;
                subEmoticonButton.Height = 40;
                subEmoticonButton.Background = null;
                subEmoticonButton.BorderThickness = new Thickness(0, 0, 1, 0);

                subEmoticonsPanel.Children.Add(subEmoticonButton);
            }
        }

        private void resetSubEmoticonsPanel()
        {
            subEmoticonsPanel.Children.Clear();
            subEmoticonsPanel.Background = Brushes.DarkGray;
        }

        private void joyButton_Click(object sender, RoutedEventArgs e)
        {
            disableAllEmoticonButtonsExcept(joyButton);

            String[] uris = {
                "EmoticonJoy.png",
                "EmoticonJoy.png",
                "EmoticonJoy.png",
                "EmoticonJoy.png",
                "EmoticonJoy.png",
                "EmoticonJoy.png",
                "EmoticonJoy.png",
            };

            showSubEmoticonsPanel(uris);
        }

        private void sadnessButton_Click(object sender, RoutedEventArgs e)
        {
            disableAllEmoticonButtonsExcept(sadnessButton);

            String[] uris = {
                "EmoticonSadness.png",
                "EmoticonSadness.png",
                "EmoticonSadness.png",
                "EmoticonSadness.png",
                "EmoticonSadness.png",
                "EmoticonSadness.png",
                "EmoticonSadness.png",
            };

            showSubEmoticonsPanel(uris);
        }

        private void disgustButton_Click(object sender, RoutedEventArgs e)
        {
            disableAllEmoticonButtonsExcept(disgustButton);

            String[] uris = {
                "EmoticonDisgust.png",
                "EmoticonDisgust.png",
                "EmoticonDisgust.png",
                "EmoticonDisgust.png",
                "EmoticonDisgust.png",
                "EmoticonDisgust.png",
                "EmoticonDisgust.png",
            };

            showSubEmoticonsPanel(uris);
        }

        private void angerButton_Click(object sender, RoutedEventArgs e)
        {
            disableAllEmoticonButtonsExcept(angerButton);

            String[] uris = {
                "EmoticonAnger.png",
                "EmoticonAnger.png",
                "EmoticonAnger.png",
                "EmoticonAnger.png",
                "EmoticonAnger.png",
                "EmoticonAnger.png",
                "EmoticonAnger.png",
            };

            showSubEmoticonsPanel(uris);
        }

        private void surpriseButton_Click(object sender, RoutedEventArgs e)
        {
            disableAllEmoticonButtonsExcept(surpriseButton);

            String[] uris = {
                "EmoticonSurprised.png",
                "EmoticonSurprised.png",
                "EmoticonSurprised.png",
                "EmoticonSurprised.png",
                "EmoticonSurprised.png",
                "EmoticonSurprised.png",
                "EmoticonSurprised.png",
            };

            showSubEmoticonsPanel(uris);
        }

        private void fearButton_Click(object sender, RoutedEventArgs e)
        {
            disableAllEmoticonButtonsExcept(fearButton);

            String[] uris = {
                "EmoticonFear.png",
                "EmoticonFear.png",
                "EmoticonFear.png",
                "EmoticonFear.png",
                "EmoticonFear.png",
                "EmoticonFear.png",
                "EmoticonFear.png",
            };

            showSubEmoticonsPanel(uris);
        }
    }
}
