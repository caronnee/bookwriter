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
      SetImage("workinprogress.png",false);
    }

    String SourceName;

    public void SetImage(String name, bool fromFile)
    {
      String str;
      if (name.StartsWith("file:///") || name.StartsWith("pack:"))
      {
        str = name;
      }
      {
        if (fromFile)
          str = "file:///" + name;
        else
          str = "pack://application:,,,/Resources/" + name;
      }
      SourceName = str;
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
