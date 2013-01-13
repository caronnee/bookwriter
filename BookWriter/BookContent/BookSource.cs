using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Controls;

namespace MyBook.BookContent
{
  public class BookSource
  {
    private String _filepath; // first go offsets

    public BookSource(String name)
    {
      // init
      _filepath = name;
      doc = new XmlDocument();
      Paragraphs = new List<IContent>();

      // load from file
      if (File.Exists(name))
        Load();
    }

    public XmlNodeList Chapters
    {
      get;
      set;
    }

    // whole text that the chapter is holding
    // current paragraphs
    public List<IContent> Paragraphs
    {
      get;
      set;
    }

    public IContent GetContent(PositionDesc desc)
    {
      if (desc == null)
        return null;
      if (desc.ContentPos < Paragraphs.Count)
        return Paragraphs[desc.ParagraphId];
      return null;
    }

    //public string SubString(int start, int end)
    //{
    //  if (Paragraphs.Count == 0)
    //    return "";
    //  if (end >= Paragraphs[SourcePosition.ParagraphId].Content.Length)
    //    end = Paragraphs[SourcePosition.ParagraphId].Content.Length;
    //  return Paragraphs[SourcePosition.ParagraphId].Content.ToString(start, end - start);
    //}

    //public void Remove(int start, int end)
    //{
    //  if (Paragraphs.Count <= SourcePosition.ParagraphId)
    //  {
    //    Paragraphs.Add(new BookParagraph());
    //  }
    //  Paragraphs[SourcePosition.ParagraphId].Content.Remove(start, end - start);
    //}

    //public void Insert(int where, StringBuilder content)
    //{
    //  System.Diagnostics.Debug.Assert(Paragraphs.Count > SourcePosition.ParagraphId);
    //  Paragraphs[SourcePosition.ParagraphId].Content.Insert(where, content);
    //}


    //public void PreviousChapter()
    //{
    //  if (SourcePosition.ChapterId == 0)
    //    return;
    //  Load(SourcePosition.ChapterId - 1);
    //}

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
      
    }

    // ugly ugly ugly
    private XmlDocument doc;

    void SaveCurrent()
    {
      // replace all paragraph by what we have now
      //XmlNode chapter = Chapters[SourcePosition.ChapterId];
      //chapter.RemoveAll();
      //for (int i = 0; i < Paragraphs.Count; i++)
      //{
      //  XmlElement el = doc.CreateElement("Paragraph");
      //  el.InnerText = Paragraphs[i].Content.ToString();
      //  chapter.AppendChild(el);
      //}
    }

    public int Save()
    {
      SaveCurrent();
      doc.Save(_filepath);
      return 0;
    }

    public int Load()
    {
      Paragraphs = new List<IContent>();
      // read the sample xml
      doc.Load(_filepath);
      XmlNodeList list = doc.SelectNodes("BookContent");
      Chapters = list[0].SelectNodes("Chapters")[0].SelectNodes("Chapter");
      Load(0);
      return Paragraphs.Count;
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
