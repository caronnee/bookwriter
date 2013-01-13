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
    public IContent Create()
    {
      return new ImageParagraph();
    }

    public Control Show(CacheToControlConverter converter, PositionDesc desc)
    {
      return converter.Resolve(this, desc);
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

