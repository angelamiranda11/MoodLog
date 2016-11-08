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
        private int ID;
        private string Text;
        private DateTime DateAndTime;
        private Emotions Emotions;

        public Entry(int id, string Text, DateTime DateAndTime, Emotions Emotions)
        {
            this.ID = id;
            this.Text = Text;
            this.DateAndTime = DateAndTime;
            this.Emotions = Emotions; // temporary code until computed emotions cannot yet be written to csv
            this.UI = new StackPanel();
        }

        public Entry(string Text)
        {
            this.ID = 0;
            this.Text = Text;
            this.DateAndTime = DateTime.Now;
            this.Emotions = new Emotions(Text);
            this.UI = new StackPanel();
        }

        StackPanel _UI;
        private StackPanel UI
        {   
            get 
            {
                Summary summary = new Summary(this.DateAndTime, this.Emotions);
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

        // Temporary method for extracting emotion values
        public void writeToNewCsv()
        {
            string path = Global.Path;
            string filename = "entries_" + Global.User.ID + "_temp.csv";

            string fullPath = Path.Combine(path, filename);

            File.AppendAllText(fullPath, this.ToString() + Environment.NewLine);
        }

        public void WriteToCsv()
        {
            using (StreamWriter w = File.AppendText(Global.Path + Global.User.EntriesFilename))
            {
                w.WriteLine(this.ToString());
            }
        }

        public override string ToString()
        {
            List<string> thisRow = new List<string>();

            thisRow.Add(Convert.ToString(ID));
            thisRow.Add(DateAndTime.Year.ToString());
            thisRow.Add(DateAndTime.Month.ToString());
            thisRow.Add(DateAndTime.Day.ToString());
            thisRow.Add(DateAndTime.Hour.ToString());
            thisRow.Add(DateAndTime.Minute.ToString());

            thisRow.Add('\"' + Text + '\"');

            float[] values = Emotions.Values;
            foreach (float value in values)
            {
                thisRow.Add(value.ToString());
            }

            return String.Join(",", thisRow);
        }
    }
}
