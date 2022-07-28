using RiddleInterface;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace TextPlugin
{
  public class TextCreator : IRiddleHandler
  {
    private TextWriter _writer;
    private TextReader _reader;
    public override String Name => "Text";

    public override Control Settings => null;
    public override Control Viewport => _writer;
    public override Control DisplayPage => _reader;

    public TextData Data { get; set; }

    public override void ClearAnswer()
    {
      // no answer here, no nned to clear
    }

    public TextCreator()
    {
      Data = new TextData();
    }

    public override void Create()
    {
      _writer = new TextWriter(this);
      CreateReadOnly();
    }

    public void CreateReadOnly()
    {
      _reader = new TextReader();
      _reader.DataContext = Data;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////// Serializer ////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////

    public override void Serialize(Serializer.BaseSerializer s)
    {
      string str = Data.Description;
      s.SerializeString(TextNodeNames.Desc, ref str);
      Data.Description = str;
    }
  }
}
