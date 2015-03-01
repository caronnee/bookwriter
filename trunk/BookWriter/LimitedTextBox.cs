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

    public void UpdatePageContent(BookSource source, int len)
    {
      Text = source.SubString(PositionStart, PositionStart + len);
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
  }

  // identifier what we are actually showing
  struct PositionDesc
  {
    int chapter;
    int paragraph;
    int positionInParagraph;
    bool IsZero()
    {
      return (chapter == 0) && (paragraph == 0) && (positionInParagraph == 0);
    }
  }

  // the replacement of the original record
  struct UpdateRecord
  {
    StringBuilder newContent;
    PositionDesc desc;
  }

  // this will be shown when there is some text to be written
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
      RecalculateMode = false;
    }

    public BookSource Cache
    {
      get;
      set;
    }

    void UpdateCache(int start, int end, StringBuilder text)
    {
      Cache.Remove(start, end);
      Cache.Insert(start, text);
    }

    void UpdatePageContent()
    {
      Prev.UpdatePageContent(Cache, PositionStart - Prev.PositionStart);
      UpdatePageContent(Cache, Next.PositionStart - PositionStart);
      Next.UpdatePageContent(Cache, Cache.GetMax(this));
    }

    public bool RecalculateMode
    {
      get;
      set;
    }

    public void Start()
    {
      PositionStart = 0;
      RecalculateMode = true;
      int maxLen = Cache.GetMax(this);
      Text = Cache.SubString(0, maxLen);
      RecalculateMode = false;
    }

    // next page will be shown in the box
    public void NextPage()
    {
      int end = Next.FindEnd(Cache);
      if (end > 0)
      {
        Prev.PositionStart = PositionStart;
        PositionStart = Next.PositionStart;
        Next.PositionStart = end;
      }
      UpdatePageContent();
    }

    public void LastPage()
    {
      // if this is this is the last position, do not the anything
      if (Next.PositionStart <= 0)
        return;
      PositionStart = Cache.GetProbableLastPosition();
      Next.PositionStart = PositionStart; // TODO ugly
      Next.PositionStart = Next.FindEnd(Cache);
      Prev.PositionStart = PositionStart;
      Prev.SetPrevious(Cache);
      UpdatePageContent();
    }

    public void PreviousPage()
    {
      if (PositionStart == 0)
        return;
      Next.PositionStart = PositionStart;
      PositionStart = Prev.PositionStart;
      Prev.SetPrevious(Cache);
      UpdatePageContent();
    }

    protected override void OnTextChanged(TextChangedEventArgs e)
    {
      base.OnTextChanged(e);

      // several steps:
      // check size of we need to recalculate start/end
      bool needRecalc = LineCount > MaxLines;
      if (needRecalc)
      {
        int newStart = SplitText();
        if (RecalculateMode == false) // updating automatically, no new content was added, do not update the cache
        {
          UpdateCache(PositionStart, Next.PositionStart, new StringBuilder(Text));
        }
        Next.PositionStart = PositionStart + newStart;
        int newPosition = CaretIndex;
        if (CaretIndex > newStart)
        {
          newPosition = Text.Length - newStart;
          NextPage();
        }
        else
        {
          UpdatePageContent();
        }
        CaretIndex = newPosition;
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
