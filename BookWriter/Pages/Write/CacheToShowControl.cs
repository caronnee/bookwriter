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
  class CacheToShowControl : CacheToControlConverter
  {

    public Control Resolve(BookParagraph textParagraph, PositionDesc desc)
    {
      WritingBox p = new WritingBox();
      p.IsReadOnly = true;
      return p;
    }

    public Control Resolve(ImageParagraph imagesParagraph, PositionDesc desc)
    {
      // TODO label
      return null;
    }

    public Control Resolve(object o, PositionDesc desc)
    {
      return null;
    }
  }
}
