using System.Windows.Controls;

namespace PasswordPlugin
{
  /// <summary>
  /// Interaction logic for TextSetting.xaml
  /// </summary>
  public partial class PasswordSetting : UserControl
  {
    public PasswordSetting()
    {
      InitializeComponent();
    }

    private void newHint_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      hintPanel.Children.Insert(0, new HintItem());
    }
  }
}
