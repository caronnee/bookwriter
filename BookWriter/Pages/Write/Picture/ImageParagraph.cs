using MyBook.BookContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml;

namespace MyBook.Pages.Write.Imaging
{ 
  public class ImageParagraph : Image, IContent
  {
    public Control Show(CacheToControlConverter converter)
    {
      return converter.Resolve(this);
    }

    public XmlNode ToXmlNode(XmlDocument doc)
    {
      XmlElement node = doc.CreateElement(XmlNodeNames.ImageName);
      return node;
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

