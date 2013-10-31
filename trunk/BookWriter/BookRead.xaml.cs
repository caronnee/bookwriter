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
        private SourceText _source;

        public BookRead(String name)
        {
            _source = new SourceText(name) ; // TODO check the correct syntax
            _source.Load();
            InitializeComponent();
            Loaded += BookRead_Loaded;
        }
        public bool IsValid()
        {
          return _source.IsValid();
        }
        void BookRead_Loaded(object sender, RoutedEventArgs e)
        {
          // fill first few pages
          BookPageLeft.Content = _source.GetPage(0);
          BookPageRight.Content = _source.GetPage(1);
        }
    }
}
