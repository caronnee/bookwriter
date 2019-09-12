using MyBook.BookContent;
using RiddleInterface;
using System;
using System.Windows.Controls;
using System.Xml;

namespace MyBook.Write.Text
{
  public class TextParagraph : IContent
  {
    public bool IsLoaded()
    {
      return true;
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

    public bool Load(XmlNode paragraph)
    {
      if (paragraph.Name != XmlNodeNames.ParagraphName)
        return false;
      Content = paragraph.InnerText;
      Content = Content.Replace("\r", "");
      Content = Content.Replace("\n", "");
      Content = Content.Replace("\t", "");
      return true;
    }

    public UserControl ConvertToReadonly()
    {
      throw new NotImplementedException();
    }
  }
}
