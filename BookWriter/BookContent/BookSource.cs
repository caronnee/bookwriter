using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Controls;
using MyBook.Pages.Write.Text;
using MyBook.Pages.Write.Imaging;

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
    public String Name
    {
      get;
      set;
    }

    public BookSource(String name)
    {
      // init
      doc = new XmlDocument();
      Paragraphs = new List<IContent>();

      XmlElement p = doc.CreateElement(XmlNodeNames.BookRoot);
      doc.AppendChild(p);
      XmlElement el = doc.CreateElement(XmlNodeNames.ChapterParentName);
      p.AppendChild(el);
      XmlNode node = doc.CreateElement(XmlNodeNames.ChapterName);
      el.AppendChild(node);

      Init();
      Load(0);
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
      Load(desc.ChapterId);
      if (desc.ParagraphId < Paragraphs.Count)
        return Paragraphs[desc.ParagraphId];
      return null;
    }

    public int InsertChapter( int oldChapter)
    {
      //Save paragraphs to the Chapter
      SaveChapter();
      // Load this chapter
      XmlElement element = doc.CreateElement(XmlNodeNames.ChapterName);
      XmlNode p = Chapters[oldChapter].ParentNode;
      p.InsertAfter(element, Chapters[oldChapter]);
      Init();
      oldChapter++;
      Load(oldChapter); 
      return oldChapter;
    }

    public void InsertParagraph(int position, IContent content)
    {
      if ( Paragraphs.Count == position)
        Paragraphs.Add(content);
      else
        Paragraphs.Insert(position, content);
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
        if (Paragraphs[i] == null)
          continue;
        XmlNode node = Paragraphs[i].ToXmlNode(doc);
        CurrentChapter.AppendChild(node);
      }
    }

    public void Load(int chapter)
    {
      if (Chapters[chapter] == CurrentChapter)
        return;

      CurrentChapter = Chapters[chapter];
      Paragraphs.Clear();
      XmlNodeList paragraphs = CurrentChapter.ChildNodes;
      for (int p = 0; p < paragraphs.Count; p++)
      {
        XmlNode node = paragraphs[p];
        IContent par = null;
        if (node.Name == XmlNodeNames.ParagraphName)
            par = new TextParagraph();
        if (node.Name == XmlNodeNames.ImageName)
            par = new ImageParagraph();
        if (par == null)
          continue;
        par.Load(paragraphs[p]);
        Paragraphs.Add(par);
      }
    }

    // Root document
    private XmlDocument doc;

    public int Save()
    {
      if (!Directory.Exists(Settings.BooksFolder))
      {
        Directory.CreateDirectory(Settings.BooksFolder);
      }
      String fullpath = Settings.BooksFolder + "\\" + Name + Constants.Extension;
      doc.Save(fullpath);
      return 0;
    }

    public void Init()
    {
      XmlNodeList list = doc.SelectNodes(XmlNodeNames.BookRoot);
      list = list[0].SelectNodes(XmlNodeNames.ChapterParentName);
      Chapters = list[0].SelectNodes(XmlNodeNames.ChapterName);
      Paragraphs.Clear();
    }

    public void Load(String filepath)
    {
      // read the sample xml
      doc.Load(filepath);
      Init();
      Load(0);
      Name = Path.GetFileNameWithoutExtension(filepath);
    }
    
    // Check if this book can be used
    public bool IsValid(String filepath,int flags = 0)
    {
      return (System.IO.File.Exists(filepath));
    }
  }
}
