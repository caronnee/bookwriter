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
    private void Serialize(Serializer.BaseSerializer s)
    {
      string dummy = "";
      int len = s.PushSection("Riddles", ref dummy, ref dummy);
      if (s.IsLoading == false )
      {
        len = Handlers.Count;
      }
      for (int i = 0; i < len; i++)
      {
        string attName = "id";
        string att =Handlers[i].Name;
        s.PushSection("Riddle", ref attName, ref att);
        IRiddleHandler correct = Handlers.Find(new Predicate<IRiddleHandler>(x => x.Name == att));
        correct.Serialize(s);
        s.PopSection();
      }
      s.PopSection();
    }

    public void Save( string filename)
    {
      Serializer.XmlBookSave saver = new Serializer.XmlBookSave(filename);
      Serialize(saver);
      saver.Finish();
    }
    public void Load( string filename)
    {
      Serializer.XmlBookLoad saver = new Serializer.XmlBookLoad(filename);
      Serialize(saver);
    }
  }
}

