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
  public class WorkingPage : Viewbox
  {
    public WorkingPage()
    {
      Position = new PositionDesc();
    }

    public IContent Create()
    {
      // TODO through dependency
      IWrite c = Child as IWrite;
      return c.Create();
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
    
    public PositionDesc Position
    {
      get;
      set;
    }

    public void Load(BookSource Cache)
    {
      IContent content = Cache.GetContent(Position);
      if (content != null)
        this.Child = content.Show(Converter, Position);
    }

    public void UpdateCache()
    {
      //UpdateCache(PositionStart, Next.PositionStart, new StringBuilder(Text));
    }

    void UpdateCache(int start, int end, StringBuilder text)
    {
      //Cache.Remove(start, end);
      //Cache.Insert(start, text);
    }

    void UpdatePageContent()
    {
      //Prev.UpdatePageContent(Cache, PositionStart - Prev.PositionStart);
      //UpdatePageContent(Cache, Next.PositionStart - PositionStart);
      //Next.UpdatePageContent(Cache, Cache.GetMax(this));
    }

    public void Start()
    {
      //Next.PositionStart = Next.FindEnd(Cache);
      //NextPage();
    }

    // next page will be shown in the box
    public void NextPage()
    {
      //int end = Next.FindEnd(Cache);
      //if (end > 0)
      //{
      //  Prev.PositionStart = PositionStart;
      //  PositionStart = Next.PositionStart;
      //  Next.PositionStart = end;
      //}
      UpdatePageContent();
    }

    public void LastPage()
    {
      // if this is this is the last position, do not the anything
      //if (Next.PositionStart <= 0)
      //  return;
      //PositionStart = Cache.GetProbableLastPosition();
      //Next.PositionStart = PositionStart; // TODO ugly
      //Next.PositionStart = Next.FindEnd(Cache);
      //Prev.PositionStart = PositionStart;
      //Prev.SetPrevious(Cache);
      //UpdatePageContent();
    }

    // set up the cache so it would show the next chapter
    public void NextChapter()
    {
      //Prev.PositionStart = Next.PositionStart = PositionStart = 0;
      //Cache.NextChapter();
      //NextPage();
      //UpdatePageContent();
    }

    // set up the cache so it would show the next chapter
    public void PreviousChapter()
    {
      //Prev.PositionStart = Next.PositionStart = PositionStart = 0;
      //Cache.PreviousChapter();
      //NextPage();
      //UpdatePageContent();
    }

    public void PreviousPage()
    {
      //if (PositionStart == 0)
      //  return;
      //Next.PositionStart = PositionStart;
      //PositionStart = Prev.PositionStart;
      //Prev.SetPrevious(Cache);
      //UpdatePageContent();
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {   
      base.OnKeyUp(e);   
      /* if (e.Key != Key.Back && e.Key != Key.Delete )
        return;
     
      if (e.Key == Key.Back && CaretIndex == 0 )
      {
          PreviousPage();
          CaretIndex = Text.Length;
          return;
      }

      if ((e.Key == Key.Delete) && (CaretIndex == Text.Length))
      {
        NextPage();
        return;
      }

      UpdateCache();
      Next.PositionStart = PositionStart;
      Next.PositionStart = Next.FindEnd(Cache);
      int oldcaret = CaretIndex;
      UpdatePageContent(Cache, Next.PositionStart - PositionStart);
      Next.UpdatePageContent(Cache, Cache.GetMax(this));
      CaretIndex = oldcaret;
       */
    }

    
  }
}
