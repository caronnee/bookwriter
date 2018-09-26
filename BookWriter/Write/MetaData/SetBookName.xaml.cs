using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace  MyBook.Meta
{
  /// <summary>
  /// Interaction logic for SetBookName.xaml
  /// </summary>
  public partial class SetBookName : Window
  {
    public SetBookName()
    {
      InitializeComponent();
    }
    public void CancelClick(object sender, RoutedEventArgs e)
    {
      x_bookName.Text = "";
      Close();
    }
    public void DoneClick(object sender, RoutedEventArgs e)
    {
      if (x_bookName.Text.Length == 0)
      {
        //red
        return;
      }
      Close();
    }
  }
}
