using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using MyBook.BookContent;

namespace MyBook
{
  public class BookSource
  {
    private String _filepath; // first go offsets
    
    public int ParagaphNumber
    {
        get;
        set;
    }

    public int GetLastPosition()
    {
        // TODO to this properly
        return Paragraphs[ParagaphNumber].Content.Length - 10;
    }

    bool IsEndOfParagraph(int pos)
    {
        return true;
    }

    public int GetMax( PageCache source )
    {
        // maximum for page. TODO calculate this according to source parameters
        return 1048;
    }
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
      ParagaphNumber = 0;
    }

    public List<BookContent.BookParagraph> Paragraphs
    {
        get;
        set;
    }

    public string SubString(int start, int end)
    {
        if (end >= Paragraphs[ParagaphNumber].Content.Length)
            end = Paragraphs[ParagaphNumber].Content.Length-1;
        return Paragraphs[ParagaphNumber].Content.ToString(start, end - start);
    }

    public void Remove(int start, int end)
    {
        Paragraphs[ParagaphNumber].Content.Remove(start, end - start);
    }

    public void Insert ( int where, StringBuilder content)
    {
        Paragraphs[ParagaphNumber].Content.Insert(where, content );
    }
    
    public void NextChapter()
    {
        // TODO
    }
    public int Load()
    {
        Paragraphs = new List<BookParagraph>();
        // read the sample xml
        XmlDocument doc = new XmlDocument();
        doc.Load(_filepath);
        XmlNodeList list = doc.SelectNodes("BookContent");
        list = list[ParagaphNumber].SelectNodes("Chapter");
        for (int i = 0; i < list.Count; i++)
        {
            XmlNode a = list[i];
            XmlNodeList paragraphs = a.SelectNodes("Content");
            for (int p = 0; p < paragraphs.Count; p++)
            {
                BookParagraph par = new BookParagraph();
                StringBuilder trim = new StringBuilder( paragraphs[p].InnerText );
                trim = trim.Replace("\r", "");
                trim = trim.Replace("\n", "");
                trim = trim.Replace("\t", "");
                par.Content = trim;
                if ( par.Content.Length > 0 )
                    Paragraphs.Add(par);
            }
        }
        return Paragraphs.Count;
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
