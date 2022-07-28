using RiddleInterface;
using Serializer;
using System.Collections.Generic;
using System.Windows.Controls;

namespace DecisionPlugin
{
  public class DecisionCreator : IRiddleHandler
  {
    private DecisionWriteBox _writer;
    private DecisionBox _reader;
    public override string Name => "Decision";

    public override Control Settings => null;

    public override Control Viewport => _writer;

    public override Control DisplayPage => _reader;

    public DecisionData Data { get; set; }

    // detail about user input
    public PostAnswerData Answer { get; set; }

    public DecisionCreator()
    {
      Data = new DecisionData();
      ClearAnswer();
    }
    public override void Create()
    {
      _writer = new DecisionWriteBox(this);
      for (int i = 0; i < Data.Posibilities.Count; i++)
      {
        _writer.AddDecision(Data.Posibilities[i]);
      }
    }

    public void Answered()
    {
      _writer.x_decisions.Visibility = System.Windows.Visibility.Collapsed;
      _writer.x_description.Text += "\n" + Answer.Reaction;
    }

    public override void ClearAnswer()
    {
      Answer = null;
    }

    public void CreateReadOnly()
    {
      _reader = new DecisionBox();
      _reader.DataContext = Data;
      if (Answer != null)
      {
        Answered();
      }
      else
      {
        for (int i = 0; i < Data.Posibilities.Count; i++)
        {
          DecisionPossibilities s = Data.Posibilities[i];
          Button bt = new Button();
          bt.Content = s.Action;
          bt.DataContext = s;
          _writer.x_decisions.Children.Add(bt);
        }
      }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////// Serialization //////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////

    struct PossibilitySerialize
    {
      public int id;
      public string action;
      public string reaction;
      public string item;
    }
    struct DecisionSerialize
    {
      public string description;
      public PossibilitySerialize[] possibilities;
    }

    public void Save(Serializer.BaseSerializer r)
    {
      hasNextDecision = NextDecisionSave;
      DecisionSerialize ds = new DecisionSerialize();
      ds.description = Data.Description;
      ds.possibilities = new PossibilitySerialize[Data.Posibilities.Count];
      for (int i = 0; i < Data.Posibilities.Count; i++)
      {
        DecisionPossibilities dp = Data.Posibilities[i];
        PossibilitySerialize ps = new PossibilitySerialize();
        ps.id = r.toId(dp.Id);
        ps.action = dp.Action;
        ps.reaction = dp.Reaction;
        ps.item = dp.Item;
        ds.possibilities[i] = ps;
      }
      Serialize(r, ref ds);
    }

    delegate bool NextDecision(BaseSerializer s, int order, ref DecisionSerialize d);
    private NextDecision hasNextDecision;
    private bool NextDecisionLoad(BaseSerializer s, int order, ref DecisionSerialize d)
    {
      if (!s.PushSection(DecisionNodeNames.PossibilityString, order))
        return false;
      PossibilitySerialize[] ps = new PossibilitySerialize[order + 1];
      if (order > 0)
        d.possibilities.CopyTo(ps, 0);

      d.possibilities = ps;
      return true;
    }
    private bool NextDecisionSave(BaseSerializer s, int order, ref DecisionSerialize d)
    {
      if (order >= d.possibilities.Length)
        return false;
      return s.PushSection(DecisionNodeNames.PossibilityString, order);
    }

    private void Serialize(Serializer.BaseSerializer r, ref DecisionSerialize ds)
    {
      r.SerializeString(DecisionNodeNames.DescriptionString, ref ds.description);
      r.PushSection(DecisionNodeNames.PossibilitiesString, 0);

      int order = 0;
      while (hasNextDecision(r, order, ref ds))
      {
        ref PossibilitySerialize ps = ref ds.possibilities[order];
        r.SerializeString(DecisionNodeNames.ActionString, ref ps.action);
        r.SerializeString(DecisionNodeNames.ReactionString, ref ps.reaction);
        r.SerializeString(DecisionNodeNames.ItemString, ref ps.item);
        r.SerializeInt(DecisionNodeNames.IdString, ref ps.id);
        r.PopSection();
        order++;
      }
      r.PopSection();
    }
    private class TempId
    {
      public int Id { get; set; }
    }
    public bool Load(Serializer.BaseSerializer r)
    {
      hasNextDecision = NextDecisionLoad;
      DecisionSerialize ds = new DecisionSerialize();
      Serialize(r, ref ds);
      Data = new DecisionData();
      // remove default one
      Data.Posibilities.Clear();
      Data.Description = ds.description;
      for ( int i =0; i < ds.possibilities.Length; i++)
      {
        PossibilitySerialize ps = ds.possibilities[i];
        DecisionPossibilities dp = new DecisionPossibilities();
        dp.Id = new TempId() { Id = ps.id };
        dp.Action = ps.action;
        dp.Reaction = ps.reaction;
        dp.Item = ps.item;
        Data.Posibilities.Add(dp);
      }
      
      return true;
    }
    public override void Finish( BaseSerializer s)
    {
      if (!s.IsLoading)
        return;
      for ( int i =0; i < Data.Posibilities.Count; i++ )
      {
        TempId l = Data.Posibilities[i].Id as TempId;
        Data.Posibilities[i].Id = s.fromId(l.Id);
      }
      Create();
    }
    public override void Serialize(BaseSerializer s)
    {
      if (s.IsLoading)
        Load(s);
      else
        Save(s);
    }
  }
}
