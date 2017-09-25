using MyBook.Pages.Write.Text;
using MyBook.Pages.Write.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace MyBook.BookContent
{
  public interface CacheToControlConverter
  {
    // what to do when type is tex
    Control Resolve(TextParagraph textParagraph);

    // What to do when type is image 
    Control Resolve(ImageParagraph imagesParagraph);

    // Default resolve
    Control Resolve(object o);
  }
}
