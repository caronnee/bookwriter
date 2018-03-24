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
using MyBook.BookContent;
using MyBook.Pages.Write;

namespace MyBook
{
  public enum PageMove
  {
    Left,
    Right
  };

/*  public class PageCache : TextBox
  {

    public void UpdatePageContent(BookSource source, int len)
    {
      //Text = source.SubString(PositionStart, PositionStart + len);
    }

    // split loaded text
    protected int SplitText()
    {
      StringBuilder visible = new StringBuilder();
      // only remove the last few lines. Rest will be saved in data
      int lines = Math.Min(LineCount, MaxLines);
      for (int i = 0; i < lines; i++)
      {
        String str = GetLineText(i);
        visible.Append(str);
      }
      return visible.Length;
    }
  }*/

  // this will be shown when there is some text to be written
  public class WorkingPage : ScrollViewer
  {
    public WorkingPage()
    {

    }

    public IContent Create()
    {
      return null;
    }

    public ControlToCacheConverter CacheConverter
    {
      get;
      set;
    }

    public CacheToControlConverter Converter
    {
      get;
      set;
    }
  }
}
