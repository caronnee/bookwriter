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
    public enum PageMove
    {
        Left,
        Right
    };

    public class PageCache : TextBox
    {
        public int PositionStart
        {
            get;
            set;
        }
        
        public void UpdatePageContent( BookSource source, int len)
        {
            Text = source.SubString(PositionStart, PositionStart + len);
        }

        protected int SplitText( )
        {
            StringBuilder visible = new StringBuilder();
            // only remove the last few lines. Rest will be saved in data
            for (int i = 0; i < MaxLines; i++)
            {
                String str = GetLineText(i);
                visible.Append(str);
            }
            return visible.Length;
        }
    }

    public class LimitedTextBox : PageCache
    {
        public static readonly DependencyProperty NextProperty =
    DependencyProperty.Register(
    "Next", typeof(BorderTextBox), typeof(LimitedTextBox));

        public static readonly DependencyProperty PrevProperty =
    DependencyProperty.Register(
    "Prev", typeof(BorderTextBox), typeof(LimitedTextBox));

        public BorderTextBox Next
        {
            get
            {
                return (BorderTextBox)GetValue(NextProperty);
            }
            set
            {
                SetValue(NextProperty, value);
            }
        }

        public BorderTextBox Prev
        {
            get
            {
                return (BorderTextBox)GetValue(PrevProperty);
            }
            set
            {
                SetValue(PrevProperty, value);
            }
        }

        public LimitedTextBox()
        {
        }

        public BookSource Cache
        {
            get;
            set;
        }


        void UpdateCache( int start, int end, StringBuilder text )
        {
            StringBuilder builder = new StringBuilder(" test");
            builder.Insert(0, text);
            String s = builder.ToString(0, builder.Length);
            Cache.Remove(start, end);
            Cache.Insert(start, text);
            String t = Cache.SubString(0,800);
        }
        void UpdatePageContent()
        {
            Prev.UpdatePageContent(Cache, PositionStart - Prev.PositionStart);
            UpdatePageContent(Cache, Next.PositionStart - PositionStart);
            Next.UpdatePageContent(Cache,1000);
        }
        
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            // several steps:
            // ccke size of we need to recalculate start/end
            bool needRecalc = LineCount > MaxLines;
            if (needRecalc)
            {
                int newStart = SplitText();
                UpdateCache(PositionStart, Next.PositionStart, new StringBuilder(Text));
                Next.PositionStart = PositionStart + newStart;
                if (CaretIndex > newStart)
                {
                    CaretIndex = Text.Length - newStart;
                    Prev.PositionStart = PositionStart;
                    PositionStart = Next.PositionStart;
                    Next.PlusPage(Cache);
                }
                UpdatePageContent();
                CaretIndex = newStart;
            }
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
