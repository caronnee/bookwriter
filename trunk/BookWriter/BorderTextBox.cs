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

        public void PlusPage( BookSource cache )
        {
            Text = cache.ToString(PositionStart, 1000);
            PositionStart += SplitText();
        }
    }
}
