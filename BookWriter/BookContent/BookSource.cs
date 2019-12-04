using MyBook.Write.Bookmark;
using MyBook.Write.Character;
using MyBook.Write.Content;
using RiddleInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using System.Xml;

namespace MyBook.BookContent
{
  public class XmlNodeNames
  {
    // main nodes
    public const String BookRoot = "Book";
    public const String Chapters = "Chapters";
    public const String SceneName = "Chapter";
    public const String Characters = "Characters";
    public const String Character = "Character";
    public const String CoverName = "Cover";
    public const String SceneParentName = "Scenes";
    public const String Episode = "Episode";

    // content types
    public const String ParagraphName = "Text";
    public const String ImageName = "Image";
    
    // external one
    public const String RiddleName = "Riddle";
  }

  static class XmlAttributeNames
  { 
    // Attributes
    public const String Name = "Name";
    public const String Description = "Description";
    public const String Column = "Column";
    public const String TimePosition = "TimePosition";
    public const String Duration = "Duration";    
  }

  public class BookSource : INotifyPropertyChanged
  {
    public List<IRiddleHandler> ContentHandlers {
      get;
      private set;
    }

    private List<IRiddleHandler> InitPlugins()
    {
      Assembly assem = Assembly.GetExecutingAssembly();
      Uri ur = new Uri(assem.CodeBase);
      FileInfo fi = new FileInfo(ur.AbsolutePath);
      string s = fi.Directory.FullName;

      //find all dlls
      string[] dlls = Directory.GetFiles(s + "\\Plugins", "*.dll");
      ICollection<Assembly> assemblies = new List<Assembly>(dlls.Length);
      List<IRiddleHandler> riddles = new List<IRiddleHandler>();
      // first  the basic ones:
      riddles.Add(new Write.Text.TextHandler());
      riddles.Add(new Write.Picture.ImageHandler());
      foreach (string dllFile in dlls)
      {
        AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
        Assembly assembly = Assembly.Load(an);
        assemblies.Add(assembly);
      }

      Type rh = typeof(IRiddleHandler);

      foreach (Assembly a in assemblies)
      {
        Type[] types = a.GetTypes();
        foreach (Type t in types)
        {
          if (t.IsAbstract || t.IsNotPublic || !rh.IsAssignableFrom(t))
            continue;
          // create as instance
          object iRiddle = a.CreateInstance(t.ToString());
          IRiddleHandler riddle = iRiddle as IRiddleHandler;
          if (riddle != null)
            riddles.Add(riddle);
        }
      }

      return riddles;
    }
    public IRiddleHandler GetCurrentHandler()
    {
      // first find the handler - we have ensured that 
      IContent content = GetContent();
      foreach (IRiddleHandler h in ContentHandlers)
      {
        if (h.ToViewport(content))
        {
          return h;
        }
      }
      // todo return not known content handler - please update :D
      return null;
    }
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
    //public void InitB()
    //{
    //  Bookmarks = new List<BookmarksHeader>();
    //  Bookmarks.Add(new BookmarksHeader { Name = "Locations", Content = "Something else" });
    //  Bookmarks.Add(new BookmarksHeader
    //  {
    //    Name = "Characters",
    //    Content = "Something else",
    //    Bookmarks = new List<BookmarksHeader>() {
    //      new BookmarksHeader { Name = "Relicts2", Content = "Something else" },
    //      new BookmarksHeader { Name = "Relicts2", Content = "Something else" },
    //      new BookmarksHeader { Name = "Relicts2", Content = "Something else" },
    //      new BookmarksHeader { Name = "Relicts2", Content = "Something else" }
    //    }
    //  });
    //  Bookmarks.Add(new BookmarksHeader
    //  {
    //    Name = "Relicts",
    //    Content = "Something else",
    //    Bookmarks = new List<BookmarksHeader>() {
    //      new BookmarksHeader { Name = "Relicts2", Content = "Something else" },
    //      new BookmarksHeader { Name = "Relicts2", Content = "Something else" },
    //      new BookmarksHeader { Name = "Relicts2", Content = "Something else" },
    //      new BookmarksHeader { Name = "Relicts2", Content = "Something else" }
    //    }
    //  });

    //  Bookmarks.Add(new BookmarksHeader { Name = "History", Content = "Something else" });
    //  Bookmarks.Add(new BookmarksHeader { Name = "Timeline", Content = "Something else" });
    //}

    public class SceneDescription
    {
      // name of the scene
      public String Name { get; set; }

      // description what will happen in the scene ( hint )
      public String Description { get; set; }

      // content of the book for writing
      public List<IContent> Pages { get; set; }

      // for timeline - order between parallel scenes
      public int Column { get; set; }

      // for timeline - vertical position when the scene happens
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
    public struct CharacterEpisodes
    {
      public string Name;
      public string Content;
    }

    public class CharacterContent
    {
      public string Name { get; set; }
      public List<CharacterEpisodes> Info { get; set; }
      public CharacterContent()
      {
        Info = new List<CharacterEpisodes>();
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
    private List<CharacterContent> _characters;
    public List<CharacterContent> Characters
    {
      get
      {
        return _characters;
      }
      private set
      {
        _characters = value;
      }
    }

    public void Init()
    {
      ContentHandlers = InitPlugins();
      _scenes = new List<SceneDescription>();
      _characters = new List<CharacterContent>();     
      CreateScene();
      // create first person
      CreateCharacter();
    }
    public void CreateCharacter()
    {
      CharacterContent c = new CharacterContent();
      c.Name = "Anonymous";
      CharacterEpisodes ep = new CharacterEpisodes();
      ep.Name = "Life";
      ep.Content = "";
      c.Info.Add(ep);

    }
    private void LoadScenes(XmlNode parent)
    {
      if (parent.ChildNodes != null)
        Scenes.Clear();
      SceneDescription d;
      foreach (XmlNode node in parent.ChildNodes)
      {
        d = new SceneDescription();
        // traverse each attribute
        foreach (XmlAttribute a in node.Attributes)
        {
          switch (a.Name)
          {
            case XmlAttributeNames.Name:
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
          IContent content = new UnrecognizedContent();
          foreach ( IRiddleHandler h in ContentHandlers)
          {
            if (h.CanLoad(contentNode))
              content = h.Load(contentNode);
          }
          d.Pages.Add(content);
        }
        Scenes.Add(d);
      }
    }
    private void LoadCharacters(XmlNode parent)
    {
      foreach (XmlNode node in parent.ChildNodes)
      {
        CharacterContent i = new CharacterContent();
        foreach (XmlAttribute att in node.Attributes)
        {
          if (att.Name == XmlAttributeNames.Name)
            i.Name = att.Name;
        }
        foreach(XmlNode n in node.ChildNodes)
        {
          switch (n.Name)
          {
            case XmlNodeNames.Episode:
              {
                CharacterEpisodes ep = new CharacterEpisodes();
                ep.Name = n.Attributes.GetNamedItem(XmlAttributeNames.Name).Value;
                ep.Content = n.Value;
                i.Info.Add(ep);
                break;
              }
          }
        }
        Characters.Add(i);
      }
    }

    public void Load(String name)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(name);
      // load whole
      XmlNodeList n = doc.GetElementsByTagName(XmlNodeNames.Chapters);
      Name = Path.GetFileNameWithoutExtension(name);
      LoadScenes(n[0]);
      n = doc.GetElementsByTagName(XmlNodeNames.Characters);
      LoadCharacters(n[0]);
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
    
    public void SaveSceneName(String sceneName)
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
    private void SaveCharacters(XmlDocument doc, XmlElement el)
    {
      XmlNode parent = doc.CreateElement(XmlNodeNames.Characters);
      el.AppendChild(parent);
      foreach (CharacterContent c in Characters)
      {
        XmlNode character = doc.CreateElement(XmlNodeNames.Character);
        parent.AppendChild(character);
        AddAttribute(doc,character,XmlAttributeNames.Name, c.Name);
        foreach( CharacterEpisodes ep in c.Info)
        {
          XmlNode n = doc.CreateElement(XmlNodeNames.Episode);
          AddAttribute(doc,n,XmlAttributeNames.Name, ep.Name);
          n.InnerText = ep.Content;
          character.AppendChild(n);
        }
      }
    }
    private void SaveScenes(XmlDocument doc, XmlElement el)
    {
      XmlNode parent = doc.CreateElement(XmlNodeNames.Chapters);
      el.AppendChild(parent);
      foreach (SceneDescription scene in Scenes)
      {
        XmlElement e = doc.CreateElement(XmlNodeNames.SceneName);
        AddAttribute(doc, e, XmlAttributeNames.Name, scene.Name);
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
        if (canBeSaved)
          parent.AppendChild(e);
      }
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
      SaveScenes(doc,parent);
      SaveCharacters(doc, parent);
      doc.AppendChild(parent);
      doc.Save(fullpath);
      return 0;
    }

    // Check if this book can be used
    public bool IsValid(String filepath, int flags = 0)
    {
      return (System.IO.File.Exists(filepath));
    }

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
