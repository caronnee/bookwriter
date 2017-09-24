using MyBook.BookContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBook.Pages.Write
{
  interface IWrite
  {
    IContent Create();
  }
}
