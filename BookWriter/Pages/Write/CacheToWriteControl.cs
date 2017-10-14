using MyBook.BookContent;
using MyBook.Pages.Write.Imaging;
using MyBook.Pages.Write.Picture;
using MyBook.Pages.Write.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MyBook.Pages.Write
{
  public class CacheToWriteControl : CacheToControlConverter
  {
    public UIElement Resolve(TextParagraph textParagraph)
    {
      WritingBox p = new WritingBox();
      p.IsReadOnly = false;
      p.Text = textParagraph.Content.ToString();
      return p;
    }

    public UIElement Resolve(ImageParagraph imagesParagraph)
    {
      ImageBox l = new ImageBox();
      l.SetImage(imagesParagraph.SourceName, true);
      return l;
    }

    public UIElement Resolve(object o)
    {
      throw new NotImplementedException();
    }
  }
}
