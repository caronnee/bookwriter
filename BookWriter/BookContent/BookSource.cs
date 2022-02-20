using MyBook.Write.Bookmark;
using RiddleInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

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
    public const String Scenes = "Scenes";
    public const String Scene = "Scene";
    public const String Page = "Page";
    public const String Plugin = "Plugin";

    // content types
    public const String ParagraphName = "Text";
    public const String ImageName = "Image";
    public const String RiddleName = "Riddle";

    /// <summary>
    /// Character
    /// </summary>
    public const String Characters = "Characters";
    public const String Character = "Character";
    public const String CharacterName = "Name";
    public const String Episodes = "Episodes";
    public const String Episode = "Episode";
    public const String EpisodeContent = "EpisodeContent";
    public const String Father = "Father";
    public const String Mother = "Mother";
    public const String Gender = "Gender";
    public const String Status = "Status";
  }

  public class AssemblyMap
  {
    public String Name { get; set; }
    public Assembly Assembly { get; set; }
    public Type Type { get; set; }
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
            m.Type = t;
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

      // constructor
      public SceneDescription()
      {
        Pages = new List<IRiddleHandler>();
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
      
      public void FromSerialize(CharacterSerializeData d)
      {
        Name = d.name;
        Id = d.id;
        Gender = (CharacterGender)d.gender;
        Status = (CharacterStatus)d.status;
        // todo 
        //Mother. father, sppuse
        Info = new List<CharacterEpisodes>();
        foreach ( EpisodesSerialization s in d.episodes)
        {
          CharacterEpisodes ep = new CharacterEpisodes();
          ep.Content = s.content;
          ep.Title = s.episodeName;
          Info.Add(ep);
        }
      }
      public CharacterSerializeData ToSerialize()
      {
        CharacterSerializeData d = new CharacterSerializeData();
        d.name = Name;
        d.id = Id;
        d.gender = ((int)Gender);
        d.status = ((int)Status);
        d.episodes = new List<EpisodesSerialization>();
        foreach (CharacterEpisodes ep in Info)
        {
          EpisodesSerialization es = new EpisodesSerialization();
          es.episodeName = ep.Content;
          es.content = ep.Content;
          d.episodes.Add(es);
        }
        return d;
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
      CharacterEpisodes ep = new CharacterEpisodes();
      ep.Title = "Life";
      ep.Content = "";
      c.Info.Add(ep);
      Characters.Add(c);
      LastOne++;
      return c;
    }

    public void Load(String name)
    {
      Serializer.XmlBookLoad b = new Serializer.XmlBookLoad(FullPath);
      LoadCharacters(b);
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

    public void SetScene(SceneDescription sceneDescription)
    {
      Position.Clear();
      Position.Scene = sceneDescription;
      NotifyPropertyChanged("Position");
      NotifyPropertyChanged("CanGoFurther");
      NotifyPropertyChanged("CanGoBack");
    }

    ////////////////////////////////////////////////////////////////////
    //////////////////////////////Serialization/////////////////////////
    ////////////////////////////////////////////////////////////////////
    public struct EpisodesSerialization
    {
      public string episodeName;
      public string content;
    }
    public struct CharacterSerializeData
    {
      public String name;
      public int id;
      public int father;
      public int mother;
      public int spouse;
      public int gender;
      public int status;
      public List<EpisodesSerialization> episodes;
    }

    /// <summary>
    /// Base function for character serialization
    /// </summary>
    /// <param name="s"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    private List<CharacterSerializeData> SerializeCharacters(Serializer.BaseSerializer s, List<CharacterSerializeData> input)
    {
      string dummy = "";
      int children = s.PushSection(XmlNodeNames.Characters, ref dummy, ref dummy);
      List<CharacterSerializeData> ret = input;
      if (s.IsLoading)
      {
        ret = new List<CharacterSerializeData>(children);
      }

      for (int i = 0; i < ret.Count; i++)
      {
        CharacterSerializeData a1 = ret[i];
        s.SerializeString(XmlNodeNames.Character, ref a1.name);
        s.SerializeInt(XmlNodeNames.Father, ref a1.father);
        children = s.PushSection(XmlNodeNames.Episodes, ref dummy, ref dummy);
        if (s.IsLoading)
        {
          a1.episodes = new List<EpisodesSerialization>(children);
        }
        for (int iEp = 0; iEp < children; iEp++)
        {
          EpisodesSerialization es = a1.episodes[i];
          s.SerializeString(XmlNodeNames.Episode, ref es.episodeName);
          s.SerializeString(XmlNodeNames.EpisodeContent, ref es.content);
          a1.episodes[i] = es;
        }
        s.PopSection();
        ret[i] = a1;
      }
      s.PopSection();
      return ret;
    }

    private void LoadCharacters(Serializer.BaseSerializer s)
    {
      List<CharacterSerializeData> data = new List<CharacterSerializeData>();
      data = SerializeCharacters(s, data);
      foreach (CharacterSerializeData d in data)
      {
        CharacterContent c = new CharacterContent();
        c.FromSerialize(d);
        Characters.Add(c);
      }
    }
    private void SaveCharacters(Serializer.BaseSerializer s)
    {
      List<CharacterSerializeData> data = new List<CharacterSerializeData>();
      foreach (CharacterContent c in Characters)
      {
        CharacterSerializeData d = c.ToSerialize();
        data.Add(d);
      }
      SerializeCharacters(s, data);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////  Scenes  ////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////
    struct SceneSerializeData
    {
      public String name;
      public List<PageSerializeData> pages;
    }
    struct PageSerializeData
    {
      public String type;
      public int order;
      public IRiddleHandler handler;
    }
    private void SaveScenes(Serializer.BaseSerializer s)
    {
      List<SceneSerializeData> data = new List<SceneSerializeData>();
      foreach( SceneDescription sd in Scenes)
      {
        SceneSerializeData d = new SceneSerializeData();
        d.name = sd.Name;
        for(int i = 0; i < sd.Pages.Count; i++)
        {
          PageSerializeData pd = new PageSerializeData();
          pd.type = sd.Pages[i].Name;
          pd.order = i;
          pd.handler = sd.Pages[i];
        }
        data.Add(d);
      }
      SerializeScenes(s, data);
    }

    private List<SceneSerializeData> SerializeScenes( Serializer.BaseSerializer s, List<SceneSerializeData> data)
    {
      List<SceneSerializeData> ret = data;
      string dummy = "";
      int len = s.PushSection(XmlNodeNames.Scenes, ref dummy, ref dummy);
      if ( s.IsLoading )
      {
        ret = new List<SceneSerializeData>(len);
      }
      for ( int i =0; i < len; i++)
      {
        SceneSerializeData d = ret[i];
        s.SerializeString(XmlNodeNames.Scene, ref d.name);
        int pagesLen = s.PushSection(XmlNodeNames.Page, ref dummy, ref dummy);
        if(d.pages.Count!= pagesLen)
        {
          d.pages = new List<PageSerializeData>(pagesLen);
        }
        for(int iPage =0; iPage <pagesLen; iPage++)
        {
          PageSerializeData psd = d.pages[iPage];
          s.SerializeString(XmlNodeNames.Plugin, ref psd.type);
          if ( s.IsLoading )
          {
            AssemblyMap m = HandlersMap.Find(x => x.Name == psd.type);
            psd.handler = m.Assembly.CreateInstance(m.Type.ToString()) as IRiddleHandler;
          }
          psd.handler.Serialize(s);
        }
        s.PopSection();
        ret[i] = d;
      }
      s.PopSection();
      return ret;
    }

    /// <summary>
    /// Serialization to file itself
    /// </summary>
    public String FullPath
    {
      get => Settings.BooksFolder + "\\" + Name + "\\" + "base" + Constants.Extension;
    }
    public int Save()
    {
      if (!Directory.Exists(Settings.BooksFolder))
      {
        Directory.CreateDirectory(Settings.BooksFolder);
      }

      Serializer.XmlBookSave s = new Serializer.XmlBookSave(FullPath);
      SaveCharacters(s);
      SaveScenes(s);
      return 0;
    }
    public void Load()
    {
      Serializer.XmlBookLoad s = new Serializer.XmlBookLoad(FullPath);
      LoadCharacters(s);
    }
  }
}
