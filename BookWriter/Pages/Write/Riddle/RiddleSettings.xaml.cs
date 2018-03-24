using MyBook.BookContent;
using RiddleInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyBook.Pages.Write.Riddle
{
  /// <summary>
  /// Interaction logic for RiddleSettings.xaml
  /// </summary>
  public partial class RiddleSettings : UserControl
  {
    public struct PluginInfo
    {
      public String Name { get; set; }
    }
    public List<PluginInfo> Plugins;
    public RiddleSettings()
    {
      // find all dlls
      InitializeComponent();
      DataContext = new RiddleParagraph();
    }

    public IContent Create()
    {
      return new RiddleParagraph();
    }

    public void Reset()
    {
      // nothing to do right now
    }
  }
}
