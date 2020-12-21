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
    public Tabcontainer(IRiddleHandler handler)
    {
      DataContext = handler;
      InitializeComponent();
      Header = handler.Name;
      Preview();
    }

    private void Preview()
    {
      IRiddleHandler h = DataContext as IRiddleHandler;
      h.ClearAnswer();
      h.CreateReadOnly();
      x_show.Content = h.DisplayPage;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      Preview();
    }
  }
}
