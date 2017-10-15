using MyBook.BookContent;
using MyBook.Pages.Write.Imaging;
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

namespace MyBook.Pages.Write.Picture
{
  /// <summary>
  /// Interaction logic for ImageBox.xaml
  /// </summary>
  public partial class ImageBox : Image, IWrite
  {
    public ImageBox()
    {
      InitializeComponent();
      SetImage("pack://application:,,,/Resources/workinprogress.png");
    }

    String SourceName;

    public void SetImage(String name)
    {
      String str;
      if ( name.StartsWith("pack:") )
        str = name;
      else
        str = "file:///" + Settings.TempFolder + name;
      SourceName = name;
      Uri uri = new Uri(str);
      Source = new BitmapImage(uri);
    }
    
    public IContent Create()
    {
      ImageParagraph im = new ImageParagraph();
      im.SourceName = SourceName;
      return im;
    }
  }
}
