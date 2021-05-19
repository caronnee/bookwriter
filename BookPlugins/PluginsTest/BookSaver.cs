using RiddleInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PluginsTest
{
  public class BookSaver : IRiddleSerializer
  {
    Stack<XmlElement> m_sections;
    XmlDocument m_doc;
    String m_name;

    public BookSaver(string name)
    {
      m_name = name;
      m_doc = new XmlDocument();
      m_sections = new Stack<XmlElement>();
      StartSection(AppNodeNames.BookString);
    }

    public void SaveValue(string name, string value)
    {
      StartSection(name);
      m_sections.Peek().InnerText = value;
      EndSection();
    }

    public void SaveParameter(string name, string value)
    {
      XmlAttribute att = m_doc.CreateAttribute(name);
      att.Value = value;
      m_sections.Peek().Attributes.Append(att);
    }

    public void StartSection(string name)
    {
      XmlElement el = m_doc.CreateElement(name);
      m_sections.Push(el);
    }

    public void EndSection()
    {
      XmlElement el = m_sections.Pop();
      m_sections.Peek().AppendChild(el);
    }

    public void Close()
    {
      m_doc.AppendChild(m_sections.Pop());
      m_doc.Save(m_name);
    }

    public int Children()
    {
      throw new NotImplementedException();
    }

    public string LoadSection(int i)
    {
      throw new NotImplementedException();
    }

    public string LoadValue()
    {
      throw new NotImplementedException();
    }

    public string LoadParameter(string name)
    {
      throw new NotImplementedException();
    }

    public string LoadSection(string name)
    {
      throw new NotImplementedException();
    }
  }

}
