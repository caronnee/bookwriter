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
  /// Interaction logic for WritingBox.xaml
  /// </summary>
  public partial class WritingBox : UserControl, IWrite
  {
    public WritingBox()
    {
      InitializeComponent();
    }

    public IContent Create()
    {
      /*if (Text.Length == 0)
        return null;
      TextParagraph par = new TextParagraph();
      par.Content = Text;
      return par;*/
      return null;
    }
  }
}
