using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MyBook
{
  class PageDesc
  {
    String id;
    String text;
  }
  class BookSource
  {
    private String _filepath; // first go offsets

    // arranged pages
    private ArrayList _pages;

    public void Recompute()
    {
      throw new Exception("Not implemented");
    }
    public BookSource(String name)
    {
      _filepath = name;
      _pages = new ArrayList();
    }
    public int Load()
    {
      StreamReader reader = new StreamReader(_filepath);
      String content = reader.ReadToEnd(); // hopefully this will be very small book....
      string[] s = content.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries );
      _pages = new ArrayList(s.Length);
      _pages.AddRange(s);
      reader.Close();
      return _pages.Count; // how many pages did we loaded
    }

    public void AddPage(String content)
    {
      _pages.Add(content);
    }
    public void Save()
    {
      StreamWriter writer = new StreamWriter(_filepath);
      foreach (string a in _pages)
      {
        writer.Write(a);
        writer.Write('\0');
      }
      writer.Close();
    }
    public String GetPage(int page)
    {
      if (page >= _pages.Count)
        return "End of book";
      return _pages[page] as string;
    }

    // Check if this book can be used
    public bool IsValid(int flags = 0)
    {
      if (System.IO.File.Exists(_filepath))
        return true;
      return false;
    }

  }
}
