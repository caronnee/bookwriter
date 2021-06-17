using RiddleInterface;
using System.Collections.Generic;
using System.Windows.Controls;

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

    public bool Load(IRiddleSerializer r)
    {
      string ret = r.LoadParameter(TextNodeNames.Id);
      if (ret != Name)
        return false;
      Data.Description = r.LoadSection(TextNodeNames.Desc);
      Create();
      return true;
    }

    public void Save(IRiddleSerializer r)
    {
      r.SaveParameter(TextNodeNames.Id, Name);
      r.SaveValue(TextNodeNames.Desc, Data.Description);
    }
  }
}
