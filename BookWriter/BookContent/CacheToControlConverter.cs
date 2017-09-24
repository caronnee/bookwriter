using MyBook.Pages.Write.Entity;
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
    Control Resolve(BookParagraph textParagraph, PositionDesc desc);

    // What to do when type is image 
    Control Resolve(ImageParagraph imagesParagraph, PositionDesc desc);

    // Default resolve
    Control Resolve(object o, PositionDesc desc);
  }
}
