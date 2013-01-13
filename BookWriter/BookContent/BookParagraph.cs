using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;

namespace MyBook.BookContent
{
  public class BookParagraph : IContent
  {
    public IContent Create()
    {
      return new BookParagraph();
    }
    public Control Show(CacheToControlConverter converter, PositionDesc desc)
    {
      return converter.Resolve(this, desc);
    }

    public StringBuilder Content
    {
      get;
      set;
    }

    public BookParagraph()
    {
      Content = new StringBuilder();
    }

    public void Load(XmlNode paragraph)
    {
      Content = new StringBuilder(paragraph.InnerText);
      Content = Content.Replace("\r", "");
      Content = Content.Replace("\n", "");
      Content = Content.Replace("\t", "");
    }

  }
}
