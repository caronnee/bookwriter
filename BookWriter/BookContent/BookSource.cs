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
    public const String Name = "Name";
    public const String Episodes = "Episodes";
    public const String Episode = "Episode";
    public const String EpisodeContent = "EpisodeContent";
    public const String Summary = "Summary";
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

  public class BookSource : INotifyPropertyChanged, ISceneProvider
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

    public SceneDescription CurrentScene { get; set; }
    

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

    public CharacterDescription Get(string name)
    {
      return Characters.Find(x => x.Name == name);
    }

    public void RemovePage()
    {
      CurrentScene.Pages.RemoveAt(CurrentScene.CurrentPosition);
      MoveBack();
    }

    // name of the book 
    public string Name
    {
      get;
      set;
    }

    public void MoveBack()
    {
      if (CurrentScene.CurrentPosition > 0)
        CurrentScene.SetPage(CurrentScene.CurrentPosition-1);
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
      h.BaseFolder = Settings.BooksFolder;
      h.SceneProvider = this;
      h.Create();
      if (CurrentScene.CurrentPosition <= CurrentScene.Pages.Count)
      {
        CurrentScene.Pages.Add(h);
        CurrentScene.SetPage(0);
      }
      else
      {
        CurrentScene.Pages.Insert(CurrentScene.CurrentPosition +1, h);
        MoveForward();
      }
      RefreshContent();
    }
    public void RefreshContent()
    {
      NotifyPropertyChanged("CanGoBack");
      NotifyPropertyChanged("CanGoFurther");
      NotifyPropertyChanged("CurrentScene");
    }

    public void MoveForward()
    {
      CurrentScene.SetPage(CurrentScene.CurrentPosition + 1);
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

    public List<CharacterDescription> Characters { get; set; }
    public List<WorldDescription> World { get; set; }
    public void Init()
    {
      HandlersMap = new List<AssemblyMap>();
      InitPlugins();
      _scenes = new List<SceneDescription>();
      Characters = new List<CharacterDescription>();
      World = new List<WorldDescription>();
      CreateWorld();
      SceneDescription d = CreateScene();
      SetScene(d);
      // create first person
      CreateCharacter();
    }

    int LastOne = 0;

    public CharacterDescription CreateCharacter()
    {
      CharacterDescription c = new CharacterDescription();
      c.Id = LastOne;
      c.Name = "Anonymous";
      CharacterEpisodes ep = new CharacterEpisodes();
      ep.Title = "Life";
      ep.Content = "";
      c.Episodes.Add(ep);
      Characters.Add(c);
      LastOne++;
      return c;
    }

    public bool CanGoBack
    {
      get { 
        return CurrentScene!=null && CurrentScene.CurrentPosition > 0; 
      }
    }

    public bool CanGoFurther
    {
      get
      {
        if (CurrentScene == null)
          return false;
        if (CurrentScene.CurrentPage == null)
          return false;
        return ( CurrentScene.CurrentPosition < CurrentScene.Pages.Count - 1 );
      }
    }

    public BookSource()
    {
      // init
      Name = null;
      Init();
    }
    public void CreateWorld()
    {
      WorldDescription world = new WorldDescription();
      world.Name = "Test world";
      World.Add(world);
    }
    public SceneDescription CreateScene()
    {
      SceneDescription d = new SceneDescription();
      Scenes.Add(d);
      d.Name = $"Scene #{Scenes.Count}";
      return d;
    }

    public void SetScene(SceneDescription sceneDescription)
    {
      CurrentScene = sceneDescription;
      NotifyPropertyChanged("CanGoFurther");
      NotifyPropertyChanged("CanGoBack");
    }

    ////////////////////////////////////////////////////////////////////
    //////////////////////////////Serialization/////////////////////////
    ////////////////////////////////////////////////////////////////////
    
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
        s.SerializeString(XmlNodeNames.Summary, ref a1.summary);
        int iEpisode = 0;
        while (hasNextEpisodeSection(s, iEpisode, ref a1))
        {
          ref EpisodesSerialization es = ref a1.episodes[iEpisode];
          s.SerializeAttribute(XmlNodeNames.Name, ref es.episodeName);
          s.SerializeString(ref es.content);
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
        CharacterDescription c = new CharacterDescription();
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
      s.SerializeAttribute(XmlNodeNames.Plugin, ref name);
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
        data.scenes[iScene] = Scenes[iScene].ToSerialize();
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
          pd.handler.BaseFolder = Settings.BooksFolder;
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
    public void Save()
    {
      Serializer.XmlBookSave s = new Serializer.XmlBookSave(FullPath);
      s.PushSection(XmlNodeNames.BookRoot, 0);
      SaveCharacters(s);
      SaveScenes(s);
      s.PopSection();
      s.Finish();
    }
    public void Load(string name)
    {
      Name = name;
      Serializer.XmlBookLoad s = new Serializer.XmlBookLoad(FullPath);
      s.PushSection(XmlNodeNames.BookRoot, 0);
      LoadCharacters(s);
      LoadScenes(s);
      s.PopSection();
    }

    public List<string> GetSceneNames()
    {
      List<string> ret = new List<string>();
      for (int i =0; i < Scenes.Count; i++)
      {
        ret.Add(Scenes[i].Name);
      }
      return ret;
    }

    public int GetSceneId(string name)
    {
      throw new NotImplementedException();
    }

    public void CreateScene(string name)
    {
      SceneDescription d = CreateScene();
      d.Name = name;
    }
  }
}
