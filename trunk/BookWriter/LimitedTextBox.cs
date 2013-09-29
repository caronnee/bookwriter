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

namespace MyBook
{
    class LimitedTextBox : TextBox
    {
        public LimitedTextBox() 
        {
        }
        private System.Windows.Size MeasureString(string candidate)
        {
            var formattedText = new FormattedText(
                candidate,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface(this.FontFamily, this.FontStyle, this.FontWeight, this.FontStretch),
                this.FontSize,
                System.Windows.Media.Brushes.Black);

            return new System.Windows.Size(formattedText.Width, formattedText.Height);
        }
        public bool IsFull(TextCompositionEventArgs e)
        {
            return false;
        }
    }
}
