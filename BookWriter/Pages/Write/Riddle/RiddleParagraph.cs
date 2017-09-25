using MyBook.BookContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;

namespace MyBook.Pages.Write.Riddle
{
  class RiddleParagraph : IContent
  {
    public Control Show(CacheToControlConverter converter)
    {
      return null;
    }

    public XmlNode ToXmlNode(XmlDocument doc)
    {
      XmlElement node = doc.CreateElement(XmlNodeNames.RiddleName);
      return node;
    }

    public IContent Create()
    {
      return null;
    }
  }
}
