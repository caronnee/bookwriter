using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Minis
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void CheckSize(object sender, TextChangedEventArgs e)
        {
            
        }

        private void checkSize(object sender, TextCompositionEventArgs e)
        {
            RichTextBox box = sender as RichTextBox;
            double diff = box.ActualHeight - box.ExtentHeight;
            if (diff < 0)
            {
                Keyboard.Focus(RightBox);
                //RightBox.Focus();
            }
        }
    }
}
