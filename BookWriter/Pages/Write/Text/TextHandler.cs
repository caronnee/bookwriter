using RiddleInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyBook.Pages.Write.Text
{
  public class TextHandler : IRiddleHandler
  {
    public TextHandler()
    {
      Name = "Text";
    }
    public string Name { get; set; }
    public UserControl Settings { get; set; }
    public UserControl Viewport { get; set; }

    public event OnSuccessAction OnSuccess;

    public void Create()
    {
      Settings = new TextSettings();
      Viewport = new WritingBox();
    }
  }
}
