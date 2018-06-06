using MyBook.Pages.Write.Imaging;
using RiddleInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;

namespace MyBook.Pages.Write.Picture
{
  public class ImageHandler : IRiddleHandler
  {
    public string Name { get; set; }
    public UserControl Settings { get; set; }
    public UserControl Viewport { get; set; }
    public ImageHandler()
    {
      Name = "Image";
      Settings = new ImageSettings();
    }
    
    public void Create()
    {
      Viewport = new ImageBox();
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

    public IContent Load(XmlNode node)
    {
      ImageParagraph p = new ImageParagraph();
      if (p.Load(node))
        return p;
      return null;
    }
  }
}
