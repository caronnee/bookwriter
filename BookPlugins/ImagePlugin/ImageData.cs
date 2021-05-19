using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagePlugin
{
  [Serializable]
  public class ImageData
  {
    public String ImagePath { get; set; }

    public String Header { get; set; }

    public String Footer { get; set; } 

    // constructor
    public ImageData()
    {
      Footer = "";
      Header = "";
      ImagePath = "";
    }
  }
}
