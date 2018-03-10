using RiddleInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextPlugin
{
    public class TextPlugin : IRiddleHandler
    {
        public TextPlugin()
        {
            Name = "TextPlugin";
        }

        public string Name { get; set; }

        public Control GetManipulationControl()
        {
            return new TextSettings();
        }
    }
}
