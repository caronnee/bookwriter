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
    /// Interaction logic for BookWrite.xaml
    /// </summary>
    public partial class BookWrite : UserControl
    {
        public BookWrite()
        {
            InitializeComponent();
            LeftPrev.moveHandler += new BorderTextBox.MoveHandler(LeftPrev_moveHandler);
            RightNext.moveHandler += new BorderTextBox.MoveHandler(RightNext_moveHandler);
        }

        void RightNext_moveHandler(int caret)
        {
            LeftPrev.Text = RightBox.Text;
            RightBox.Text = ""; //TODO load new content
            LeftBox.Text = RightNext.Text;
            RightNext.Text = "";//TODO load next
            LeftBox.Focus();
            LeftBox.CaretIndex = caret;
        }

        void LeftPrev_moveHandler(int caret)
        {
            throw new NotImplementedException();
        }
    }
}
