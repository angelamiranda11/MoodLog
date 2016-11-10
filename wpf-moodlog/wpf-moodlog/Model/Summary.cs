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
                TextBlock dominant = EntryNoUI();
                StackPanel legend = Emotions.LegendUI;

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

        private TextBlock EntryNoUI()
        {
            return new TextBlock()
            {
                Text = '#' + ID.ToString(),
            };
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
