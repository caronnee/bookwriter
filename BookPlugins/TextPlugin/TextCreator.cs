using RiddleInterface;
using System.Collections.Generic;
using System.Windows.Controls;

namespace TextPlugin
{
  public class TextCreator : IRiddleHandler
  {
    public string Name => "Text";

    //public List<Outcome> Outcomes { get; set; }
    public Control Settings { get; set; }
    public Control Viewport { get; set; }
    public Control DisplayPage { get; set; }

    public TextData Data { get; set; }
    public string BaseFolder { get; set; }
    public int Order { get; set; }

    public void ClearAnswer()
    {
      // no answer here
    }

    public TextCreator()
    {
      Data = new TextData();
      Settings = null;
    }

    public void Create()
    {
      Viewport = new TextWriter();
      Viewport.DataContext = Data;
      CreateReadOnly();
    }

    public void CreateReadOnly()
    {
      DisplayPage = new TextReader();
      DisplayPage.DataContext = Data;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////// Serializer ////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public void Serialize(Serializer.BaseSerializer s)
    {
      string str = Data.Description;
      s.SerializeString(TextNodeNames.Desc, ref str);
      Data.Description = str;
    }
  }
}
