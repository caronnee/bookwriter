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

    public void x_countdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      int index = (DataContext as PasswordMaker).Data.Definition.NAllowedFailures;
      if (index <= 0 )
      {
        x_hints_holder.Visibility = Visibility.Collapsed;
      }
      else
      {
        x_hints_holder.Visibility = Visibility.Visible;
        int oSize = x_hints.Children.Count;
        if ( oSize > index )
        {
          // remove overflow textboxes
          int toChange = oSize - index;
          x_hints.Children.RemoveRange(index, toChange);
          PasswordData d = (DataContext as PasswordMaker).Data;
          d.Definition.Hints.RemoveRange(index, toChange);
        }
        else
        {
          int toChange = index - oSize;
          for (int i = 0; i < toChange; i++)
          {
            PasswordData d = (DataContext as PasswordMaker).Data;
            HintItem it = new HintItem();
            d.Definition.Hints.Add(it);            
          }
        }
        RefillHints();
      }
    }

    public void RefillHints()
    {
      x_hints.Children.Clear();
      PasswordData d = (DataContext as PasswordMaker).Data;
      for ( int i =0; i < d.Definition.Hints.Count; i++)
      {
        TextBox b = new TextBox();
        x_hints.Children.Add(b);
        HintItem it = d.Definition.Hints[i];
        b.DataContext = it;
        b.SetBinding(TextBox.TextProperty, "Hint");
      }
    }

    private void pwd_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      // Buttons for decisions restorations
      PasswordMaker dc = (DataContext as PasswordMaker);
      x_fail.ItemsSource = dc.Outcomes;
      x_fail.DisplayMemberPath = "Name";
      x_fail.SelectedIndex = 0;
      x_success.ItemsSource = dc.Outcomes;
      x_success.DisplayMemberPath = "Name";
      x_success.SelectedIndex = 0;
    }
  }
}
