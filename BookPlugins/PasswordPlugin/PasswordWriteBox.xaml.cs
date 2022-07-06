using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PasswordPlugin
{
  /// <summary>
  /// Interaction logic for PasswordWriteBox.xaml
  /// </summary>
  public partial class PasswordWriteBox : UserControl
  {
    // Constructor
    public PasswordWriteBox()
    {
      InitializeComponent();
    }  
    public PasswordMaker Owner { get; set; }
    public void x_countdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      //int index = x_countdown.SelectedIndex;
      //if (index == x_countdown.Items.Count - 1)
      //  index = -1;
      //PasswordMaker m = (DataContext as PasswordMaker);
      //m.Data.Definition.NAllowedFailures = index;
      //if (index <= 0 )
      //{
      //  x_hints_holder.Visibility = Visibility.Collapsed;
      //}
      //else
      //{
      //  x_hints_holder.Visibility = Visibility.Visible;
      //  RefillHints();
      //}
    }

    public void RefillHints()
    {
      x_hints.Children.Clear();
      PasswordData d = (DataContext as PasswordMaker).Data;
      for ( int i =0; i < d.Definition.NAllowedFailures; i++)
      {
        TextBox b = new TextBox();
        x_hints.Children.Add(b);
        HintItem it = d.Definition.Hints[i];
        b.DataContext = it;
        b.SetBinding(TextBox.TextProperty, "Hint");
      }
    }
  }
}
