using RiddleInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;

namespace PasswordPlugin
{
  static class PasswordXmlNames
  {
    static public String Name = "PasswordParagraph";
    static public String Description = "Description";
    static public String Question = "Question";
    static public String Option = "Option";
    static public String Hint = "Hints";
    static public String RegExp = "Desc";
    static public String Correct = "Correct";
    static public String GoTo = "Goto";
  }

  class PasswordParagraph : IContent
  {
    public struct PassItem
    {
      public String regexp;
      public String pagedesc;
      public String hint;
    }
    public String description;
    public String questionText;
    public List<PassItem> items;

    public UserControl ConvertToReadonly()
    {
      PasswordBox box = new PasswordBox();
      box.question.Text = questionText;
      // TODO switch text according to the page size
      box.description.Text = description;
      return box;
    }

    public bool Load(XmlNode paragraph)
    {
      if ( paragraph.Name != PasswordXmlNames.Name)
        return false;
      items = new List<PassItem>();
      XmlNodeList nodes = paragraph.ChildNodes;
      foreach (XmlNode node in nodes)
      {
        if (node.Name == PasswordXmlNames.Question)
          questionText = node.InnerText;
        else if (node.Name == PasswordXmlNames.Description)
          description = node.InnerText;
        else if (node.Name == PasswordXmlNames.Hint)
        {
          XmlNodeList list = node.ChildNodes;
          foreach ( XmlNode t in list )
          {
            PassItem item = new PassItem();
            item.hint = t.Attributes[PasswordXmlNames.Hint].InnerText;
            item.pagedesc = t.Attributes[PasswordXmlNames.GoTo].InnerText;
            item.regexp = t.Attributes[PasswordXmlNames.RegExp].InnerText;
            items.Add(item);
          }
        }
      }
      return true;
    }

    public XmlNode ToXmlNode(XmlDocument doc)
    {
      XmlNode node = doc.CreateElement(PasswordXmlNames.Name);
      XmlNode contentNode = doc.CreateElement(PasswordXmlNames.Question);
      contentNode.InnerText = questionText;
      node.AppendChild(contentNode);
      contentNode = doc.CreateElement(PasswordXmlNames.Description);
      contentNode.InnerText = description;
      node.AppendChild(contentNode);

      // save all hints
      foreach (var a in items)
      {
        XmlNode tempNode = doc.CreateElement(PasswordXmlNames.Option);
        XmlAttribute att = doc.CreateAttribute(PasswordXmlNames.Hint);
        att.Value = a.hint;
        tempNode.Attributes.Append(att);
        att = doc.CreateAttribute(PasswordXmlNames.GoTo);
        att.Value = a.pagedesc;
        tempNode.Attributes.Append(att);
        att = doc.CreateAttribute(PasswordXmlNames.RegExp);
        att.Value = a.regexp;
        tempNode.Attributes.Append(att);
        node.AppendChild(tempNode);
      }
      return node;
    }
    
  }
}
