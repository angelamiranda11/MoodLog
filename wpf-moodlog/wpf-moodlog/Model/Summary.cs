using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Media;

namespace wpf_moodlog.Model
{
    class Summary
    {
        private string Text;
        private string Date;
        private string Time;
        private bool IsNewEntry;

        public Summary(string Text, string Date, string Time, bool IsNewEntry)
        {
            this.Text = Text;
            this.Date = Date;
            this.Time = Time;
            this.IsNewEntry = IsNewEntry;
            this.UI = new DockPanel();

            initUIProperties();
        }

        DockPanel _UI;
        public DockPanel UI
        {
            get
            {
                Emotions emotions = new Emotions(this.Text, this.IsNewEntry);

                PieSeries chart = emotions.ChartUI;
                TextBlock dateTime = DateTimeUI();
                TextBlock dominant = emotions.DominantUI;
                StackPanel legend = emotions.LegendUI;

                DockPanel.SetDock(chart, Dock.Left);
                DockPanel.SetDock(dateTime, Dock.Top);
                DockPanel.SetDock(dominant, Dock.Top);
                DockPanel.SetDock(legend, Dock.Top);

                addToUIChildren(chart, dateTime, dominant, legend);

                return this._UI;
            }
            private set
            {
                this._UI = value;
            }
        }

        private void initUIProperties()
        {
            _UI.Background = convertToBrush("#ecf0f1");
        }

        private TextBlock DateTimeUI()
        {
            TextBlock ui = new TextBlock();

            ui.Text = this.Date + " " + this.Time;

            return ui;
        }

        private void addToUIChildren(PieSeries chart, TextBlock dateTime, TextBlock dominant, StackPanel legend)
        {
            _UI.Children.Add(chart);
            _UI.Children.Add(dateTime);
            _UI.Children.Add(dominant);
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
