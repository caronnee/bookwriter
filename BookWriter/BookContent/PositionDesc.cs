using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBook.BookContent
{
  // identifier what we are actually showing
  public class PositionDesc
  {
    public int ChapterId
    {
      get;
      set;
    }

    public int ParagraphId
    {
      get;
      set;
    }

    public int ContentPos
    {
      get;
      set;
    }

    public int Lenght
    {
      get;
      set;
    }

    public void Clear()
    {
      ChapterId = 0;
      Lenght = 0;
      ParagraphId = 0;
      ContentPos = 0;
    }

    bool IsInvalid()
    {
      return Lenght < 0;
    }
  }
}
