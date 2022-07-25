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

namespace TextPlugin
{
  /// <summary>
  /// Interaction logic for TextWriter.xaml
  /// </summary>
  public partial class TextWriter : UserControl
  {
    public TextCreator Owner { get; set; }
    public TextWriter(TextCreator owner)
    {
      Owner = owner;
      InitializeComponent();
    }
  }
}
