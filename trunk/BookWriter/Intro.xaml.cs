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
        private Button CreateBookControl(string name)
        {
            Button b = new Button();
            Style s = TryFindResource("Rotated") as Style;
            b.Style = s;
            b.Content = name;

            b.Click +=new RoutedEventHandler(Load);
            return b;
        }

        public Intro()
        {
            InitializeComponent();
            string name = TestHelper.CreateDummyBook();
            Button b = CreateBookControl(name);
            this.Shelf.Children.Add(b);
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
