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
      public List<PossibilitySerialize> possibilities;
    }

    public void Save(Serializer.BaseSerializer r)
    {
      DecisionSerialize ds = new DecisionSerialize();
      ds.description = Data.Description;
      ds.possibilities = new List<PossibilitySerialize>();
      for (int i =0; i < Data.Posibilities.Count; i++)
      {
        DecisionPossibilities dp = Data.Posibilities[i];
        PossibilitySerialize ps = new PossibilitySerialize();
        ps.id = dp.Id;
        ps.action = dp.Action;
        ps.reaction = dp.Reaction;
        ps.item = dp.Item;
        ds.possibilities.Add(ps);
      }
      Serialize(r, ref ds);
    }
    private void Serialize(Serializer.BaseSerializer r, ref DecisionSerialize ds)
    {
      r.SerializeString(DecisionNodeNames.DescriptionString, ref ds.description);
      string dummystring = "";
      int order = 0;
      while( r.PushSection(DecisionNodeNames.PossibilitiesString, order, dummystring, ref dummystring) )
      {
        if (r.IsLoading)
          ds.possibilities = new List<PossibilitySerialize>();
        int pOrder = 0;
        while (r.PushSection(DecisionNodeNames.PossibilityString, order, dummystring, ref dummystring))
        {
          PossibilitySerialize ps = ds.possibilities[pOrder];
          r.SerializeString(DecisionNodeNames.ActionString, ref ps.action);
          r.SerializeString(DecisionNodeNames.ReactionString, ref ps.reaction);
          r.SerializeString(DecisionNodeNames.ItemString, ref ps.item);
          r.SerializeInt(DecisionNodeNames.IdString, ref ps.id);
          if (r.IsLoading)
            ds.possibilities.Add(ps);
          r.PopSection();
          pOrder++;
        }
        r.PopSection();
      }
    }

    public bool Load(Serializer.BaseSerializer r)
    {
      DecisionSerialize ds = new DecisionSerialize();
      Serialize(r, ref ds);
      Data = new DecisionData();
      Data.Description = ds.description;
      for ( int i =0; i < ds.possibilities.Count; i++)
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
