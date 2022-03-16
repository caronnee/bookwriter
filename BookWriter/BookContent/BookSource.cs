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
        foreach (EpisodesSerialization s in d.episodes)
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
        d.episodes = new EpisodesSerialization[this.Info.Count];
        for (int i = 0; i < Info.Count; i++)
        {
          EpisodesSerialization es = new EpisodesSerialization();
          es.episodeName = Info[i].Content;
          es.content = Info[i].Content;
          d.episodes[i] = es;
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

    public List<CharacterContent> Characters { get; set; }

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
      get
      {
        return (Position.Scene.Pages.Count > 0)
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
      public EpisodesSerialization[] episodes;
    }

    public struct CharactersSerializeData
    {
      public CharacterSerializeData[] characters;
    }

    private delegate bool HasNextCharacterSection(Serializer.BaseSerializer a, int order, ref CharactersSerializeData characters);
    private HasNextCharacterSection hasNextCharacterSection;
    private bool HasNextCharacterSectionLoad(Serializer.BaseSerializer a, int order, ref CharactersSerializeData characters)
    {
      if (!a.PushSection(XmlNodeNames.Character, order))
        return false;
      CharacterSerializeData[] chrs = new CharacterSerializeData[order + 1];
      if (order > 0)
      {
        characters.characters.CopyTo(chrs, 0);
      }
      characters.characters = chrs;
      return true;
    }

    private bool HasNextCharacterSectionSave(Serializer.BaseSerializer a, int order, ref CharactersSerializeData characters)
    {
      if (order >= characters.characters.Length)
        return false;
      if (!a.PushSection(XmlNodeNames.Character, order))
        return false;
      return true;
    }

    private delegate bool HasNextEpisodeSection(Serializer.BaseSerializer a, int order, ref CharacterSerializeData character);
    private HasNextEpisodeSection hasNextEpisodeSection;
    private bool HasNextEpisodeSectionLoad(Serializer.BaseSerializer a, int order, ref CharacterSerializeData character)
    {
      if (!a.PushSection(XmlNodeNames.Episode, order))
        return false;
      EpisodesSerialization[] es = new EpisodesSerialization[order + 1];
      if (order > 0)
      {
        character.episodes.CopyTo(es, 0);
      }
      character.episodes = es;
      return true;
    }
    private bool HasNextEpisodeSectionSave(Serializer.BaseSerializer a, int order, ref CharacterSerializeData character)
    {
      if (order >= character.episodes.Length)
        return false;
      if (!a.PushSection(XmlNodeNames.Episode, order))
        return false;
      return true;
    }
    /// <summary>
    /// Base function for character serialization
    /// </summary>
    /// <param name="s"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    private void SerializeCharacters(Serializer.BaseSerializer s, ref CharactersSerializeData characters)
    {
      if (!s.PushSection(XmlNodeNames.Characters, 0))
        return;

      int iCharacters = 0;
      while (hasNextCharacterSection(s, iCharacters, ref characters))
      {
        ref CharacterSerializeData a1 = ref characters.characters[iCharacters];
        s.SerializeString(XmlNodeNames.Character, ref a1.name);
        s.SerializeInt(XmlNodeNames.Father, ref a1.father);
        int iEpisode = 0;
        while (hasNextEpisodeSection(s, iEpisode, ref a1))
        {
          ref EpisodesSerialization es = ref a1.episodes[iEpisode];
          s.SerializeString(XmlNodeNames.Episode, ref es.episodeName);
          s.SerializeString(XmlNodeNames.EpisodeContent, ref es.content);
          s.PopSection();
          iEpisode++;
        }
        s.PopSection();
        iCharacters++;
      }
      s.PopSection();
    }

    private void LoadCharacters(Serializer.BaseSerializer s)
    {
      hasNextEpisodeSection = HasNextEpisodeSectionLoad;
      hasNextCharacterSection = HasNextCharacterSectionLoad;
      CharactersSerializeData data = new CharactersSerializeData();
      SerializeCharacters(s, ref data);
      foreach (CharacterSerializeData d in data.characters)
      {
        CharacterContent c = new CharacterContent();
        c.FromSerialize(d);
        Characters.Add(c);
      }
    }
    private void SaveCharacters(Serializer.BaseSerializer s)
    {
      hasNextEpisodeSection = HasNextEpisodeSectionSave;
      hasNextCharacterSection = HasNextCharacterSectionSave;
      CharactersSerializeData data = new CharactersSerializeData();
      data.characters = new CharacterSerializeData[Characters.Count];
      for (int i = 0; i < Characters.Count; i++)
      {
        CharacterSerializeData d = Characters[i].ToSerialize();
        data.characters[i] = d;
      }
      SerializeCharacters(s, ref data);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////  Scenes  ////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////
    struct PageSerializeData
    {
      public String type;
      public int order;
      public IRiddleHandler handler;
    }
    struct SceneSerializeData
    {
      public String name;
      public PageSerializeData[] pages;
    }
    struct ScenesSerializeData
    {
      public SceneSerializeData[] scenes;
    }

    private delegate bool HasNextScene(Serializer.BaseSerializer s, int order, ref ScenesSerializeData scenes);
    private HasNextScene hasNextScene;
    bool HasNextSceneLoad(Serializer.BaseSerializer s, int order, ref ScenesSerializeData scenes)
    {
      if (!s.PushSection(XmlNodeNames.Scene, order))
        return false;
      SceneSerializeData[] sd = new SceneSerializeData[order + 1];
      if (order > 0)
      {
        scenes.scenes.CopyTo(sd, 0);
      }
      sd[order] = new SceneSerializeData();
      scenes.scenes = sd;
      return true;
    }
    bool HasNextSceneSave(Serializer.BaseSerializer s, int order, ref ScenesSerializeData scenes)
    {
      if (order >= scenes.scenes.Length)
        return false;
      if (!s.PushSection(XmlNodeNames.Scene, order))
        return false;
      return true;
    }

    private delegate bool HasNextPage(Serializer.BaseSerializer s, int order, ref SceneSerializeData d);
    private HasNextPage hasNextPage;

    private bool HasNextPageLoad(Serializer.BaseSerializer s, int order, ref SceneSerializeData d)
    {
      if (!s.PushSection(XmlNodeNames.Page, order))
        return false;
      PageSerializeData[] data = new PageSerializeData[order + 1];
      if (order > 0)
      {
        d.pages.CopyTo(data, 0);
      }
      PageSerializeData psd = new PageSerializeData();
      d.pages = data;
      string plugin = "";
      s.SerializeAttribute(XmlNodeNames.Plugin, ref plugin);

      AssemblyMap m = HandlersMap.Find(x => x.Name == plugin);
      psd.handler = m.Assembly.CreateInstance(m.Type.ToString()) as IRiddleHandler;
      psd.type = plugin;
      data[order] = psd;
      return true;
    }
    private bool HasNextPageSave(Serializer.BaseSerializer s, int order, ref SceneSerializeData d)
    {
      if (order >= d.pages.Length)
        return false;
      if (!s.PushSection(XmlNodeNames.Page, order))
        return false;
      string name = d.pages[order].type;
      AssemblyMap m = HandlersMap.Find(x => x.Name == name);
      IRiddleHandler h = m.Assembly.CreateInstance(m.Type.ToString()) as IRiddleHandler;
      d.pages[order].handler = h;
      if (h == null)
      {
        s.PopSection();
        return false;
      }
      return true;
    }
    private void SaveScenes(Serializer.BaseSerializer s)
    {
      hasNextScene = HasNextSceneSave;
      hasNextPage = HasNextPageSave;
      ScenesSerializeData data = new ScenesSerializeData();
      data.scenes = new SceneSerializeData[Scenes.Count];
      for (int iScene = 0; iScene < Scenes.Count; iScene++)
      {
        SceneDescription sd = Scenes[iScene];
        SceneSerializeData d = new SceneSerializeData();
        d.name = sd.Name;
        for (int i = 0; i < sd.Pages.Count; i++)
        {
          PageSerializeData pd = new PageSerializeData();
          pd.type = sd.Pages[i].Name;
          pd.order = i;
          pd.handler = sd.Pages[i];
        }
        data.scenes[iScene] = d;
      }
      SerializeScenes(s, ref data);
    }

    private void LoadScenes(Serializer.BaseSerializer s)
    {
      hasNextScene = HasNextSceneLoad;
      hasNextPage = HasNextPageLoad;
      ScenesSerializeData data = new ScenesSerializeData();
      SerializeScenes(s, ref data);
      Scenes = new List<SceneDescription>();
      data.scenes = new SceneSerializeData[Scenes.Count];
      for (int iScene = 0; iScene < data.scenes.Length; iScene++)
      {
        SceneDescription sd = new SceneDescription();
        ref SceneSerializeData d = ref data.scenes[iScene];
        sd.Name = d.name;
        for (int iPage = 0; iPage < d.pages.Length; iPage++)
        {
          ref PageSerializeData pd = ref d.pages[iPage];
          sd.Pages.Add( pd.handler );
        }
      }
    }
    private void SerializeScenes(Serializer.BaseSerializer s, ref ScenesSerializeData data)
    {
      if (!s.PushSection(XmlNodeNames.Scenes, 0))
        return;
      int iScene = 0;
      while (hasNextScene(s, iScene, ref data))
      {
        ref SceneSerializeData d = ref data.scenes[iScene];
        s.SerializeString(XmlNodeNames.Scene, ref d.name);
        int iPage = 0;
        while (hasNextPage(s, iPage, ref d))
        {
          ref PageSerializeData psd = ref d.pages[iPage];
          s.SerializeString(XmlNodeNames.Plugin, ref psd.type);
          psd.handler.Serialize(s);
          iPage++;
        }
        s.PopSection();
        iScene++;
      }
      s.PopSection();
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
      LoadScenes(s);
    }
  }
}
