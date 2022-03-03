using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace RiddleInterface
{
  public interface IRiddleHandler
  {
    // base folder for this plugin data
    String BaseFolder { get; set; }

    // name of the content handler
    String Name { get; }

    // setting assotiated with the content handler ( rotation etc )
    Control Settings { get; set; }

    // showable content
    Control Viewport { get; set; }

    // final page for displaying to reader
    Control DisplayPage { get; set; }

    // sets the page as not answered yet
    void ClearAnswer();
    
    void Serialize(Serializer.BaseSerializer s);

    // creates write module
    void Create();
  }
}
