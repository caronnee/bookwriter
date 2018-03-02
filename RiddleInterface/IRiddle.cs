using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiddleInterface
{
    public interface IRiddle
    {
      String Name { get; set; }
      void OnStringInput();
      void OnMouseInput(int x, int y);
      void OnInit();
    }
}
