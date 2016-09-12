using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                //sample for loading resources for BoW Algo
                string positiveBank = wpf_moodlog.Properties.Resources.positive_words_no_emval;
                string negativeBank = wpf_moodlog.Properties.Resources.negative_words_no_emval;
                string punctuations = wpf_moodlog.Properties.Resources.punctuations;
                string emoticons = wpf_moodlog.Properties.Resources.emoticons;
                
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
 
            }
        }
    }
}
