using MyBook.BookContent;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyBook.Pages.Write.Text
{
  /// <summary>
  /// Interaction logic for TextSettings.xaml
  /// </summary>
  public partial class TextSettings : UserControl, ISettings
  {
    public TextSettings()
    {
      InitializeComponent();
      Reset();
    }

    public void Reset()
    {
      DataContext = new WritingBox();
    }
  }
}
