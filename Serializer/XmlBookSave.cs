using System;
using System.Xml;

namespace Serializer
{
  public class XmlBookSave : BaseSerializer
  {
    private XmlDocument _doc;
    String _name;
    XmlNode _current;
    public XmlBookSave(String name)
    {
      _doc = new XmlDocument();
      _name = name;
      IsLoading = false;
    }
    public override void PopSection()
    {
      _current = _current.ParentNode;
    }

    public override int PushSection(string name, ref string att, ref string value)
    {
      XmlElement el = _doc.CreateElement(name);
      int ret = 0;
      if ( _current == null )
      {
        _current = el;
        _doc.AppendChild(el);
        return ret;
      }  
      ret = _current.ChildNodes.Count;
      _current.AppendChild(el);
      _current = el;
      return ret;
    }

    public override void SerializeInt(string name, ref int value)
    {
      XmlElement el = _doc.CreateElement(name);
      el.Value = value.ToString();
      _current.AppendChild(el);
    }

    public override void SerializeDouble(string name, ref double value)
    {
      XmlElement el = _doc.CreateElement(name);
      el.Value = value.ToString();
      _current.AppendChild(el);
    }

    public override void SerializeString(string name, ref string value)
    {
      XmlElement el = _doc.CreateElement(name);
      el.Value = value;
      _current.AppendChild(el);
    }
  }
}
