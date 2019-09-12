using RiddleInterface;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Xml;

namespace DecisionMaker
{
  static class DecisionXmlNames
  {
    static public String Name = "Decision";
    static public String Option = "Option";
    static public String Decision = "Decision";
    static public String SkipTo = "SkipTo";
    static public String Desc = "Description";
  }

  public struct DecisionDescription
  {
    public String Scene;
    public String Description;
  }

  class ContentDecision : IContent
  {
    public String Text;

    public List<DecisionDescription> Decisions;

    public ContentDecision()
    {
      Decisions = new List<DecisionDescription>();
    }

    public UserControl ConvertToReadonly()
    {
      throw new NotImplementedException();
    }

    public bool IsLoaded()
    {
      return true;
    }

    public bool Load(XmlNode node)
    {
      if (node.Name!= DecisionXmlNames.Name)
        return false;
      foreach (XmlElement el in node.ChildNodes)
      {
        if (el.Name == DecisionXmlNames.Desc)
          Text = el.InnerText;
        else if (el.Name == DecisionXmlNames.Option)
        {
          DecisionDescription d = new DecisionDescription();
          foreach (XmlElement el2 in el.ChildNodes)
          {
            if (el2.Name == DecisionXmlNames.Decision)
              d.Description = el2.InnerText;
            if (el2.Name == DecisionXmlNames.SkipTo)
              d.Scene = el2.InnerText;
          }
          this.Decisions.Add(d);
        }
      }
      return true;
    }

    public XmlNode ToXmlNode(XmlDocument doc)
    {
      XmlNode node = doc.CreateElement(DecisionXmlNames.Name);
      XmlElement el = doc.CreateElement(DecisionXmlNames.Desc);
      el.InnerText = Text;
      node.AppendChild(el);
      foreach (DecisionDescription en in Decisions)
      { 
        // for each decision
        XmlElement option = doc.CreateElement(DecisionXmlNames.Option);
        XmlElement n = doc.CreateElement(DecisionXmlNames.Decision);
        n.InnerText = en.Description;
        option.AppendChild(n);
        n = doc.CreateElement(DecisionXmlNames.SkipTo);
        n.InnerText = en.Scene;
        option.AppendChild(n);
        node.AppendChild(option);
      }
      return node;
    }
  }
}
