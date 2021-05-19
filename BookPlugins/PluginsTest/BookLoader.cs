using RiddleInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PluginsTest
{
  public class BookLoader : IRiddleSerializer
  {
    XmlDocument m_doc;

    public BookLoader(String name)
    {
      m_doc = new XmlDocument();
      m_doc.Load(name);
      m_actualNode = m_doc.FirstChild;
      // start section "book"
    }

    XmlNode m_actualNode;

    public int Children()
    {
      return m_actualNode.ChildNodes.Count;
    }

    public string LoadSection(int i)
    {
      m_actualNode = m_actualNode.ChildNodes[i];
      return m_actualNode.Name;
    }

    public void EndSection()
    {
      m_actualNode = m_actualNode.ParentNode;
    }

    public string LoadValue()
    {
      return m_actualNode.InnerText;
    }

    public string LoadParameter(string name)
    {
      for (int i = 0; i < m_actualNode.Attributes.Count; i++)
      {
        XmlAttribute att = m_actualNode.Attributes[i];
        if (att.Name == name)
          return att.Value;
      }
      return "";
    }

    public void Close()
    {

    }

    public void SaveParameter(string name, string value)
    {
      throw new NotImplementedException();
    }

    public void StartSection(string name)
    {
      foreach (XmlElement el in m_actualNode.ChildNodes)
      {
        if (el.Name == name)
        {
          m_actualNode = el;
          return;
        }
      }
    }

    public void SaveValue(string name, string value)
    {
      throw new NotImplementedException();
    }

    public string LoadSection(string name)
    {
      foreach (XmlElement el in m_actualNode.ChildNodes)
      {
        if (el.Name == name)
        {
          return el.InnerText;
        }
      }
      return "";
    }
  }

}
