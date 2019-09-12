using RiddleInterface;
using System;
using System.Windows.Controls;
using System.Xml;

namespace DecisionMaker
{
  public class DecisionMakerHandler : IRiddleHandler
  {
    public string Name { get; set; }
    public Control Settings { get; set; }
    public Control Viewport { get; set; }

    public DecisionMakerHandler()
    {
      Name = "DecisionMaker";
      Settings = new DecisionSettings();
    }
    public void Create()
    {
      Viewport = new DecisionBox();
    }

    public IContent CreateRiddle()
    {
      ContentDecision s = new ContentDecision();
      DecisionBox b = Viewport as DecisionBox;
      s.Text = b.x_description.Text;
      foreach (DecisionEntry e in b.x_decisions.Children)
      {
        DecisionDescription dd = new DecisionDescription();
        dd.Description = e.x_description.Text;
        dd.Scene = e.x_scene.SelectedIndex.ToString();
        s.Decisions.Add(dd);
      }
      return s;
    }

    public bool CanLoad(XmlNode node)
    {
      if (node.Name != DecisionXmlNames.Name)
        return false;
      return true;
    }

    public IContent Load(XmlNode node)
    {
      ContentDecision p = new ContentDecision();
      if (p.Load(node))
        return p;
      return null;
    }

    public bool ToViewport(IContent content)
    {
      ContentDecision d = content as ContentDecision;
      if (d == null)
        return false;
      DecisionBox box = new DecisionBox();
      foreach (DecisionDescription dec in d.Decisions)
      {
        DecisionEntry e = new DecisionEntry();
        box.x_decisions.Children.Add(e);
        e.x_scene.SelectedIndex = Int32.Parse( dec.Scene);
        e.x_description.Text = dec.Description;
      }
      Viewport = box;
      return true;
    }
  }
}
