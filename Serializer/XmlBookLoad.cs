using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Serializer
{
  public class XmlBookLoad : BaseSerializer
  {
    XmlDocument _doc;
    XmlNode _current;
    public XmlBookLoad(String name)
    {
      _doc = new XmlDocument();
      _doc.LoadXml(name);
    }
    public override void PopSection()
    {
      _current = _current.ParentNode;
    }

    public override int PushSection(string name, ref string att, ref string value)
    {
      //_current.
      return 0;
    }

    public override void SerializeDouble(string name, ref double value)
    {
      throw new NotImplementedException();
    }

    public override void SerializeInt(string name, ref int value)
    {
      throw new NotImplementedException();
    }

    public override void SerializeString(string name, ref string value)
    {
      throw new NotImplementedException();
    }
  }
}
