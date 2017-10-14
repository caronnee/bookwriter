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
  public class ImageParagraph : IContent
  {
    public String SourceName
    {
      get;
      set;
    }

    public Control Show(CacheToControlConverter converter)
    {
      return converter.Resolve(this);
    }

    public XmlNode ToXmlNode(XmlDocument doc)
    {
      XmlElement node = doc.CreateElement(XmlNodeNames.ImageName);
      node.InnerText = SourceName;
      return node;
    }

    public ImageParagraph()
    {
      SourceName = "pack://application:,,,/Resources/workinprogress.png";
    }

    public ImageParagraph(String imageToLoad)
    {
      SourceName = imageToLoad;
    }
  }
}

