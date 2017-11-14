using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginInterface
{
  public interface IRiddleAction
  {
    // name of the plugin
    string Name { get; }

    // returns true if the was success
    bool PerformActionMouse( int x, int y);

    // 
    bool PerformActionMouse(String positionInActiveWindow);

    // how many pages to skip
    int OnAnswer(System.Net.Mime.MediaTypeNames.Image image);
  }
}
