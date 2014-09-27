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
using System.Xml;

namespace MyBook
{
    /// <summary>
    /// Interaction logic for BookWrite.xaml
    /// </summary>
    public partial class BookWrite : UserControl
    {
        public BookWrite(String name)
        {
            InitializeComponent();
            if (name.Length > 0)
            {
                SourceText.Cache = new BookSource(name);
                // load from file
                SourceText.Cache.Load();
                //SourceText.Start();
            }
        }

        private void SaveBook(object sender, RoutedEventArgs e)
        {
            // export to XML format. DTD
        }

        private void startPage_Click(object sender, RoutedEventArgs e)
        {
            SourceText.Start();
        }

        private void BookWriter_Loaded(object sender, RoutedEventArgs e)
        {
            SourceText.Start();
        }

        private void nextPage_Click(object sender, RoutedEventArgs e)
        {
            SourceText.NextPage();
        }
    }
}
