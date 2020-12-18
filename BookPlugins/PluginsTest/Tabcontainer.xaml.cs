using RiddleInterface;
using System;
using System.Collections.Generic;
using System.Linq;
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
  /// Interaction logic for Tabcontainer.xaml
  /// </summary>
  public partial class Tabcontainer : TabItem
  {
    private IRiddleHandler _interface;

    public Tabcontainer(IRiddleHandler handler)
    {
      InitializeComponent();
      _interface = handler;
      _interface.Create();
      Header = _interface.Name;
      x_maker.Content = _interface.Viewport;
      x_show.Content = _interface.CreateRiddle().ConvertToReadonly();
    }
    private void Preview()
    {
      x_show.Content = _interface.CreateRiddle().ConvertToReadonly();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      Preview();
    }
  }
}
