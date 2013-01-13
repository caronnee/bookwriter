using MyBook.Pages.Write.Bookmark;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MyBook.Pages.Write.DataWriteContext
{
  public class DataWriteContext : INotifyPropertyChanged
  {   
    private List<IBookmark> _bookmarks;
    public List<IBookmark> Bookmarks
    {
      get { return _bookmarks; }
      set
      {
        _bookmarks = value;
        NotifyPropertyChanged("Bookmarks");
      }
    }

    void NotifyPropertyChanged(string property)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(property));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public DataWriteContext()
    {
      _bookmarks = new List<IBookmark>();
    }

    // based on book
    public void Init(string filename)
    {
      _bookmarks.Add(new BookmarksHeader { Name = "Locations", Content = "Something else" });
      _bookmarks.Add(new BookmarksHeader {
        Name = "Characters",
        Content = "Something else",
        Bookmarks = new List<IBookmark>() {
          new BookmarksHeader { Name = "Relicts2", Content = "Something else" },
          new BookmarksHeader { Name = "Relicts2", Content = "Something else" },
          new BookmarksHeader { Name = "Relicts2", Content = "Something else" },
          new BookmarksHeader { Name = "Relicts2", Content = "Something else" }
        }
      });
      _bookmarks.Add(new BookmarksHeader
      {
        Name = "Relicts",
        Content = "Something else",
        Bookmarks = new List<IBookmark>() {
          new BookmarksHeader { Name = "Relicts2", Content = "Something else" },
          new BookmarksHeader { Name = "Relicts2", Content = "Something else" },
          new BookmarksHeader { Name = "Relicts2", Content = "Something else" },
          new BookmarksHeader { Name = "Relicts2", Content = "Something else" }
        }
      });

      _bookmarks.Add(new BookmarksHeader { Name = "History", Content = "Something else" });
      _bookmarks.Add(new BookmarksHeader { Name = "Timeline", Content = "Something else" });
    }
  }
}
