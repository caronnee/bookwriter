using RiddleInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace PluginsTest
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    // possible outcomes
    [XmlIgnore]
    List<Outcome> Locations { get; set; }
    [XmlElement]
    List<IRiddleHandler> Handlers { get; set; }
    public MainWindow()
    {
      // all handlers that can be downloaded from dll
      InitializeComponent();
      Locations = new List<Outcome>();

      Locations.Add(new Outcome() { Name = "Page 0", Id = 0 });
      Locations.Add(new Outcome() { Name = "Page 1", Id = 1 });
      Locations.Add(new Outcome() { Name = "Page 2", Id = 2 });
      Locations.Add(new Outcome() { Name = "Page 3", Id = 3 });
      Handlers = InitPlugins();
      RefreshAll();
    }
    
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
          riddle.Outcomes = Locations;
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

    private void ProcessAnswer(int id)
    {
      x_info.Text = $"Chosen outcome {id}";
    }

    private void RefreshAll()
    {
      x_test.Items.Clear();
      for (int i = 0; i < Handlers.Count; i++)
      {
        Tabcontainer cont = new Tabcontainer(Handlers[i]);
        Handlers[i].onAnswer += ProcessAnswer;
        x_test.Items.Add(cont);
      }
    }
    
    private void x_load_click(object sender, RoutedEventArgs e)
    {
      Assembly assem = Assembly.GetExecutingAssembly();
      Uri ur = new Uri(assem.CodeBase);
      FileInfo fi = new FileInfo(ur.AbsolutePath);
      string s = fi.Directory.FullName + "TestBook.book";
      BookLoader f = new BookLoader(s);

      f.StartSection(AppNodeNames.RiddlesString);
      int c = f.Children();
      for ( int i =0; i < c; i++)
      {
        f.LoadSection(i);
        foreach(IRiddleHandler h in Handlers)
        {
          if (h.Load(f))
            break;
        }
        f.EndSection();
      }
      f.EndSection();
      f.Close();
      MessageBox.Show($"Load succesfull to {s}");
      RefreshAll();
    }
    
    private void SaveClick(object sender, RoutedEventArgs e)
    {
      Assembly assem = Assembly.GetExecutingAssembly();
      Uri ur = new Uri(assem.CodeBase);
      FileInfo fi = new FileInfo(ur.AbsolutePath);
      string s = fi.Directory.FullName + "TestBook.book";
      BookSaver ms = new BookSaver(s);
      ms.StartSection(AppNodeNames.RiddlesString);
      foreach (IRiddleHandler h in Handlers)
      {
        ms.StartSection(AppNodeNames.RiddleString);
        h.Save(ms);
        ms.EndSection();
      }
      ms.EndSection();
      ms.Close();

      MessageBox.Show($"Save succesfull to {s}");
    }
  }
}
