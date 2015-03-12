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
  // identifier what we are actually showing
  public class PositionDesc
  {
    public int ChapterId
    {
      get;
      set;
    }
    public int ParagraphId
    {
      get;
      set;
    }
    public int ContentPos
    {
      get;
      set;
    }
    bool IsZero()
    {
      return (ChapterId == 0) && (ParagraphId == 0) && (ContentPos == 0);
    }
  }
  public class BookSource
  {
    private String _filepath; // first go offsets

    public PositionDesc SourcePosition
    {
      get;
      set;
    }

    // return the position as is should be 
    public int GetProbableLastPosition()
    {
      // TODO to this properly
      return Paragraphs[SourcePosition.ParagraphId].Content.Length - 10;
    }

    bool IsEndOfParagraph(int pos)
    {
      return true;
    }

    public int GetMax(PageCache source)
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
      SourcePosition = new PositionDesc();
    }

    public XmlNodeList Chapters
    {
      get;
      set;
    }

    // whole text that the chapter is holding
    // current paragraphs
    public List<BookContent.BookParagraph> Paragraphs
    {
      get;
      set;
    }

    public string SubString(int start, int end)
    {
      if (Paragraphs.Count == 0)
        return "";
      if (end >= Paragraphs[SourcePosition.ParagraphId].Content.Length)
        end = Paragraphs[SourcePosition.ParagraphId].Content.Length;
      return Paragraphs[SourcePosition.ParagraphId].Content.ToString(start, end - start);
    }

    public void Remove(int start, int end)
    {
      if (Paragraphs.Count <= SourcePosition.ParagraphId)
      {
        Paragraphs.Add(new BookParagraph());
      }
      Paragraphs[SourcePosition.ParagraphId].Content.Remove(start, end - start);
    }

    public void Insert(int where, StringBuilder content)
    {
      System.Diagnostics.Debug.Assert(Paragraphs.Count > SourcePosition.ParagraphId);
      Paragraphs[SourcePosition.ParagraphId].Content.Insert(where, content);
    }

    public void NextChapter()
    {
      // TODO move to the next chapter
      // clear, save and load new chapter
      int chapter = SourcePosition.ChapterId + 1;
      if (chapter >= Chapters.Count)
        return;
      Load( chapter );
    }

    public void PreviousChapter()
    {
      if (SourcePosition.ChapterId == 0)
        return;
      Load(SourcePosition.ChapterId - 1);
    }

    private void Load(int chapter)
    {
      XmlNode chapterNode = Chapters[chapter];
      Paragraphs.Clear();

      XmlNodeList paragraphs = chapterNode.ChildNodes;
      for (int p = 0; p < paragraphs.Count; p++)
      {
        BookParagraph par = new BookParagraph();
        par.Load(paragraphs[p]);
        if (par.Content.Length > 0)
          Paragraphs.Add(par);
      }
      
      SourcePosition.ChapterId = chapter;
      SourcePosition.ContentPos = 0;
      SourcePosition.ParagraphId = 0;
    }

    // ugly ugly ugly
    private XmlDocument doc;

    void SaveCurrent()
    {
      // replace all paragraph by what we have now
      XmlNode chapter = Chapters[SourcePosition.ChapterId];
      chapter.RemoveAll();
      for (int i = 0; i < Paragraphs.Count; i++)
      {
        XmlElement el = doc.CreateElement("Paragraph");
        el.InnerText = Paragraphs[i].Content.ToString();
        chapter.AppendChild(el);
      }
    }

    public int Save()
    {
      SaveCurrent();
      doc.Save(_filepath);
      return 0;
    }

    public int Load()
    {
      Paragraphs = new List<BookParagraph>();
      // read the sample xml
      doc = new XmlDocument();
      doc.Load(_filepath);
      XmlNodeList list = doc.SelectNodes("BookContent");
      Chapters = list[0].SelectNodes("Chapter");
      Load(0);
      return Paragraphs.Count;
    }

    public void AddPage(String content)
    {
      _pages.Add(content);
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
