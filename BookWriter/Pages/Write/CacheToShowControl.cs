using MyBook.BookContent;
using MyBook.Pages.Write.Imaging;
using MyBook.Pages.Write.Picture;
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

    public Control Resolve(TextParagraph textParagraph)
    {
      WritingBox p = new WritingBox();
      p.IsReadOnly = true;
      return p;
    }

    public Control Resolve(ImageParagraph imagesParagraph)
    {
      ImageBox ibox = new ImageBox();
      //ibox.Content
      return null;
    }

    public Control Resolve(object o)
    {
      return null;
    }
  }
}
