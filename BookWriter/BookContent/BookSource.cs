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
    /// Common nodes
    /// </summary>
    public const String Id = "Id";
    public const String Name = "Name";
    public const String Summary = "Summary";
    public const String Content = "Content";

    /// <summary>
    /// Documents
    /// </summary>
    public const String Documents = "Documents";
    public const String Document = "Document";
    public const String Misc = "Additional";

    /// <summary>
    /// World
    /// </summary>
    public const String Locations = "Locations";
    public const String Location = "Location";
    public const String MapSource = "MapImage"; 
    public const String Info = "Details"; 

    /// <summary>
    /// Scenes
    /// </summary>
    public const String Scenes = "Scenes";
    public const String Scene = "Scene";
    public const String Page = "Page";
    public const String Plugin = "Plugin";

    /// <summary>
    /// Character
    /// </summary>
    public const String Characters = "Characters";
    public const String Character = "Character";
    public const String Episodes = "Episodes";
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

    public SceneDescription CurrentScene { get; set; }
    

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
      NotifyPropertyChanged("Scenes");
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
    public List<DocumentDescription> Documents { get; set; }
    
    public void Init()
    {
      HandlersMap = new List<AssemblyMap>();
      InitPlugins();
      _scenes = new List<SceneDescription>();
      Characters = new List<CharacterDescription>();
      World = new List<WorldDescription>();
      Documents = new List<DocumentDescription>();
      CreateWorld();
      CreateDocument();
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
      world.Name = "New world";
      World.Add(world);
    }
    public void CreateDocument()
    {
      DocumentDescription document = new DocumentDescription();
      document.Name = "new document";
      Documents.Add(document);
    }
    public SceneDescription CreateScene()
    {
      SceneDescription d = new SceneDescription();
      d.Id = CreateId();
      Scenes.Add(d);
      d.Name = $"Scene #{Scenes.Count}";
      return d;
    }

    public void SetScene(SceneDescription sceneDescription)
    {
      if (sceneDescription != null)
      {
        if (sceneDescription.CurrentPage != null)
          sceneDescription.CurrentPage.Create();
      }
      CurrentScene = sceneDescription;
      RefreshContent();
    }

    ////////////////////////////////////////////////////////////////////
    //////////////////////////////Serialization/////////////////////////
    ////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////
    ////////////////////Documents Serialization/////////////////////////
    ////////////////////////////////////////////////////////////////////
    private delegate bool HasNextDocumentSection(Serializer.BaseSerializer a, int order, ref DocumentsSerializeData doc);
    private HasNextDocumentSection hasNextDocumentSection;
    private bool HasNextDocumentSectionLoad(Serializer.BaseSerializer a, int order, ref DocumentsSerializeData doc)
    {
      if (!a.PushSection(XmlNodeNames.Document, order))
        return false;

      if (order == 0)
      {
        doc.documents = new DocumentSerializeData[1];
        doc.documents[0] = new DocumentSerializeData();
        return true;
      }
      DocumentSerializeData[] temp = doc.documents;
      doc.documents = new DocumentSerializeData[temp.Length + 1];
      temp.CopyTo(doc.documents, 0);
      return true;
    }
    private bool HasNextDocumentSectionSave(Serializer.BaseSerializer a, int order, ref DocumentsSerializeData documents)
    {
      if (order >= documents.documents.Length)
        return false;
      return a.PushSection(XmlNodeNames.Document, order);
    }
    private void LoadDocuments(Serializer.XmlBookLoad s)
    {
      Documents.Clear();
      hasNextDocumentSection = HasNextDocumentSectionLoad;
      DocumentsSerializeData serializeData = new DocumentsSerializeData();
      SerializeDocuments(s, serializeData);
      if (serializeData.documents == null)
        return;
      for (int i =0; i <serializeData.documents.Length;i++)
      {
        DocumentDescription d = new DocumentDescription();
        d.FromSerialize(serializeData.documents[i]);
        Documents.Add(d);
      }
    }
    private void SaveDocuments(Serializer.XmlBookSave s)
    {
      hasNextDocumentSection = HasNextDocumentSectionSave;
      DocumentsSerializeData serializeData = new DocumentsSerializeData();
      serializeData.documents = new DocumentSerializeData[Documents.Count];
      for( int i =0; i < Documents.Count; i++)
      {
        serializeData.documents[i] = Documents[i].ToSerialize();
      }
      SerializeDocuments(s, serializeData);
    }
    public void SerializeDocuments(Serializer.BaseSerializer s, DocumentsSerializeData sd)
    {
      if (!s.PushSection(XmlNodeNames.Documents, 0))
        return;
      int order = 0;
      while (hasNextDocumentSection(s, order, ref sd))
      {
        ref DocumentSerializeData dd = ref sd.documents[order];
        s.SerializeString(XmlNodeNames.Summary, ref dd.summary);
        s.SerializeString(XmlNodeNames.Name, ref dd.name);
        s.SerializeString(XmlNodeNames.Content, ref dd.content);
        s.SerializeString(XmlNodeNames.Misc, ref dd.misc);
        order++;
        s.PopSection();
      }
      s.PopSection();
    }
    ////////////////////////////////////////////////////////////////////
    ///////////////////////////////World Serialization//////////////////
    ////////////////////////////////////////////////////////////////////
    private delegate bool HasNextLocationSection(Serializer.BaseSerializer a, int order, ref LocationsSerializeData location);
    private HasNextLocationSection hasNextLocationSection;

    private bool HasNextLocationSectionSave(Serializer.BaseSerializer a, int order, ref LocationsSerializeData locations)
    {
      if (order >= locations.locatios.Length)
        return false;
      return a.PushSection(XmlNodeNames.Location, order);
    }
    private bool HasNextLocationSectionLoad(Serializer.BaseSerializer a, int order, ref LocationsSerializeData location)
    {
      if (!a.PushSection(XmlNodeNames.Location,order))
        return false;
      if ( order == 0)
      {
        location.locatios = new LocationSerializeData[order + 1];
        location.locatios[0] = new LocationSerializeData();
        return true;
      }
      LocationSerializeData[] d = location.locatios; 
      location.locatios = new LocationSerializeData[order + 1];
      d.CopyTo(location.locatios, 0);
      location.locatios[order] = new LocationSerializeData();
      return true;
    }

    private delegate bool HasNextInfoSection(Serializer.BaseSerializer a, int order, ref LocationSerializeData location);
    private HasNextInfoSection hasNextInfoSection;

    private bool HasNextInfoSectionSave(Serializer.BaseSerializer a, int order, ref LocationSerializeData place)
    {
      if (order >= place.details.Length)
        return false;
      return a.PushSection(XmlNodeNames.Info, order);
    }
    private bool HasNextInfoSectionLoad(Serializer.BaseSerializer a, int order, ref LocationSerializeData place)
    {
      if (!a.PushSection(XmlNodeNames.Info, order))
        return false;
      MapSerializeData[] data = place.details;
      place.details = new MapSerializeData[order + 1];
      data.CopyTo(place.details, 0);
      place.details[order] = new MapSerializeData();
      return true;
    }

    private delegate bool HasNextMapSection(Serializer.BaseSerializer a, int order, ref LocationSerializeData location);
    private HasNextMapSection hasNextMapSection;

    private bool HasNextMapSectionSave(Serializer.BaseSerializer a, int order, ref LocationSerializeData place)
    {
      if (order >= place.maps.Length)
        return false;
      return a.PushSection(XmlNodeNames.MapSource, order);
    }
    private bool HasNextMapSectionLoad(Serializer.BaseSerializer a, int order, ref LocationSerializeData place)
    {
      if (!a.PushSection(XmlNodeNames.MapSource, order))
        return false;
      MapSerializeData[] data = place.maps;
      place.details = new MapSerializeData[order + 1];
      data.CopyTo(place.maps, 0);
      place.details[order] = new MapSerializeData();
      return true;
    }
    private void LoadWorld(Serializer.BaseSerializer s)
    {
      World.Clear();
      hasNextLocationSection = HasNextLocationSectionLoad;
      hasNextInfoSection = HasNextInfoSectionLoad;
      hasNextMapSection = HasNextMapSectionLoad;

      LocationsSerializeData data = new LocationsSerializeData();
      SerializeWorld(s, ref data);
      // load indie World
      if (data.locatios == null)
        return;
      for ( int i =0; i < data.locatios.Length; i++)
      {
        WorldDescription d = new WorldDescription();
        d.FromSerialize(data.locatios[i]);
        World.Add(d);
      }
    }
    private void SaveWorld(Serializer.BaseSerializer s)
    {
      hasNextLocationSection = HasNextLocationSectionSave;
      hasNextInfoSection = HasNextInfoSectionSave;
      hasNextMapSection = HasNextMapSectionSave;

      LocationsSerializeData data = new LocationsSerializeData();
      data.locatios = new LocationSerializeData[World.Count];
      for (int i = 0; i < World.Count; i++)
      {
        data.locatios[i] = World[i].ToSerialize();
      }
      SerializeWorld(s, ref data);
    } 
    private void SerializeWorld(Serializer.BaseSerializer s, ref LocationsSerializeData d)
    {
      if (!s.PushSection(XmlNodeNames.Locations, 0))
        return;
      int order = 0;
      while (hasNextLocationSection(s,order, ref d))
      {
        ref LocationSerializeData sd = ref d.locatios[order];
        s.SerializeAttribute(XmlNodeNames.Name, ref sd.name);
        int mapOrder = 0;
        while (hasNextMapSection(s,mapOrder,ref sd))
        {
          ref MapSerializeData mp = ref sd.maps[mapOrder];
          s.PushSection(XmlNodeNames.MapSource,0);
          s.SerializeAttribute(XmlNodeNames.Name, ref mp.name);
          s.SerializeString(ref mp.content);
          s.PopSection();
          mapOrder++;
        }
        mapOrder = 0;
        while (hasNextInfoSection(s, mapOrder, ref sd))
        {
          ref MapSerializeData mp = ref sd.details[mapOrder];
          s.PushSection(XmlNodeNames.Info,0);
          s.SerializeAttribute(XmlNodeNames.Name, ref mp.name);
          s.SerializeString(ref mp.content);
          s.PopSection();
          mapOrder++;
        }
        order++;
        s.PopSection();
      }
      s.PopSection();
    }
    ////////////////////////////////////////////////////////////////////
    ///////////////////////////Character Serialization//////////////////
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
    private int ToId(object o)
    {
      SceneDescription d = o as SceneDescription;
      if (d == null)
        return -1;
      if (_id < d.Id)
        _id = d.Id;
      return d.Id;
    }
    private object FromId(int o)
    {
      for (int i =0; i < Scenes.Count; i++)
      {
        if (Scenes[i].Id == o)
          return Scenes[i];
      }
      return null;
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
      Characters.Clear();
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
      Scenes.Clear();
      hasNextScene = HasNextSceneLoad;
      hasNextPage = HasNextPageLoad;
      ScenesSerializeData data = new ScenesSerializeData();
      SerializeScenes(s, ref data);
      Scenes = new List<SceneDescription>();
      for (int iScene = 0; iScene < data.scenes.Length; iScene++)
      {
        SceneDescription sd = new SceneDescription();
        ref SceneSerializeData d = ref data.scenes[iScene];
        sd.Name = d.name;
        sd.Id = Int32.Parse(d.id);
        if (d.pages != null)
        {
          for (int iPage = 0; iPage < d.pages.Length; iPage++)
          {
            ref PageSerializeData pd = ref d.pages[iPage];
            pd.handler.BaseFolder = Settings.BooksFolder;
            sd.Pages.Add(pd.handler);
          }
          sd.SetPage(0);
        }
        Scenes.Add(sd);
      }
      foreach (SceneDescription sd in Scenes)
      {
        foreach(IRiddleHandler h in sd.Pages)
        {
          h.Finish(s);
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
        s.SerializeAttribute(XmlNodeNames.Id, ref d.id);
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
      s.toId = ToId;
      s.fromId = FromId;
      s.PushSection(XmlNodeNames.BookRoot, 0);
      SaveCharacters(s);
      SaveScenes(s);
      SaveWorld(s);
      SaveDocuments(s);
      s.PopSection();
      s.Finish();
    }
   
    public void Load(string name)
    {
      Name = name;
      Serializer.XmlBookLoad s = new Serializer.XmlBookLoad(FullPath);
      s.toId = ToId;
      s.fromId = FromId;
      s.PushSection(XmlNodeNames.BookRoot, 0);
      LoadCharacters(s);
      LoadScenes(s);
      LoadWorld(s);
      LoadDocuments(s);
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
    int _id = 0;
    private int CreateId()
    {
      _id++;
      return _id;
    }
    public void CreateScene(string name)
    {
      SceneDescription d = CreateScene();
      d.Id = CreateId();
      d.Name = name;
    }
  }
}
