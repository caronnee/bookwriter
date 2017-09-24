using MyBook.BookContent;
using MyBook.Pages.Write.Entity;
using MyBook.Pages.Write.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace MyBook.Pages.Write
{
  public class CacheToWriteControl : CacheToControlConverter
  {
    public Control Resolve(BookParagraph textParagraph)
    {
      WritingBox p = new WritingBox();
      p.IsReadOnly = false;
      p.Text = textParagraph.Content.ToString();
      return p;
    }

    public Control Resolve(ImageParagraph imagesParagraph)
    {
      Label l = new Label();
      l.Content = imagesParagraph;
      return l;
    }

    public Control Resolve(object o)
    {
      throw new NotImplementedException();
    }
  }
}
