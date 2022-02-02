using MyBook.Write.Bookmark;
using RiddleInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace MyBook.BookContent
{
  public class XmlNodeNames
  {
    // main nodes
    public const String BookRoot = "Book";
    public const String CoverName = "Cover";

    /// <summary>
    /// Scenes
    /// </summary>
    public const String Chapters = "Chapters";
    public const String SceneName = "Chapter";
    // content types
    public const String ParagraphName = "Text";
    public const String ImageName = "Image";
    public const String RiddleName = "Riddle";

    /// <summary>
    /// Character
    /// </summary>
    public const String Characters = "Characters";
    public const String Character = "Character";
    public const String Episode = "Episode";
    public const String Father = "Father";
    public const String Mother = "Mother";
    public const String Gender = "Gender";
    public const String Status = "Status";
  }

  public class AssemblyMap
  {
    public String Name { get; set; }
    public Assembly Assembly { get; set; }
  }

  public class BookSource : INotifyPropertyChanged
  {
    //
    public List<AssemblyMap> HandlersMap { get; set; }

    private void InitPlugins()
    {
      Assembly assem = Assembly.GetExecutingAssembly();
      Uri ur = new Uri(assem.CodeBase);
      FileInfo fi = new FileInfo(ur.AbsolutePath);
      string s = fi.Directory.FullName;

      //find all dlls
      string[] dlls = Directory.GetFiles(s + "\\Plugins", "*.dll");
      ICollection<Assembly> assemblies = new List<Assembly>(dlls.Length);

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
          {
            AssemblyMap m = new AssemblyMap();
            m.Assembly = a;
            m.Name = riddle.Name;
            HandlersMap.Add(m);
          }
        }
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    void NotifyPropertyChanged(string property)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(property));
    }

    public IRiddleHandler CurrentPage { get; set; }

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

    public void SetPage(int s, int p)
    {
      Position = new PositionDesc();
      Position.Scene = Scenes[s];
      Position.Clear();
      Position.ParagraphId = p;
      CurrentPage = Scenes[s].Pages[p];
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

      // content of the book for writing
      public List<IRiddleHandler> Pages { get; set; }

      // for timeline - order between parallel scenes
      public int Column { get; set; }

      // for timeline - vertical position when the scene happens
      public int TimePosition { get; set; }

      // size of the scene
      public int Duration { get; set; }

      // constructor
      public SceneDescription()
      {
        Pages = new List<IRiddleHandler>();
        Duration = 1;
        TimePosition = -1;
        Column = 0;
      }
    }

    public class CharacterEpisodes
    {
      public String Title { get; set; }
      public String Content { get; set; }
    }

    // whole info about character
    public class CharacterContent
    {
      // unique identificator of the character
      public int Id { get; set; }

      // name of the scene
      public String Name { get; set; }

      // spouse of the character
      public CharacterContent Spouse { get; set; }

      // father of the character
      public CharacterContent Father { get; set; }

      // mother of the character
      public CharacterContent Mother { get; set; }

      // current status of the character
      public CharacterStatus Status { get; set; }

      // Gender. Male / female ;)
      public CharacterGender Gender { get; set; }

      // details
      public List<CharacterEpisodes> Info { get; set; }

      /// <summary>
      /// Functions
      /// </summary>
      public CharacterContent()
      {
        Info = new List<CharacterEpisodes>();
      }
    }

    public CharacterContent Get(string name)
    {
      return Characters.Find(x => x.Name == name);
    }

    public void RemovePage()
    {
      Position.Scene.Pages.RemoveAt(Position.ParagraphId);
      MoveBack();
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
      RefreshContent();
    }

    public void CreatePage(AssemblyMap assembly)
    {
      IRiddleHandler h = null;
      Type[] types = assembly.Assembly.GetTypes();
      Type rh = typeof(IRiddleHandler);
      foreach (Type t in types)
      {
        if (t.IsAbstract || t.IsNotPublic || !rh.IsAssignableFrom(t))
          continue;
        h = assembly.Assembly.CreateInstance(t.ToString()) as IRiddleHandler;
        break;
      }

      h.Create();
      if (Position.ParagraphId < 0)
      {
        Position.Scene.Pages.Add(h);
      }
      else
      {
        Position.Scene.Pages.Insert(Position.ParagraphId + 1, h);
      }
      MoveForward();      
    }

    private void RefreshContent()
    {
      CurrentPage = Position.Scene.Pages[Position.ParagraphId];
      NotifyPropertyChanged("Position");
      NotifyPropertyChanged("CanGoBack");
      NotifyPropertyChanged("CanGoFurther");
      NotifyPropertyChanged("CurrentPage");
    }

    public void MoveForward()
    {
      if (Position.ParagraphId == Position.Scene.Pages.Count - 1)
        return;
      Position.ParagraphId++;
      RefreshContent();  
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

    public CharacterContent DummyCharacter { get; set; }

    public List<CharacterContent> Characters { get;set; }

    public void Init()
    {
      DummyCharacter = new CharacterContent();
      DummyCharacter.Id = -1;
      DummyCharacter.Name = "Unknown";
      HandlersMap = new List<AssemblyMap>();
      InitPlugins();
      _scenes = new List<SceneDescription>();
      Characters = new List<CharacterContent>();
      CreateScene();
      // create first person
      CreateCharacter();
    }

    int LastOne = 0;

    public CharacterContent CreateCharacter()
    {
      CharacterContent c = new CharacterContent();
      c.Id = LastOne;
      c.Name = "Anonymous";
      c.Father = DummyCharacter;
      c.Mother = DummyCharacter;
      CharacterEpisodes ep = new CharacterEpisodes();
      ep.Title = "Life";
      ep.Content = "";
      c.Info.Add(ep);
      Characters.Add(c);
      LastOne++;
      return c;
    }
    private void LoadScenes()
    {

    }

    //class ParentFind
    //{
    //  public CharacterContent c;
    //  public Int32 father;
    //  public Int32 mother;
    //}

    private void LoadCharacters()
    {

    }

    public void Load(String name)
    {
      //XmlDocument doc = new XmlDocument();
      //doc.Load(name);
      //// load whole
      //XmlNodeList n = doc.GetElementsByTagName(XmlNodeNames.Chapters);
      //Name = Path.GetFileNameWithoutExtension(name);
      //LoadScenes(n[0]);
      //n = doc.GetElementsByTagName(XmlNodeNames.Characters);
      //LoadCharacters(n[0]);
      //Position.Scene = Scenes[0];
      //NotifyPropertyChanged("CanGoFurther");
      //NotifyPropertyChanged("CanGoBack");
      //object o = HandlersMap[0].Assembly.GetType("IRiddleHandler");
      //CurrentPage = o as IRiddleHandler;
    }

    public bool CanGoBack
    {
      get { return Position.ParagraphId > 0; }
    }

    public bool CanGoFurther
    {
      get { return ( Position.Scene.Pages.Count >0 ) 
          && (Position.ParagraphId < Position.Scene.Pages.Count - 1);
      }
    }

    public IRiddleHandler GetContent()
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
      NotifyPropertyChanged("Position");
      NotifyPropertyChanged("CanGoBack");
      NotifyPropertyChanged("CanGoFurther");
    }

   
    private void SaveScenes()
    {
      // todo better
    }

    public int Save()
    {
      if (!Directory.Exists(Settings.BooksFolder))
      {
        Directory.CreateDirectory(Settings.BooksFolder);
      }
      String fullpath = Settings.BooksFolder + "\\" + Name + Constants.Extension;
      SaveScenes();
      //SaveCharacters(doc, parent);
      //doc.AppendChild(parent);
      //doc.Save(fullpath);
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
