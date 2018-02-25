using RiddleInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stringPlugin
{
  public class TextPlugin : IRiddle
  {
    public String Name()
    {
      return "String password";
    }
    public void OnStringInput()
    {
      throw new NotImplementedException();
    }

    public void OnMouseInput(int x, int y)
    {
      throw new NotImplementedException();
    }

    public void OnInit()
    {
      throw new NotImplementedException();
    }
  }
}
