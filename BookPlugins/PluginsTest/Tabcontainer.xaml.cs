using RiddleInterface;
using System.Windows;
using System.Windows.Controls;

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

    private void x_make_preview(object sender, RoutedEventArgs e)
    {
      Preview();
    }
  }
}
