using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RiddleInterface
{
    public interface IRiddle
    {
        // final string handlin g or so will be put here
        void OnInit(Control drawOn);
    }
    public interface IRiddleHandler
    {
      String Name { get; private set; }
      Control GetManipulationControl();
    }
}
