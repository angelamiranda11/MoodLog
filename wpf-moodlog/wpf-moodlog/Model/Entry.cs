using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace wpf_moodlog.Model
{
    public class Entry
    {
        private static DateTime thisDay = DateTime.Now;

        private string Text;
        private string Date;
        private string Time;
        private bool IsNewEntry;

        public Entry(string Text, string Date, string Time)
        {
            this.Text = Text;
            this.Date = Date;
            this.Time = Time;
            this.IsNewEntry = false;
            this.UI = new StackPanel();
        }

        public Entry(string Text)
        {
            this.Text = Text;
            this.Date = DateToday();
            this.Time = TimeToday();
            this.IsNewEntry = true;
            this.UI = new StackPanel();
        }

        private string DateToday()
        {
            return thisDay.ToString("dddd, MMMM dd,");
        }

        private string TimeToday()
        {
            return thisDay.ToString("h:mm tt");
        }

        StackPanel _UI;
        private StackPanel UI
        {   
            get 
            {
                Summary summary = new Summary(this.Text, this.Date, this.Time, this.IsNewEntry);
                Content content = new Content(this.Text);

                addToUIChildren(summary.UI, content.UI);

                return this._UI;
            }
            set
            {
                this._UI = value;
            }
        }

        private void addToUIChildren(DockPanel summary, TextBlock content)
        {
            _UI.Children.Add(summary);
            _UI.Children.Add(content);
        }

        public Border BorderedUI
        {
            get
            {
                Border border = new Border()
                {
                    BorderBrush = Brushes.DarkGray,
                    BorderThickness = new Thickness(1),
                    LayoutTransform = new RotateTransform(180),
                    Margin = new Thickness(0, 10, 0, 10),
                };

                border.Child = UI;

                return border;
            }
        }
    }
}
