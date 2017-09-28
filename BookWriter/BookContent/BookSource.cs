using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Controls;
using MyBook.Pages.Write.Text;

namespace MyBook.BookContent
{
  static class XmlNodeNames
  {
    static public String BookRoot = "Book";
    static public String ChapterName = "Chapter";
    static public String CoverName = "Cover";
    static public String ChapterParentName = "Chapters";

    // content types
    static public String ParagraphName = "Paragraph";
    static public String ImageName = "Image";
    static public String RiddleName = "Riddle";
  }

  public class BookSource
  {
    public BookSource(String name)
    {
      // init
      Paragraphs = new List<IContent>();
      
      // load from file
      if (File.Exists(name))
        Load(name);
      else{
        doc = new XmlDocument();
        XmlElement p = doc.CreateElement(XmlNodeNames.BookRoot);
        doc.AppendChild(p);
        XmlElement el = doc.CreateElement(XmlNodeNames.ChapterParentName);
        p.AppendChild(el);
        XmlNode node = doc.CreateElement(XmlNodeNames.ChapterName);
        el.AppendChild(node);
        Init(0);
      }
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

    public int NPages()
    {
      return Paragraphs.Count;
    }

    public int InsertChapter( int oldChapter)
    {
      //Save paragraphs to the Chapter

      // Load this chapter
      XmlElement element = doc.CreateElement(XmlNodeNames.ChapterName);
      XmlNode p = Chapters[oldChapter].ParentNode;
      p.InsertAfter(element, Chapters[oldChapter]);
      oldChapter++;
      Load(oldChapter);
      return oldChapter;
    }

    public int NChapters()
    {
      return Chapters.Count;
    }

    public void SetParagraph(PositionDesc desc, IContent content)
    {
      if (desc.ParagraphId >= Paragraphs.Count)
        Paragraphs.Add(content);
      else
        Paragraphs[desc.ParagraphId] = content;
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

    XmlNode CurrentChapter
    {
      get;
      set;
    }

    public void SaveChapter()
    {
      // save to the xml
      CurrentChapter.RemoveAll();
      for (int i = 0; i < Paragraphs.Count; i++)
      {
        XmlNode node = Paragraphs[i].ToXmlNode(doc);
        CurrentChapter.AppendChild(node);
      }
    }
    private void Load(int chapter)
    {
      SaveChapter(); 
      Paragraphs.Clear();

      Init(chapter);

      XmlNodeList paragraphs = CurrentChapter.ChildNodes;
      for (int p = 0; p < paragraphs.Count; p++)
      {
        TextParagraph par = new TextParagraph();
        par.Load(paragraphs[p]);
        if (par.Content.Length > 0)
          Paragraphs.Add(par);
      }
    }

    // Root document
    private XmlDocument doc;

    public int Save(String name)
    {
      String fullpath = Settings.BooksFolder + "\\" + name + Constants.Extension;
      if (!Directory.Exists(Settings.BooksFolder))
      {
        Directory.CreateDirectory(Settings.BooksFolder);
      }
      doc.Save(fullpath);
      return 0;
    }

    public void Init(int i)
    {
      XmlNodeList list = doc.SelectNodes(XmlNodeNames.BookRoot);
      list = list[0].SelectNodes(XmlNodeNames.ChapterParentName);
      Chapters = list[0].SelectNodes(XmlNodeNames.ChapterName);
      CurrentChapter = Chapters[i];
    }
    public int Load(String filepath)
    {
      Paragraphs = new List<IContent>();
      // read the sample xml
      doc.Load(filepath);

      // TODO last bookmark
      Init(0);
      Load(0);
      return Paragraphs.Count;
    }


    // Check if this book can be used
    public bool IsValid(String filepath,int flags = 0)
    {
      if (System.IO.File.Exists(filepath))
        return true;
      return false;
    }
  }
}
