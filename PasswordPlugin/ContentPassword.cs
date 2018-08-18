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
    static public String Name = "Password";
    static public String Description = "Description";
    static public String Question = "Question";
    static public String Help = "Help";
    static public String Hints = "Hints";
    static public String Hint = "Hint";
    static public String RegExp = "Desc";
    static public String Correct = "Correct";
    static public String GoTo = "Goto";
    static public String SceneName = "Scene";
  }

  class ContentPassword : IContent
  {
    public struct PassItem
    {
      public String regexp;
      public String sceneId;
      public String hint;
    }
    public String description;
    public String questionText;
    public List<PassItem> items;

    public UserControl ConvertToReadonly()
    {
      PasswordBox box = new PasswordBox();
      box.x_question.Text = questionText;
      // TODO switch text according to the page size
      box.x_description.Text = description;
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
        else if (node.Name == PasswordXmlNames.Hints)
        {
          XmlNodeList list = node.ChildNodes;
          foreach ( XmlNode t in list )
          {
            PassItem item = new PassItem();
            item.hint = t.Attributes[PasswordXmlNames.Help].InnerText;
            item.regexp = t.Attributes[PasswordXmlNames.RegExp].InnerText;
            try
            {
              item.sceneId = t.Attributes[PasswordXmlNames.GoTo].InnerText;
            }
            catch (Exception)
            {
              item.sceneId = null;
            }
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
      XmlNode hintsNode = doc.CreateElement(PasswordXmlNames.Hints);

      // save all hints
      foreach (var a in items)
      {
        XmlNode tempNode = doc.CreateElement(PasswordXmlNames.Hint);
        XmlAttribute att = doc.CreateAttribute(PasswordXmlNames.Help);
        att.Value = a.hint;
        tempNode.Attributes.Append(att);
        att = doc.CreateAttribute(PasswordXmlNames.GoTo);
        att.Value = a.sceneId;
        tempNode.Attributes.Append(att);
        att = doc.CreateAttribute(PasswordXmlNames.RegExp);
        att.Value = a.regexp;
        tempNode.Attributes.Append(att);
        hintsNode.AppendChild(tempNode);
      }
      node.AppendChild(hintsNode);
      return node;
    }
    
  }
}
