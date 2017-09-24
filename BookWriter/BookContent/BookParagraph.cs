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

    public String Content
    {
      get;
      set;
    }

    public BookParagraph()
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
