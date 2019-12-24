using MyBook.Write.WriteContent.Text;
using RiddleInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;

namespace MyBook.Write.Text
{
  public class TextHandler : IRiddleHandler
  {
    public TextHandler()
    {
      Name = "Text";
      Settings = new TextSettings();
    }
    public string Name { get; set; }
    public Control Settings { get; set; }
    public Control Viewport { get; set; }

    public void Create()
    {
      Viewport = new WritingBox();
    }

    public IContent CreateRiddle()
    {
      TextParagraph p = new TextParagraph();
      WritingBox box = Viewport as WritingBox;
      p.Content = box.Text;
      return p;
    }

    public bool CanLoad(XmlNode node)
    {
      if (node.Name != BookContent.XmlNodeNames.ParagraphName)
        return false;
      return true;
    }

    public IContent Load(XmlNode node)
    {
      TextParagraph p = new TextParagraph();
      if (p.Load(node))
        return p;
      return null;
    }

    public bool ToViewport(IContent content)
    {
      TextParagraph p = content as TextParagraph;
      if (p == null)
        return false;
      WritingBox b = new WritingBox();
      b.Text = p.Content;
      Viewport = b;
      return true;
    }
  }
}
