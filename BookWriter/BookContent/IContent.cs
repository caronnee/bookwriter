using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;

namespace MyBook.BookContent
{
  public interface IContent
  {
    Control Show(CacheToControlConverter converter);
    XmlNode ToXmlNode(XmlDocument doc);
  }
}
