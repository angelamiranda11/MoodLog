using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Media;

namespace wpf_moodlog.Model
{
    class Summary
    {
        private int ID;
        private DateTime DateTime;
        private Emotions Emotions;

        public Summary(int ID, DateTime DateTime, Emotions Emotions)
        {
            this.ID = ID;
            this.DateTime = DateTime;
            this.Emotions = Emotions;
            this.UI = new DockPanel();

            initUIProperties();
        }

        DockPanel _UI;
        public DockPanel UI
        {
            get
            {
                PieSeries chart = Emotions.ChartUI;
                TextBlock dateTime = DateTimeUI();
                Button showComputation = ShowComputationUI();
                StackPanel legend = Emotions.LegendUI;

                DockPanel.SetDock(chart, Dock.Left);
                DockPanel.SetDock(dateTime, Dock.Top);
                DockPanel.SetDock(showComputation, Dock.Top);
                DockPanel.SetDock(legend, Dock.Top);

                addToUIChildren(chart, dateTime, showComputation, legend);

                return this._UI;
            }
            private set
            {
                this._UI = value;
            }
        }

        private Button ShowComputationUI()
        {
            Button button = new Button()
            {
                Content = "Show computation of entry #" + ID.ToString(),
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0,5,0,5),
                Padding = new Thickness(1),
                Width = 195,
                Name = "showComputation" + ID,
            };

            button.Click += new RoutedEventHandler(showComputationButton_Click);

            return button;
        }

        private void showComputationButton_Click(object sender, RoutedEventArgs e)
        {
            Global.EntriesPage.computationTextBox.Text = "";

            Button showComputationButton = e.Source as Button;

            int buttonId = Convert.ToInt32(showComputationButton.Name.Remove(0, "showComputation".Length));

            User user = Global.User;
            using (CsvFileReader reader = new CsvFileReader(Global.GetStreamOf(user.EntriesFilename, FileMode.Open)))
            {
                CsvRow row = new CsvRow();
                while (reader.ReadRow(row))
                {
                    int rowId = getIdFrom(row);
                    
                    if (rowId == buttonId)
                    {
                        string rowText = getTextFrom(row);
                        Program program = new Program();

                        program.processText(rowText);
                    }
                }
            }
        }

        private string getTextFrom(CsvRow row)
        {
            return row[6];
        }

        private int getIdFrom(CsvRow row)
        {
            return Convert.ToInt32(row[0]);
        }

        private void initUIProperties()
        {
            _UI.Background = convertToBrush("#ecf0f1");
        }

        private TextBlock DateTimeUI()
        {
            TextBlock ui = new TextBlock();

            ui.Text = DateTime.ToString("dddd, MMMM d, h:mm tt");

            return ui;
        }

        private void addToUIChildren(PieSeries chart, TextBlock dateTime, Button showComputation, StackPanel legend)
        {
            _UI.Children.Add(chart);
            _UI.Children.Add(dateTime);
            _UI.Children.Add(showComputation);
            _UI.Children.Add(legend);
        }

        private SolidColorBrush convertToBrush(String hex)
        {
            BrushConverter brushConverter = new BrushConverter();
            SolidColorBrush brush = (SolidColorBrush)brushConverter.ConvertFrom(hex);
            brush.Freeze();

            return brush;
        }
    }
}
