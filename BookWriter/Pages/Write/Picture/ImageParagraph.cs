using MyBook.BookContent;
using RiddleInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml;

namespace MyBook.Pages.Write.Imaging
{ 
  public class ImageParagraph : IContent
  {
    public bool Load(XmlNode node)
    {
      if (node.Name != XmlNodeNames.ImageName)
        return false;
      if ( node.InnerText.Length > 0)
        SourceName = node.InnerText;
      return true;
    }

    public String SourceName
    {
      get;
      set;
    }
    public String ImageHeader
    {
      get;
      set;
    }
    public String ImageFooter
    {
      get;
      set;
    }

    public XmlNode ToXmlNode(XmlDocument doc)
    {
      XmlElement node = doc.CreateElement(XmlNodeNames.ImageName);
      node.InnerText = SourceName;
      return node;
    }

    public UserControl ConvertToReadonly()
    {
      throw new NotImplementedException();
    }

    public ImageParagraph()
    {
      SourceName = "";
    }

    public ImageParagraph(String imageToLoad)
    {
      SourceName = imageToLoad;
    }
  }
}

