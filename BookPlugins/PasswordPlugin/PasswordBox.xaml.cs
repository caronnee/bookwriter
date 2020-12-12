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
    public delegate void CheckHandler();
    public event CheckHandler onCheckHander;
    
    private void verifyClick(object sender, RoutedEventArgs e)
    {
      if (onCheckHander != null)
        onCheckHander();
    }
  }
}
