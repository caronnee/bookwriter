using MyBook.Pages.Write.Bookmark;
using RiddleInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml;

namespace MyBook.BookContent
{
  static class XmlNodeNames
  {
    static public String BookRoot = "Book";
    static public String SceneName = "Chapter";
    static public String CoverName = "Cover";
    static public String SceneParentName = "Scenes";

    // Attributes
    static public String SceneId = "SceneName";

    // content types
    static public String ParagraphName = "Text";
    static public String ImageName = "Image";
    static public String RiddleName = "Riddle";
  }

  public class BookSource : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    void NotifyPropertyChanged(string property)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(property));
    }

    public PositionDesc Position
    {
      get;
      set;
    }

    private List<BookmarksHeader> _bookmarks;
    public List<BookmarksHeader> Bookmarks
    {
      get { return _bookmarks; }
      set
      {
        _bookmarks = value;
        NotifyPropertyChanged("Bookmarks");
      }
    }
    public void InitB()
    {
      Bookmarks = new List<BookmarksHeader>();
      Bookmarks.Add(new BookmarksHeader { Name = "Locations", Content = "Something else" });
      Bookmarks.Add(new BookmarksHeader
      {
        Name = "Characters",
        Content = "Something else",
        Bookmarks = new List<BookmarksHeader>() {
          new BookmarksHeader { Name = "Relicts2", Content = "Something else" },
          new BookmarksHeader { Name = "Relicts2", Content = "Something else" },
          new BookmarksHeader { Name = "Relicts2", Content = "Something else" },
          new BookmarksHeader { Name = "Relicts2", Content = "Something else" }
        }
      });
      Bookmarks.Add(new BookmarksHeader
      {
        Name = "Relicts",
        Content = "Something else",
        Bookmarks = new List<BookmarksHeader>() {
          new BookmarksHeader { Name = "Relicts2", Content = "Something else" },
          new BookmarksHeader { Name = "Relicts2", Content = "Something else" },
          new BookmarksHeader { Name = "Relicts2", Content = "Something else" },
          new BookmarksHeader { Name = "Relicts2", Content = "Something else" }
        }
      });

      Bookmarks.Add(new BookmarksHeader { Name = "History", Content = "Something else" });
      Bookmarks.Add(new BookmarksHeader { Name = "Timeline", Content = "Something else" });
    }

    public class SceneDescription
    {
      public String Name { get; set; }
      public int Order { get; set; }
      public List<IContent> Pages { get; set; }
      public SceneDescription()
      {
        Pages = new List<IContent>();
        Order = 0;
      }
    }

    public void SetPage(IContent content)
    {
      if (Position.ParagraphId == ActualScene.Pages.Count)
      {
        ActualScene.Pages.Add(content);
      }
      else
        ActualScene.Pages[Position.ParagraphId] = content;

    }
    public SceneDescription ActualScene
    {
      get;
      set;
    }

    // name of the book 
    public String Name
    {
      get;
      set;
    }

    public void MoveBack()
    {
      Position.ParagraphId--;
      if (Position.ParagraphId < 0)
        Position.ParagraphId = 0;
      NotifyPropertyChanged("CanGoFurther");
      NotifyPropertyChanged("CanGoBack");
    }

    public void CreatePage()
    {
      if (ActualScene.Pages[ActualScene.Pages.Count-1] == null)
        return;
      ActualScene.Pages.Add(null);
      MoveForward();
    }
    public void MoveForward()
    {
      if (Position.ParagraphId == ActualScene.Pages.Count - 1)
        return;
      Position.ParagraphId++;
      NotifyPropertyChanged("CanGoFurther");
      NotifyPropertyChanged("CanGoBack");
    }
    private List<SceneDescription> _scenes;
    public List<SceneDescription> Scenes
    {
      get
      {
        return _scenes;
      }
      private set
      {
        _scenes = value;
      }
    }

    public void Init()
    {
      InitB();
      _scenes = new List<SceneDescription>();
      CreateScene();
    }

    public void Load(String name, List<IRiddleHandler> handlers)
    {
      Name = name;
      XmlDocument doc = new XmlDocument();
      doc.Load(Name);
      // load whole
      XmlNode parent = doc.FirstChild;
      foreach (XmlNode node in parent.ChildNodes)
      {
        ActualScene = new SceneDescription();
        String sn = node.Attributes[XmlNodeNames.SceneId].Value;
        foreach (XmlNode contentNode in node.ChildNodes)
        {
          IContent content = null;
          foreach (IRiddleHandler handler in handlers)
          {
            content = handler.Load(node);
            if (content != null)
            {
              ActualScene.Pages.Add(content);
              break;
            }
          }
        }
      }
    }

    public bool CanGoBack
    {
      get { return Position.ParagraphId != 0; }
    }

    public bool CanGoFurther
    {
      get { return Position.ParagraphId < ActualScene.Pages.Count - 1; }
    }

    public BookSource()
    {
      // init
      Name = null;
      Position = new PositionDesc();
      Position.Clear();
      Init();
    }

    public BookSource(String name)
    {
      // init
      Name = name;
      Init();
      Load(name);
    }

    public void CreateScene()
    {
      ActualScene = new SceneDescription();      
      Scenes.Add(ActualScene);
      SetPage(null);
    }
    public void SaveScene(String sceneName)
    {
      ActualScene.Name = sceneName;            
      NotifyPropertyChanged("Scenes");
    }

    public int Save()
    {
      if (!Directory.Exists(Settings.BooksFolder))
      {
        Directory.CreateDirectory(Settings.BooksFolder);
      }
      String fullpath = Settings.BooksFolder + "\\" + Name + Constants.Extension;
      XmlDocument doc = new XmlDocument();
      XmlElement parent = doc.CreateElement(XmlNodeNames.BookRoot);
      foreach (var scene in Scenes)
      {
        XmlElement e = doc.CreateElement(XmlNodeNames.SceneName);
        XmlAttribute att = doc.CreateAttribute(XmlNodeNames.SceneId);
        e.Attributes.Append(att);
        SceneDescription d = scene;
        att.Value = d.Name;
        foreach (IContent content in d.Pages)
        {
          XmlNode node = content.ToXmlNode(doc);
          e.AppendChild(node);
        }
        parent.AppendChild(e);
      }
      doc.AppendChild(parent);
      doc.Save(fullpath);
      return 0;
    }


    public void Load(String filepath)
    {
      Init();
      // read the sample xml
      XmlDocument doc = new XmlDocument();
      doc.Load(filepath);

      Name = Path.GetFileNameWithoutExtension(filepath);
    }

    // Check if this book can be used
    public bool IsValid(String filepath, int flags = 0)
    {
      return (System.IO.File.Exists(filepath));
    }
  }
}
