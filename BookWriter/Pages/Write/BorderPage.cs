using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Controls;
using MyBook.BookContent;
using MyBook.Pages.Write;

namespace MyBook
{
  // only for showing
  public class BorderPage : Viewbox
  {
    public PositionDesc Position
    {
      get;
      set;
    }

    public BorderPage()
    {
      Label l = new Label();
      // Take from settings
      l.Content = "Empty";
      this.Child = l;
      Position = new PositionDesc();
    }

    public int FindEnd(BookSource cache)
    {
      this.Child = new TextBlock();

      //Text = cache.SubString(PositionStart, cache.GetMax(this));
      //if (Text.Length == 0) // or show another paragraph
      //{
      //  return 0;
      //}
      //return PositionStart + SplitText();
      return 0;
    }
  }
}
