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
