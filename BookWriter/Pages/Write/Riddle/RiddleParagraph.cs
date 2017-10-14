using MyBook.BookContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace MyBook.Pages.Write.Riddle
{
  class RiddleParagraph : IContent
  {
    public UIElement Show(CacheToControlConverter converter)
    {
      return null;
    }

    public void Load(XmlNode node)
    {
      // nothing so far
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
