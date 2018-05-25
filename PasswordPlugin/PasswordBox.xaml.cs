using System.Windows;
using System.Windows.Controls;

namespace PasswordPlugin
{
  /// <summary>
  /// Interaction logic for PasswordBox.xaml
  /// </summary>
  public partial class PasswordBox : UserControl
  {
    public PasswordBox()
    {
      InitializeComponent();
    }

    // delegate
    delegate void CheckHandler();
    CheckHandler onCheckHander;

    private void Answer_PasswordChanged(object sender, RoutedEventArgs e)
    {

    }

    private void verifyClick(object sender, RoutedEventArgs e)
    {
      if (onCheckHander != null)
        onCheckHander();
    }
  }
}
