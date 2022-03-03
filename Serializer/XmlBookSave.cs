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
    public void Finish()
    {
      _doc.Save(_name);
    }
    public override void PopSection()
    {
      _current = _current.ParentNode;
    }

    public override bool PushSection(string name, int order, string attName, ref string value)
    {
      XmlElement el = _doc.CreateElement(name);
      if (attName.Length!=0)
      {
        XmlAttribute att = _doc.CreateAttribute(attName);
        att.InnerText = value;
        el.Attributes.Append(att);
      }
      int ret = 0;
      if ( _current == null )
      {
        _current = el;
        _doc.AppendChild(el);
        return true;
      }  
      ret = _current.ChildNodes.Count;
      _current.AppendChild(el);
      _current = el;
      return true;
    }

    public override bool SerializeInt(string name, ref int value)
    {
      string val = value.ToString();
      return SerializeString(name, ref val);
    }

    public override bool SerializeDouble(string name, ref double value)
    {
      string val = value.ToString();
      return SerializeString(name, ref val);
    }
    public override void SerializeString(ref string s)
    {
      _current.InnerText = s;
    }

    public override bool SerializeString(string name, ref string value)
    {
      XmlElement el = _doc.CreateElement(name);
      el.InnerText = value;
      _current.AppendChild(el);
      return true;
    }
  }
}
