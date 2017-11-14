using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBook.Pages.Write
{
  [Export]
  interface IRiddle
  {
    bool OnTextInput(String str);
    // position of the mouse in the active area
    bool OnMouseInput(int x, int y);
    int OnCorrect();
    //
    Control EditControls();
  }
}
