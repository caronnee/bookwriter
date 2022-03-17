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
    private void CheckFolder()
    {
      int index = _name.LastIndexOf('\\');
      string str = _name.Substring(0, index);
      System.IO.Directory.CreateDirectory(str);
    }
    public void Finish()
    {
      CheckFolder();
      _doc.Save(_name);
    }
    public override void PopSection()
    {
      _current = _current.ParentNode;
    }

    public override bool SerializeAttribute(string name, ref string val)
    {
      XmlAttribute att = _doc.CreateAttribute(name);
      att.InnerText = val;
      _current.Attributes.Append(att);
      return true;
    }
    public override bool PushSection(string name, int order)
    {
      XmlElement el = _doc.CreateElement(name);
      if ( _current == null )
      {
        _current = el;
        _doc.AppendChild(el);
        return true;
      }  
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
