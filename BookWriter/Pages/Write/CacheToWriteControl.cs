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

    public Control Resolve(BookParagraph textParagraph, PositionDesc desc)
    {
      WritingBox p = new WritingBox();
      p.IsReadOnly = false;
      p.Text = textParagraph.Content.ToString();
      desc.ContentPos += p.Text.Length;
      return p;
    }

    public Control Resolve(ImageParagraph imagesParagraph, PositionDesc desc)
    {
      Label l = new Label();
      l.Content = imagesParagraph;
      return l;
    }

    public Control Resolve(object o, PositionDesc desc)
    {
      throw new NotImplementedException();
    }
  }
}
