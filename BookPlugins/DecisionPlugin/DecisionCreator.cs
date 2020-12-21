using RiddleInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;

namespace DecisionPlugin
{
  public class DecisionCreator : IRiddleHandler
  {
    public DecisionData Data { get; set; }
    
    // detail about user input
    public PostAnswerData Answer { get; set; }

    public DecisionCreator()
    {
      Outcomes = new List<Outcome>();
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

    public OnSuccessAction onAnswer { get; set; }

    public string Name { get { return "Decision"; } }

    public List<Outcome> Outcomes { get; set; }

    public Control Settings { get; set; }

    public Control Viewport { get; set; }

    public Control DisplayPage { get; set; }

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
          Outcome o = Outcomes.Find(x => x.Id == s.Id);
          // this must be valid. Solved while loading
          System.Diagnostics.Debug.Assert(o != null);
          bt.Content = s.Action;
          bt.DataContext = s;
          bt.Click += Bt_Click;
          b.x_decisions.Children.Add(bt);
        }
      }
    }

    private void Bt_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      Button b = sender as Button;
      if (onAnswer!=null)
      {
        DecisionPossibilities a = b.DataContext as DecisionPossibilities;
        Answer = new PostAnswerData();
        Answer.id = a.Id;
        Answer.Reaction = a.Reaction;
        Answer.ActionTaken = a.Action;
        onAnswer(a.Id);
      }
      Answered();
    }
    
    public void Save(Stream stream)
    {
      IFormatter formatter = new BinaryFormatter();
      formatter.Serialize(stream, Data);
    }
    public void Load(Stream stream)
    {
      IFormatter formatter = new BinaryFormatter();
      Data = formatter.Deserialize(stream) as DecisionData;
      Create();
    }
  }
}
