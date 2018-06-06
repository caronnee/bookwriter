using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MyBook.Pages.Write.Bookmark
{
  public class BookmarksHeader
  {
    public BookmarksHeader()
    {

    }
    public string Name { get; set; }
    public string Content { get; set; }
    public List<BookmarksHeader> Bookmarks { get; set; }
  }
}
