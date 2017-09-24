using MyBook.BookContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MyBook.Pages.Write.Entity
{
  public class ImageParagraph : Image, IContent
  {
    public Control Show(CacheToControlConverter converter)
    {
      return converter.Resolve(this);
    }

    public String ImageToLoad
    {
      get;
      set;
    }

    public ImageParagraph()
    {
      Uri uri = new Uri("pack://application:,,,/Resources/workinprogress.png");
      Source = new BitmapImage(uri);
    }

    public ImageParagraph(String imageToLoad)
    {
      ImageToLoad = imageToLoad;
    }
  }
}

