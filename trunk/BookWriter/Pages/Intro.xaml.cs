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
            ShelfBook b = new ShelfBook();
            Style s = TryFindResource("Rotated") as Style;
            b.Filename = name;
            b.Style = s;
            b.Content = name;
            b.Click +=new RoutedEventHandler(Load);
            return b;
        }

        public Intro()
        {
            InitializeComponent();
            // TODO change this to dynamically load the books
            string name = TestHelper.CreateDummyBook();
            Button b = CreateBookControl(name);
            this.Shelf.Children.Add(b);
        }

        public delegate void LoadHandler(String str, int flags);
        public event LoadHandler LoadBook;

        public delegate void SettingsPage();
        public event SettingsPage OnSettingsPage;

        private void Load(object sender, RoutedEventArgs e)
        {
            if (LoadBook != null)
            {
                ShelfBook ctrl = sender as ShelfBook;
                int typeFlags = writeButton.IsChecked == true ? 1:0;
                LoadBook(ctrl.Filename,typeFlags); // name of the book
            }
        }

        private void settingButton_Click(object sender, RoutedEventArgs e)
        {
          if (OnSettingsPage != null)
          {
            ShelfBook ctrl = sender as ShelfBook;
            int typeFlags = writeButton.IsChecked == true ? 1 : 0;
            OnSettingsPage(); // name of the book
          }
        }
    }
}
