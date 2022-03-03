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
    List<int> childrenStack;
    public XmlBookLoad(String name)
    {
      _doc = new XmlDocument();
      _doc.Load(name);
      IsLoading = true;
      _current = _doc.FirstChild;
      childrenStack = new List<int>();
    }
    public override void PopSection()
    {
      _current = _current.ParentNode;
    }

    public override void SerializeString(ref string s)
    {
      s = _current.InnerText;
    }

    public override bool PushSection(string name, int order, string att, ref string attValue)
    {
      if (_current.ChildNodes.Count >= order)
        return false;

      int iter = 0;
      bool found = false;
      for(int i =0; i < _current.ChildNodes.Count; i++)
      {
        if (_current.ChildNodes[i].Name == name)
        {
          if ( iter == order)
          {
            found = true;
            _current = _current.ChildNodes[i];
            break;
          }
          iter++;
        }
      }
      if (!found)
        return false;
      attValue = "";
      foreach(XmlAttribute a in _current.Attributes)
      {
        if (a.Name == att)
        {
          attValue = a.Value;
          break;
        }
      }
      return true;
    }

    public override bool SerializeDouble(string name, ref double value)
    {
      foreach (XmlNode n in _current.ChildNodes)
      {
        if (n.Name == name)
        {
          value = float.Parse(n.Value);
          return true;
        }
      }
      return false;
    }

    public override bool SerializeInt(string name, ref int value)
    {
      foreach (XmlNode n in _current.ChildNodes)
      {
        if (n.Name == name)
        {
          value = Int32.Parse(n.Value);
          return true;
        }
      }
      return false;
    }

    public override bool SerializeString(string name, ref string value)
    {
      foreach (XmlNode n in _current.ChildNodes)
      {
        if (n.Name == name)
        {
          value = n.Value;
          return true;
        }
      }
      return false;
    }
  }
}
