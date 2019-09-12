using RiddleInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;

namespace MyBook.Write.Content
{
  class UnrecognizedContent : IContent
  {
    XmlNode _node;

    public bool IsLoaded()
    {
      return false;
    }
    public UserControl ConvertToReadonly()
    {
      throw new NotImplementedException();
    }

    public bool Load(XmlNode node)
    {
      _node = node;
      return true;
    }

    public XmlNode ToXmlNode(XmlDocument doc)
    {
      return _node;
    }
  }
}
