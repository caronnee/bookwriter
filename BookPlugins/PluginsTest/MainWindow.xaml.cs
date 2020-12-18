using RiddleInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PluginsTest
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    
    public MainWindow()
    {
      // all handlers that can be downloaded from dll
      InitializeComponent();
      List<IRiddleHandler> Handlers = InitPlugins();
      for ( int i =0; i < Handlers.Count; i++)
      {
        Tabcontainer cont = new Tabcontainer(Handlers[i]);
        
        x_test.Items.Add(cont);
      }
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
          if (riddle != null)
            riddles.Add(riddle);
        }
      }

      return riddles;
    }
  }
}
