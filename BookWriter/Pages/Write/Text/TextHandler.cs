﻿using RiddleInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;

namespace MyBook.Pages.Write.Text
{
  public class TextHandler : IRiddleHandler
  {
    public TextHandler()
    {
      Name = "Text";
      Settings = new TextSettings();
    }
    public string Name { get; set; }
    public UserControl Settings { get; set; }
    public UserControl Viewport { get; set; }

    public void Create()
    {
      Viewport = new WritingBox();
    }

    public IContent CreateRiddle()
    {
      TextParagraph p = new TextParagraph();
      WritingBox box = Viewport as WritingBox;
      p.Content = box.textContent.Text;
      return p;
    }

    public IContent Load(XmlNode node)
    {
      TextParagraph p = new TextParagraph();
      if (p.Load(node))
        return p;
      return null;
    }
  }
}
