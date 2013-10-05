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
        PagesDone left;
        PagesDone right;

        public BookViewPort()
        {
            left = new PagesDone(); // TODO load
            right = new PagesDone();//incompleted pages
            InitializeComponent();
        }
        public PagesDone LeftSource
        {
            get
            {
                return left;
            }
        }

        public PagesDone RightSource
        {
            get
            {
                return right;
            }
        }

        //private void CheckLines(object sender, TextChangedEventArgs e)
        //{
        //     s = sender as TextBox;
        //    if (s.LineCount > s.MaxLines)
        //    {
        //        int caretPosition = s.CaretIndex;
        //        // get content of the next lines that are not visible
        //        int index = s.GetLastVisibleLineIndex() + 1;
        //        String rest = s.GetLineText(index);
        //        s.Text = s.Text.Substring(0, s.Text.Length - rest.Length);
        //        RightBox.Text = rest + RightBox.Text;
        //        if (caretPosition >= s.Text.Length)
        //        {
        //            // if the caret was at position that was cut
        //            RightBox.Focus(); // set caret position appropriately, after the word inserted
        //            RightBox.CaretIndex = rest.Length;
        //        }
        //        else
        //        {
        //            s.CaretIndex = caretPosition;
        //        }
        //    }
        //    else
        //    {
        //        // only if total length was removed
        //    }
        //}

        //private void CheckForward(object sender, TextChangedEventArgs e)
        //{
        //    PageCache box = sender as PageCache;
        //    box.CheckSize();
        //}
    }
}
