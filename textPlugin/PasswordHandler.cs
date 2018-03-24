using RiddleInterface;
using System.Windows.Controls;

namespace PasswordPlugin
{
  public class PasswordHandler : IRiddleHandler
  {
    public PasswordHandler()
    {
      Name = "PasswordPlugin";

      //TODO set dependencies
    }

    public string Name { get; set; }
    public UserControl Settings { get; set; }
    public UserControl Viewport { get; set; }

    public void Create()
    {
      Settings = new PasswordSetting();
      Viewport = new PasswordBox();
    }
  }
}
