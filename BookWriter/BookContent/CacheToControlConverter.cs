using MyBook.Pages.Write.Text;
using MyBook.Pages.Write.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace MyBook.BookContent
{
  public interface CacheToControlConverter
  {
    // what to do when type is tex
    UIElement Resolve(TextParagraph textParagraph);

    // What to do when type is image 
    UIElement Resolve(ImageParagraph imagesParagraph);

    // Default resolve
    UIElement Resolve(object o);
  }
}
