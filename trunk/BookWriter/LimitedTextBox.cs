using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Drawing;
using System.Windows.Media;
using System.Globalization;
using System.Windows;
using System.ComponentModel;

namespace MyBook
{
    public interface PageCache
    {
        void Append(String str);
        void Prepend(String str);
        void CheckSize();
        void MakeActive(String str);
    }

    public class LimitedTextBox : TextBox, PageCache
    {
        public static readonly DependencyProperty NextProperty =
    DependencyProperty.Register(
    "Next", typeof(PageCache), typeof(LimitedTextBox));

        public static readonly DependencyProperty PrevProperty =
    DependencyProperty.Register(
    "Prev", typeof(PageCache), typeof(LimitedTextBox));

        public PageCache Next
        {
            get
            {
                return (PageCache)GetValue(NextProperty);
            }
            set
            {
                SetValue(NextProperty, value);
            }
        }

        public PageCache Prev
        {
            get
            {
                return (PageCache)GetValue(PrevProperty);
            }
            set
            {
                SetValue(PrevProperty, value);
            }
        }

        public LimitedTextBox() 
        {
        }

        public virtual void MakeActive(String str)
        {
            Focus();
            CaretIndex = str.Length;
        }

        public virtual void CheckSize()
        {
            if ( LineCount > MaxLines)
            {
                int caretPosition = CaretIndex;
                String added = "";
                // only remove the last few lines. Rest will be saved in data
                for (int i = MaxLines; i < LineCount; i++)
                {
                    String str = GetLineText(i);
                    added += str;
                }
                Text = Text.Substring(0, Text.Length - added.Length);
                // only if caret was at the last place, move to the first box and fill everything possible
                Next.Prepend(added);
                if (caretPosition >= added.Length)
                {
                    Next.MakeActive(added);
                }
                else
                {
                    CaretIndex = caretPosition;
                }
            }
            else
            {
                // System.Diagnostics.Debug.Assert(false);
                // TODO
                // check if you can add something from forward
            }
        }

        public virtual void Append(String str)
        {
            // append to the last text
            Text += str;
        }

        public virtual void Prepend(String str)
        {
            Text = str + Text;
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            CheckSize();
        }
        //private System.Windows.Size MeasureString(string candidate)
        //{
        //    var formattedText = new FormattedText(
        //        candidate,
        //        CultureInfo.CurrentUICulture,
        //        FlowDirection.LeftToRight,
        //        new Typeface(this.FontFamily, this.FontStyle, this.FontWeight, this.FontStretch),
        //        this.FontSize,
        //        System.Windows.Media.Brushes.Black);

        //    return new System.Windows.Size(formattedText.Width, formattedText.Height);
        //}
    }
}
