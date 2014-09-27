using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Controls;

namespace MyBook
{
    // only for showing
    public class BorderTextBox : PageCache
    {
        public BorderTextBox()
        {
        }

        public void SetPrevious(BookSource cache)
        {
            Text = cache.SubString(0, PositionStart);
            StringBuilder str = new StringBuilder();
            int lines = Math.Min(LineCount, MaxLines);
            for (int i = 0; i < lines; i++)
            {
                 str.Append( GetLineText(lines - i) );
            }
            PositionStart -= str.Length;
        }

        public int FindEnd( BookSource cache )
        {
            Text = cache.SubString(PositionStart, cache.GetMax(this));
            if (Text.Length == 0) // or show another paragraph
            {
                return -1;
            }
            return PositionStart + SplitText();
        }
    }
}
