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
    /// Interaction logic for BookRead.xaml
    /// </summary>
    public partial class BookRead : UserControl
    {
      private string _name;

        public BookRead(String name)
        {
          _name = name; // TODO check the correct syntax
            InitializeComponent();
            Loaded += BookRead_Loaded;
        }

        private String GetPage(int page)
        {
          return "xxxx";
        }
        void BookRead_Loaded(object sender, RoutedEventArgs e)
        {
          // fill first few pages
          BookPageLeft.Content = GetPage(0);
          BookPageRight.Content = GetPage(1);
        }
        // Check if this book can be used
        public bool IsValid( int flags = 0)
        {
          if (System.IO.File.Exists("../../resources/dummyContent.book"))
            return true;
          return false;
        }
    }
}
