using MyBook.BookContent;
using MyBook.Pages.Write.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBook.Pages.Write
{
  public class ControlToCacheConverter
  {
    public IContent Create(WritingBox box)
    {
      if (box.Text.Length == 0)
        return null;
      TextParagraph paragraph = new TextParagraph();
      paragraph.Content = box.Text.ToString();
      return paragraph;
    }
  }
}
