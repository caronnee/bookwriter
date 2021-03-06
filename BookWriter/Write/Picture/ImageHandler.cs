﻿using MyBook.Write.Imaging;
using RiddleInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;

namespace MyBook.Write.Picture
{
  public class ImageHandler : IRiddleHandler
  {
    public string Name { get; set; }
    public Control Settings { get; set; }
    public Control Viewport { get; set; }
    public ImageHandler()
    {
      Name = "Image";
      Settings = new ImageSettings();
    }
    
    public void Create()
    {
      Viewport = new ImageBox();
    }
    private void LoadContent(ImageParagraph p)
    {
      ImageBox box = new ImageBox();
      box.x_footer.Text = p.ImageFooter;
      box.x_header.Text = p.ImageHeader;
      box.x_picture.Source = null;// TODO
      Viewport = box;
    }
    public IContent CreateRiddle()
    {
      ImageParagraph p = new ImageParagraph();
      ImageBox box = Viewport as ImageBox;
      p.SourceName = box.x_picture.Source.ToString();
      p.ImageHeader = box.x_header.Text;
      p.ImageFooter = box.x_footer.Text;
      return p;
    }

    public bool CanLoad(XmlNode node)
    {
      if (node.Name != BookContent.XmlNodeNames.ImageName)
        return false;
      return true;
    }

    public IContent Load(XmlNode node)
    {
      ImageParagraph p = new ImageParagraph();
      if (p.Load(node))
        return p;
      return null;
    }

    public bool ToViewport(IContent content)
    {
      ImageParagraph p = content as ImageParagraph;
      if (p == null)
        return false;

      return true;
    }
  }
}
