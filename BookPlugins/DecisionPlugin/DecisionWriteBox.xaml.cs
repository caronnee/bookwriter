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
      DataContextChanged += DecisionWriteBox_DataContextChanged;
    }

    private void InitCrossroad(SingleDecision sd, DecisionCreator dc, DecisionPossibilities p)
    {
      DecisionData d = dc.Data;
      sd.DataContext = p;
      sd.x_crossroad.ItemsSource = dc.Outcomes;
      sd.x_crossroad.DisplayMemberPath = "Name";
      int selected = dc.Outcomes.FindIndex(x => p.Id == x.Id);
      if (selected < 0)
        return;
      sd.x_crossroad.SelectedItem = dc.Outcomes[selected];
    }

    private void DecisionWriteBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      // Buttons for decisions restorations
      DecisionCreator dc = (DataContext as DecisionCreator);
      DecisionData d = dc.Data;
      InitCrossroad(x_first_decision, dc, d.Posibilities[0]);
      for (int i = 1; i < d.Posibilities.Count; i++)
      {
        AddDecision(d.Posibilities[i]);
      }
    }

    private void x_add_decision_Click(object sender, RoutedEventArgs e)
    {
      DecisionData d = (DataContext as DecisionCreator).Data;
      DecisionPossibilities p = new DecisionPossibilities();
      d.Posibilities.Add(p);
      AddDecision(p);
    }

    private void AddDecision(DecisionPossibilities p)
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
