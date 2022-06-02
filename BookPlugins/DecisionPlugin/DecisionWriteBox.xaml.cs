using RiddleInterface;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace DecisionPlugin
{
  /// <summary>
  /// Interaction logic for DecisionWriteBox.xaml
  /// </summary>
  public partial class DecisionWriteBox : UserControl
  {
    public DecisionWriteBox()
    {
      InitializeComponent();
    }

    private void InitCrossroad(SingleDecision sd, DecisionCreator d, DecisionPossibilities p)
    {
      sd.x_crossroad.ItemsSource = d.SceneProvider.GetSceneNames();
      sd.DataContext = p;
    }

    private void x_add_decision_Click(object sender, RoutedEventArgs e)
    {
      DecisionData d = (DataContext as DecisionCreator).Data;
      DecisionPossibilities p = new DecisionPossibilities();
      d.Posibilities.Add(p);
      AddDecision(p);
    }
    public void AddDecision(DecisionPossibilities p)
    {
      int t = x_decisions.Children.Count-1;
      SingleDecision d = new SingleDecision();
      x_decisions.Children.Insert(t,d);
      x_remove_decision.IsEnabled = true;
      DecisionCreator dc = DataContext as DecisionCreator;
      InitCrossroad(d, dc, p);
    }

    private void x_remove_decision_Click(object sender, RoutedEventArgs e)
    {
      int t = x_decisions.Children.Count-2;
      DecisionData d = (DataContext as DecisionCreator).Data;
      d.Posibilities.RemoveAt(t);
      x_decisions.Children.RemoveAt(t);
      x_remove_decision.IsEnabled = x_decisions.Children.Count > 2;
    }
  }
}
