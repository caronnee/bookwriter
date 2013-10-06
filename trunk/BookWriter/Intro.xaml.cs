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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyBook
{
    /// <summary>
    /// Interaction logic for Intro.xaml
    /// </summary>
    public partial class Intro : UserControl
    {
        public Intro()
        {
            InitializeComponent();
        }

        public delegate void LoadHandler(String str);
        public event LoadHandler LoadBook;

        private void Load(object sender, RoutedEventArgs e)
        {
            if (LoadBook != null)
            {
                Control ctrl = sender as Control;
                LoadBook(ctrl.Name); // name of the book
            }
        }
    }
}
