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

namespace MyBook.Pages.Write.Riddle
{
  /// <summary>
  /// Interaction logic for RiddleSettings.xaml
  /// </summary>
  public partial class RiddleSettings : UserControl
  {
    public RiddleSettings()
    {
      InitializeComponent();
      DataContext = new RiddleParagraph();
    }
  }
}
