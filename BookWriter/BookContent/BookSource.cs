using MyBook.Write.Bookmark;
using RiddleInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml;

namespace MyBook.BookContent
{
  public class XmlNodeNames
  {
    // main nodes
    public const String BookRoot = "Book";
    public const String SceneName = "Chapter";
    public const String CoverName = "Cover";
    public const String SceneParentName = "Scenes";
    
    // content types
    public const String ParagraphName = "Text";
    public const String ImageName = "Image";
    
    // external one
    public const String RiddleName = "Riddle";
  }

  static class XmlAttributeNames
  { 
    // Attributes
    public const String SceneId = "Name";
    public const String Description = "Description";
    public const String Column = "Column";
    public const String TimePosition = "TimePosition";
    public const String Duration = "Duration";    
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
      // name of the scene
      public String Name { get; set; }

      // description what will happen in the scene ( hint )
      public String Description { get; set; }

      // content of the book for writing
      public List<IContent> Pages { get; set; }

      // order between parallel scenes
      public int Column { get; set; }

      // vertical position when the scene happens
      public int TimePosition { get; set; }

      // size of the scene
      public int Duration { get; set; }
      
      // constructor
      public SceneDescription()
      {
        Pages = new List<IContent>();
        Duration = 1;
        TimePosition = -1;
        Column = 0;
        Description = "";
        Name = "";
      }
    }
    public void RemovePage()
    {
      Position.Scene.Pages.RemoveAt(Position.ParagraphId);
      if (Position.ParagraphId == Position.Scene.Pages.Count)
        Position.ParagraphId--;
    }
    public void SetPage(IContent content)
    {
      if (Position.ParagraphId == Position.Scene.Pages.Count)
      {
        Position.Scene.Pages.Add(content);
      }
      else
        Position.Scene.Pages[Position.ParagraphId] = content;
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
      // if current page is null - ok
      if (Position.Scene.Pages[Position.ParagraphId] == null)
        return;
      Position.Scene.Pages.Insert(Position.ParagraphId+1,null);
      MoveForward();
    }
    public void MoveForward()
    {
      if (Position.ParagraphId ==Position.Scene.Pages.Count - 1)
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
      Name = Path.GetFileNameWithoutExtension(name);
      XmlDocument doc = new XmlDocument();
      doc.Load(name);
      // load whole
      XmlNode parent = doc.FirstChild;
      if (parent.ChildNodes != null)
        Scenes.Clear();
      SceneDescription d;
      foreach (XmlNode node in parent.ChildNodes)
      {
        d = new SceneDescription();
        // traverse each attribute
        foreach( XmlAttribute a in node.Attributes)
        {
          switch ( a.Name )
          { 
            case XmlAttributeNames.SceneId:
              d.Name = a.Value;
              break;
            case XmlAttributeNames.Description:
              d.Description = a.Value;
              break;
            case XmlAttributeNames.Column:
              d.Column = Int32.Parse(a.Value);
              break;
            case XmlAttributeNames.TimePosition:
              d.TimePosition = Int32.Parse(a.Value);
              break;
            case XmlAttributeNames.Duration:
              d.Duration = Int32.Parse(a.Value);
              break;
          }
        }
        foreach (XmlNode contentNode in node.ChildNodes)
        {
          IContent content = null;
          foreach (IRiddleHandler handler in handlers)
          {
            content = handler.Load(contentNode);
            if (content != null)
            {
              d.Pages.Add(content);
              break;
            }
          }
        }
        Scenes.Add(d);
      }
      Position.Scene = Scenes[0];
      NotifyPropertyChanged("CanGoFurther");
      NotifyPropertyChanged("CanGoBack");
    }

    public bool CanGoBack
    {
      get { return Position.ParagraphId > 0; }
    }

    public bool CanGoFurther
    {
      get { return Position.ParagraphId < Position.Scene.Pages.Count - 1; }
    }

    public IContent GetContent()
    {
      //Position.Scene = Scenes.Find(new Predicate<SceneDescription>(x => x.Name == Position.SceneName));
      return Position.Scene.Pages[Position.ParagraphId];
    }

    public BookSource()
    {
      // init
      Name = null;
      Position = new PositionDesc();
      Position.Clear();
      Init();
    }

    public void CreateScene()
    {
      Position.Scene = new SceneDescription();      
      Scenes.Add(Position.Scene);
      Position.Scene.Name = $"Scene #{Scenes.Count}"; 
      Position.Clear();
      SetPage(null);
      NotifyPropertyChanged("Position");
      NotifyPropertyChanged("CanGoBack");
      NotifyPropertyChanged("CanGoFirther");
    }
    public void SaveScene(String sceneName)
    {
      Position.Scene.Name = sceneName;            
      NotifyPropertyChanged("Scenes");
    }

    public void AddAttribute(XmlDocument doc, XmlNode node,String name, String value)
    {
      XmlAttribute att = doc.CreateAttribute(name);
      att.Value = value;
      node.Attributes.Append(att);
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
      foreach (SceneDescription scene in Scenes)
      {
        XmlElement e = doc.CreateElement(XmlNodeNames.SceneName);
        AddAttribute(doc, e, XmlAttributeNames.SceneId, scene.Name);
        AddAttribute(doc, e, XmlAttributeNames.Description, scene.Description);
        if (scene.TimePosition >= 0)
        {
          AddAttribute(doc, e, XmlAttributeNames.Column, scene.Column.ToString());
          AddAttribute(doc, e, XmlAttributeNames.Duration, scene.Duration.ToString());
          AddAttribute(doc, e, XmlAttributeNames.TimePosition, scene.TimePosition.ToString());
        }
        bool canBeSaved = false;
        foreach (IContent content in scene.Pages)
        {
          if (content == null)
            continue;
          canBeSaved = true;
          XmlNode node = content.ToXmlNode(doc);
          e.AppendChild(node);
        }
        if ( canBeSaved )
          parent.AppendChild(e);
      }
      doc.AppendChild(parent);
      doc.Save(fullpath);
      return 0;
    }

    // Check if this book can be used
    public bool IsValid(String filepath, int flags = 0)
    {
      return (System.IO.File.Exists(filepath));
    }

    public delegate void OnSceneSet();

    public void SetScene(SceneDescription sceneDescription)
    {
      Position.Clear();
      Position.Scene = sceneDescription;
      NotifyPropertyChanged("Position");
      NotifyPropertyChanged("CanGoFurther");
      NotifyPropertyChanged("CanGoBack");
    }
  }
}
