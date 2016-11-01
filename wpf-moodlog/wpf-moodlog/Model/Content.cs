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
        private String Text;

        public Content(String Text)
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
            _UI.Text = this.Text;
            _UI.TextWrapping = TextWrapping.WrapWithOverflow;
        }
    }
}
