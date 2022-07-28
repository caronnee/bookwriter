using System.Windows;
using System.Windows.Controls;

namespace DecisionPlugin
{
  /// <summary>
  /// Interaction logic for SingleDecision.xaml
  /// </summary>
  public partial class SingleDecision : UserControl
  {
    public DecisionPossibilities Decision { get; set; }
    public SingleDecision( DecisionPossibilities p)
    {
      Decision = p;
      InitializeComponent();
    }

    private void x_crossroad_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      //MessageBox.Show("changed");
    }
  }
}
