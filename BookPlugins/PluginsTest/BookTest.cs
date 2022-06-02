using RiddleInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PluginsTest
{
  public class BookTest
  {
    class TestScene : ISceneProvider
    {
      List<string> s = new List<string>() { "aselfjk", "dva", "tri" };

      public void CreateScene(string name)
      {
        s.Add(name);
      }

      public int GetSceneId(string name)
      {
        for ( int i =0; i < s.Count; i++)
        {
          if (s[i] == name)
            return i;
        }
        return -1;
      }

      public List<string> GetSceneNames()
      {
        return s;
      }
    }
    public List<IRiddleHandler> Handlers { get; set; }

    private List<IRiddleHandler> InitPlugins()
    {
      Assembly assem = Assembly.GetExecutingAssembly();
      Uri ur = new Uri(assem.CodeBase);
      FileInfo fi = new FileInfo(ur.AbsolutePath);
      string s = fi.Directory.FullName;

      //find all dlls
      string[] dlls = Directory.GetFiles(s, "*.dll");
      ICollection<Assembly> assemblies = new List<Assembly>(dlls.Length);
      List<IRiddleHandler> riddles = new List<IRiddleHandler>();

      foreach (string dllFile in dlls)
      {
        AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
        Assembly assembly = Assembly.Load(an);
        assemblies.Add(assembly);
      }

      Type rh = typeof(IRiddleHandler);
      TestScene mySceneProvider = new TestScene();
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
          //riddle.Outcomes = Locations;
          riddle.BaseFolder = s;
          riddle.SceneProvider = mySceneProvider;
          if (riddle != null)
          {
            riddles.Add(riddle);
            riddle.Create();
          }
        }
      }
      return riddles;
    }

    public BookTest()
    {
      Handlers = InitPlugins();
    }

    public delegate IRiddleHandler HasNextSession(Serializer.BaseSerializer s, int order);
    public HasNextSession hasNextSession;

    IRiddleHandler HasNextSessionLoad(Serializer.BaseSerializer s, int order)
    {
      if (!s.PushSection("Riddle", order))
        return null;
      string name = "";
      s.SerializeAttribute("Plugin", ref name);
      return Handlers.Find(new Predicate<IRiddleHandler>(x => x.Name == name));
    }
    IRiddleHandler HasNextSessionSave(Serializer.BaseSerializer s, int order)
    {
      if (order < Handlers.Count)
      {
        IRiddleHandler irh = Handlers[order];
        s.PushSection("Riddle", order);
        string name = irh.Name;
        s.SerializeAttribute("Plugin", ref name);
        return Handlers[order];
      }
      return null;
    }
    private void Serialize(Serializer.BaseSerializer s)
    {
      // we know it will be just one
      s.PushSection("Riddles", 0);
      int order = 0;
      IRiddleHandler correct;
      while ((correct = hasNextSession(s, order))!=null)
      {
        //Handlers.Find(new Predicate<IRiddleHandler>(x => x.Name == name));
        correct.Serialize(s);
        s.PopSection();
        order++;
      }
      s.PopSection();
    }

    public void Save( string filename)
    {
      Serializer.XmlBookSave saver = new Serializer.XmlBookSave(filename);
      hasNextSession = HasNextSessionSave;
      Serialize(saver);
      saver.Finish();
    }
    public void Load( string filename)
    {
      Serializer.XmlBookLoad loader = new Serializer.XmlBookLoad(filename);
      hasNextSession = HasNextSessionLoad;
      Serialize(loader);
      foreach(IRiddleHandler h in Handlers)
      {
        h.Create();
      }
    }
  }
}

