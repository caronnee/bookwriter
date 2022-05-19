using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace RiddleInterface
{
  public interface ISceneProvider
  {
    List<string> GetSceneNames();
    int GetSceneId(string name);

    void CreateScene(string name);
  }
  public abstract class IRiddleHandler
  {
    //! base folder for this plugin data
    public String BaseFolder { get; set; }

    // name of the content handler
    public abstract String Name { get; }

    public ISceneProvider SceneProvider { get; set; }
    // setting assotiated with the content handler ( rotation etc )
    public abstract Control Settings { get; }

    // showable content
    public abstract Control Viewport { get; }

    // final page for displaying to reader
    public abstract Control DisplayPage { get; }

    // sets the page as not answered yet
    public abstract void ClearAnswer();

    public abstract void Serialize(Serializer.BaseSerializer s);

    // creates write module
    public abstract void Create();
  }
}
