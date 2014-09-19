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
        private BookSource _source;
        public BookWrite(String name)
        {
            InitializeComponent();
            SourcePrev.moveHandler += new BorderTextBox.MoveHandler(LeftPrev_moveHandler);
            SourceNext.moveHandler += new BorderTextBox.MoveHandler(RightNext_moveHandler);
            if (name.Length > 0)
            {
                _source = new BookSource(name);
                // load from file
                _source.Load();
            }
        }

        void RightNext_moveHandler(int caret)
        {
            if (caret < 0)
                return;//do nothing
            SourceNext.Move(PageMove.Left);
            SourceText.Move(PageMove.Left);
            SourcePrev.Move(PageMove.Left);
        }

        void LeftPrev_moveHandler(int caret)
        {
            if (caret < 0)
                return;//do nothing
            throw new NotImplementedException();
        }

        private void moveRight_Click(object sender, RoutedEventArgs e)
        {
            RightNext_moveHandler(0);
        }

        private void moveLeft_Click(object sender, RoutedEventArgs e)
        {
            LeftPrev_moveHandler(0);
        }

        private void SaveBook(object sender, RoutedEventArgs e)
        {
            // export to XML format. DTD
        }
    }
}
