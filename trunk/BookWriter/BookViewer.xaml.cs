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
        private String next;
        private String previous;

        public BookViewPort()
        {
            InitializeComponent();
        }
        
        private void CheckLines(object sender, TextChangedEventArgs e)
        {
            TextBox s = sender as TextBox;
            if (s.LineCount > s.MaxLines)
            {
                int caretPosition = s.CaretIndex;
                // get content of the next lines that are not visible
                int index = s.GetLastVisibleLineIndex() + 1;
                String rest = s.GetLineText(index);
                s.Text = s.Text.Substring(0, s.Text.Length - rest.Length);
                RightBox.Text = rest + RightBox.Text;
                if (caretPosition >= s.Text.Length)
                {
                    // if the caret was at position that was cut
                    RightBox.Focus(); // set caret position appropriately, after the word inserted
                    RightBox.CaretIndex = rest.Length;
                }
                else
                {
                    s.CaretIndex = caretPosition;
                }
            }
            else
            {
                // only if total length was removed
            }
        }

        private void CheckForward(object sender, TextChangedEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box.LineCount > box.MaxLines)
            {
                int caretPosition = box.CaretIndex;
                String added = "";
                // only remove  the last few lines. Rest will be saved in data
                for (int i = box.MaxLines; i < box.LineCount; i++)
                {
                    String str = box.GetLineText(i);
                    added += str;
                }
                box.Text = box.Text.Substring(0, box.Text.Length - added.Length);
                // only if caret was at the last place, move to the first box and fill everything possible
                if (caretPosition >= added.Length)
                {
                    LeftBox.Focus();
                    LeftBox.Text = added;
                    LeftBox.CaretIndex = added.Length;
                    RightBox.Text = ""; // clear the rightbox text
                }
                else
                {
                    box.CaretIndex = caretPosition;
                }
            }
            else
            {
                //TODO
                // check if you can add something from forward
            }
        }
    }
}
