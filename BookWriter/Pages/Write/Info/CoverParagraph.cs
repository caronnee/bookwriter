using MyBook.BookContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;

namespace MyBook.Pages.Write.Info
{
  public class CoverParagraph : IContent
  {
    public String Filename
    {
      get;
      set;
    }

    public String ImageSource
    {
      get;
      set;
    }

    public Control Show(CacheToControlConverter converter)
    {
      return new Cover();
    }

    public XmlNode ToXmlNode(XmlDocument doc)
    {
      XmlElement node = doc.CreateElement(XmlNodeNames.CoverName);
      return node;
    }
  }
}
