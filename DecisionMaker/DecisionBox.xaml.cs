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

namespace DecisionMaker
{
  /// <summary>
  /// Interaction logic for DecisionBox.xaml
  /// </summary>
  public partial class DecisionBox : UserControl
  {
    public DecisionBox()
    {
      InitializeComponent();
    }

    private void removeItem(object sender, RoutedEventArgs w)
    {
      Button el = sender as Button;
      DecisionEntry b = el.DataContext as DecisionEntry;
      x_decisions.Children.Remove(b);
    }

    private void x_add_Click(object sender, RoutedEventArgs e)
    {
      DecisionEntry d = new DecisionEntry();
      d.x_remove.Click += new RoutedEventHandler(removeItem);
      d.x_remove.DataContext = d;
      x_decisions.Children.Add(d);
    }
  }
}
