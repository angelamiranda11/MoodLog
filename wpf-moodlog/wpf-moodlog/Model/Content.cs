using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace wpf_moodlog.Model
{
    class Content
    {
        private string Text;

        public Content(string Text)
        {
            this.Text = Text;
            this.UI = new TextBlock();

            initUIProperties();
        }

        TextBlock _UI;
        public TextBlock UI
        {
            get
            {
                return this._UI;
            }
            private set
            {
                this._UI = value;
            }
        }

        private void initUIProperties()
        {
            _UI.Margin = new Thickness(5);
            Console.Write(Text);
            _UI.Text = getUnicodeOfString(Text);
            _UI.TextWrapping = TextWrapping.WrapWithOverflow;
        }

        private string getUnicodeOfString(string text)
        {
            byte[] unicodeBytes = Encoding.Unicode.GetBytes(text);

            return Encoding.Unicode.GetString(unicodeBytes);
        }

    }
}
