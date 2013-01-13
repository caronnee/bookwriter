using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MyBook.Pages.Write.Bookmark
{
  public interface IBookmark
  {
    string Name { get; }
    string Content{ get; }
    List<IBookmark> Bookmarks { get; }
  }

  public class BookmarksHeader :  IBookmark
  {
    public BookmarksHeader()
    {

    }
    public string Name { get; set; }
    public string Content { get; set; }
    public List<IBookmark> Bookmarks { get; set; }
  }
}
