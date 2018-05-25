using System;
using System.Windows.Controls;

namespace RiddleInterface
{
  public interface IRiddle
  {
    // final string handling or so will be put here
    void OnInit(UserControl drawOn);
  }

  public delegate void OnSuccessAction();

  public interface IRiddleHandler
  {
    String Name { get; set; }
    UserControl Settings { get; set; }
    UserControl Viewport { get; set; }
    void Create();
    event OnSuccessAction OnSuccess;
  }
}
