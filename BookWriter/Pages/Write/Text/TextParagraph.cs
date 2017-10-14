using MyBook.BookContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace MyBook.Pages.Write.Text
{
  public class TextParagraph : IContent
  {
    public UIElement Show(CacheToControlConverter converter)
    {
      return converter.Resolve(this);
    }

    public XmlNode ToXmlNode(XmlDocument doc)
    {
      XmlElement node = doc.CreateElement(XmlNodeNames.ParagraphName);
      node.InnerText = Content;
      return node;
    }

    public String Content
    {
      get;
      set;
    }

    public TextParagraph()
    {
      Content = "";
    }

    public void Load(XmlNode paragraph)
    {
      Content = paragraph.InnerText;
      Content = Content.Replace("\r", "");
      Content = Content.Replace("\n", "");
      Content = Content.Replace("\t", "");
    }

  }
}
