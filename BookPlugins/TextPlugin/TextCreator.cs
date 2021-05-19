using RiddleInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace TextPlugin
{
  public class TextCreator : IRiddleHandler
  {
    public OnSuccessAction onAnswer { get; set; }

    public string Name => "Text";

    public List<Outcome> Outcomes { get; set; }
    public Control Settings { get; set; }
    public Control Viewport { get; set; }
    public Control DisplayPage { get; set; }

    public TextData Data { get; set; }
    public string BaseFolder { get; set; }

    public void ClearAnswer()
    {
      // no answer here
    }

    public void Create()
    {
      Data = new TextData();
      Settings = null;
      Viewport = new TextWriter();
      Viewport.DataContext = Data;
      CreateReadOnly();
    }

    public void CreateReadOnly()
    {
      DisplayPage = new TextReader();
      DisplayPage.DataContext = Data;
    }

    public bool Load(IRiddleSerializer r)
    {
      string ret = r.LoadParameter(TextNodeNames.Id);
      if (ret != Name)
        return false;
      r.LoadSection(TextNodeNames.Desc);
      Data.Description = r.LoadValue();
      return true;
    }

    public void Save(IRiddleSerializer r)
    {
      r.SaveParameter(TextNodeNames.Id, Name);
      r.SaveValue(TextNodeNames.Desc, Data.Description);
    }
  }
}
