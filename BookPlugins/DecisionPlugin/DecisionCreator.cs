using RiddleInterface;
using Serializer;
using System.Collections.Generic;
using System.Windows.Controls;

namespace DecisionPlugin
{
  public class DecisionCreator : IRiddleHandler
  {
    public int Order { get; set; }
    public string Name { get { return "Decision"; } }

    public Control Settings { get; set; }

    public Control Viewport { get; set; }

    public Control DisplayPage { get; set; }

    public string BaseFolder { get; set; }

    public DecisionData Data { get; set; }
    
    // detail about user input
    public PostAnswerData Answer { get; set; }

    public DecisionCreator()
    {
      Data = new DecisionData();
      Settings = null;
      Viewport = null;
      ClearAnswer();
    }

    public void Create()
    {
      DecisionWriteBox b = new DecisionWriteBox();
      b.DataContext = this;
      Viewport = b;
    }

    public void Answered()
    {
      DecisionBox b = DisplayPage as DecisionBox;
      b.x_decisions.Visibility = System.Windows.Visibility.Collapsed;
      b.x_description.Text += "\n" + Answer.Reaction;
    }

    public void ClearAnswer()
    {
      Answer = null;
    }

    public void CreateReadOnly()
    {
      DecisionBox b = new DecisionBox();
      DisplayPage = b;
      b.x_description.Text = Data.Description;
      if ( Answer != null)
      {
        Answered();
      }
      else
      {
        for (int i = 0; i < Data.Posibilities.Count; i++)
        {
          DecisionPossibilities s = Data.Posibilities[i];
          Button bt = new Button();
          //Outcome o = Outcomes.Find(x => x.Id == s.Id);
          // this must be valid. Solved while loading
          //System.Diagnostics.Debug.Assert(o != null);
          bt.Content = s.Action;
          bt.DataContext = s;
          bt.Click += Bt_Click;
          b.x_decisions.Children.Add(bt);
        }
      }
    }

    private void Bt_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      //Button b = sender as Button;
      //if (false)
      //{
      //  DecisionPossibilities a = b.DataContext as DecisionPossibilities;
      //  Answer = new PostAnswerData();
      //  Answer.id = a.Id;
      //  Answer.Reaction = a.Reaction;
      //  Answer.ActionTaken = a.Action;
      //}
      Answered();
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
      for (int i =0; i < Data.Posibilities.Count; i++)
      {
        DecisionPossibilities dp = Data.Posibilities[i];
        PossibilitySerialize ps = new PossibilitySerialize();
        ps.id = dp.Id;
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
      PossibilitySerialize[] ps = new PossibilitySerialize[order+1];
      if (order >0)
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
        dp.Id = ps.id;
        dp.Action = ps.action;
        dp.Reaction = ps.reaction;
        dp.Item = ps.item;
        Data.Posibilities.Add(dp);
      }
      
      Create();
      return true;
    }

    public void Serialize(BaseSerializer s)
    {
      if (s.IsLoading)
        Load(s);
      else
        Save(s);
    }
  }
}
