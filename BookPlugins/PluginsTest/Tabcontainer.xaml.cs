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
    public SceneTest Handler { get; set; }
    public Tabcontainer(SceneTest handler)
    {
      Handler = handler;
      InitializeComponent();
      Header = handler.page.Name;
      Preview();
    }

    private void Preview()
    {
      x_show.Content = Handler.page.DisplayPage;
    }

    private void x_make_preview(object sender, RoutedEventArgs e)
    {
      Preview();
    }
  }
}
