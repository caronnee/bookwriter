using MyBook.Pages.Write.Imaging;
using RiddleInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyBook.Pages.Write.Picture
{
  public class ImageHandler : IRiddleHandler
  {
    public string Name { get; set; }
    public UserControl Settings { get; set; }
    public UserControl Viewport { get; set; }
    public ImageHandler()
    { }

    public event OnSuccessAction OnSuccess;

    public void Create()
    {
      Settings = new ImageSettings();
      Viewport = new ImageBox();
    }
  }
}
