using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace wpf_moodlog.Model
{
    public class Entry
    {
        private string Text;
        private DateTime DateTime;
        private Emotions Emotions;

        public Entry(string Text, DateTime DateTime, Emotions Emotions)
        {
            this.Text = Text;
            this.DateTime = DateTime;
            this.Emotions = new Emotions(Text); // temporary code until computed emotions cannot yet be written to csv
            this.UI = new StackPanel();
        }

        public Entry(string Text)
        {
            this.Text = Text;
            this.DateTime = DateTime.Now;
            this.Emotions = new Emotions(Text);
            this.UI = new StackPanel();
        }

        StackPanel _UI;
        private StackPanel UI
        {   
            get 
            {
                Summary summary = new Summary(this.DateTime, this.Emotions);
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

        public void writeToCsv()
        {
            Stream entries = getEntriesStream();

            using (CsvFileWriter writer = new CsvFileWriter(entries))
            {
                for (int i = 0; i < 100; i++)
                {
                    CsvRow row = new CsvRow();

                    row.Add("");
                    row.Add(DateTime.ToString("yyyy"));
                    row.Add(DateTime.ToString("M"));
                    row.Add(DateTime.ToString("d"));
                    row.Add(DateTime.ToString("H"));
                    row.Add(DateTime.ToString("m"));
                    row.Add(Text);

                    float[] values = Emotions.Values;

                    foreach (float value in values)
                    {
                        row.Add(value.ToString());
                    }

                    writer.WriteRow(row);
                }
            }
        }

        public Stream getEntriesStream()
        {
            string filename = Global.User.Entries;

            return Assembly.GetExecutingAssembly().GetManifestResourceStream("wpf_moodlog.Data." + filename + ".csv");
        }
    }
}
