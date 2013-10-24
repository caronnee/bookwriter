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

namespace MyBook
{
    /// <summary>
    /// Interaction logic for BookViewPort.xaml
    /// </summary>
    public partial class BookViewPort : Window
    {
        public BookViewPort()
        {
            Intro intro = new Intro();
            intro.LoadBook += LoadBook;
            InitializeComponent();
            Content = intro;
        }
        public void LoadBook(String str)
        {
            Content = new BookRead();
        }
    }
}
