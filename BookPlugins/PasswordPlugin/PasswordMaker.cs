using RiddleInterface;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PasswordPlugin
{
  public class PasswordMaker : IRiddleHandler
  {
    #region Provided by interface

    public string Name { get { return "Password"; } }
    public Control Settings { get; set; }
    public Control Viewport { get; set; }
    public List<Outcome> Outcomes { get; set; }
    public OnSuccessAction onAnswer { get; set; }

    #endregion

    public PasswordData Data { get; set; }

    public PasswordMaker()
    {
      Data = new PasswordData();
      Data.Definition.AcceptableAnswer = "Hooray";
      Outcomes = new List<Outcome>();
    }

    private void Answered()
    {
      PasswordBox b = DisplayPage as PasswordBox;
      b.x_final_answer.Visibility = Visibility.Visible;
      b.x_final_answer.Text = "";
      foreach (string s in Data.Answer.Reactions)
      {
        b.x_final_answer.Text += s + Environment.NewLine;
      }

      if ( Data.Answer.Id >=0 )
      {
        // complete answer was done
        // hide "send" button
        b.x_answer_container.Visibility = Visibility.Collapsed;
      }
    }

    public void CreateReadOnly()
    {
      PasswordBox b = new PasswordBox();
      DisplayPage = b;
      PasswordWriteBox p = Viewport as PasswordWriteBox;
      b.x_description.Text = p.x_description.Text;
      
      // answer was done
      if ( Data.Answer.Id >=0 )
      {
        Answered();        
      }
      else
      {
        b.x_send_answer.Click += X_send_answer_Click;
      }
    }

    public void ClearAnswer()
    {
      Data.Answer = new AnswerData();
    }

    private void X_send_answer_Click(object sender, RoutedEventArgs e)
    {
      //
      PasswordBox b = DisplayPage as PasswordBox;
      if (Data.Definition.AcceptableAnswer == b.x_answer.Text)
      {
        int id = Data.Definition.SuccessId;
        Data.Answer.Reactions.Add(Data.Definition.SuccessReaction);
        Data.Answer.Id = id;
        Answered();
        if ( onAnswer!=null )
        {
          onAnswer(id);
        }
      }
      else
      {
        if (Data.Answer.Failures == Data.Definition.NAllowedFailures)
        {
          int id = Data.Definition.FailureId;
          Data.Answer.Id = id;
          Data.Answer.Reactions.Add(Data.Definition.FailureReaction);
          if (onAnswer != null)
          {
            onAnswer(id);
          }
        }
        else 
        {
          if (Data.Definition.NAllowedFailures >= 0)
          {
            int index = Data.Answer.Failures;
            Data.Answer.Failures++;
            Data.Answer.Reactions.Add(Data.Definition.Hints[index].Hint);
          }
        }
        Answered();
      }
    }

    // 
    public Control DisplayPage { get; set; }

    // folder for data. Not used for password. Yet
    public string BaseFolder { get; set; }

    public void Save(IRiddleSerializer r)
    {
      r.SaveParameter(PasswordNodeNames.IdString, Name);
      r.SaveValue(PasswordNodeNames.DescString, Data.Definition.Description);
      r.SaveValue(PasswordNodeNames.AnswerString, Data.Definition.AcceptableAnswer);
      r.SaveValue(PasswordNodeNames.FailureString, Data.Definition.FailureId.ToString());
      r.SaveValue(PasswordNodeNames.FailureReactionString, Data.Definition.FailureReaction);
      r.SaveValue(PasswordNodeNames.FailuresString, Data.Definition.NAllowedFailures.ToString());
      r.SaveValue(PasswordNodeNames.SuccessString, Data.Definition.SuccessId.ToString());
      r.SaveValue(PasswordNodeNames.SuccessReactionString, Data.Definition.SuccessReaction);

      r.StartSection(PasswordNodeNames.HintsString);
      for ( int i = 0; i < Data.Definition.Hints.Count; i++)
      {
        r.SaveValue(PasswordNodeNames.HintString,Data.Definition.Hints[i].Hint);
      }
      r.EndSection();
      
      // answer does not need to be saved, because ot will be saved as in progress
    }

    public bool Load(IRiddleSerializer r)
    {
      string s = r.LoadParameter(PasswordNodeNames.IdString);
      if (s != Name)
        return false;
      Data = new PasswordData();
      Data.Definition.Description = r.LoadSection(PasswordNodeNames.DescString);
      Data.Definition.AcceptableAnswer = r.LoadSection(PasswordNodeNames.AnswerString);
      s = r.LoadSection(PasswordNodeNames.FailureString);
      Data.Definition.FailureId = System.Int32.Parse(s);
      Data.Definition.FailureReaction = r.LoadSection(PasswordNodeNames.FailureReactionString);
      s = r.LoadSection(PasswordNodeNames.FailuresString);
      Data.Definition.NAllowedFailures = System.Int32.Parse(s);
      s = r.LoadSection(PasswordNodeNames.SuccessString);
      Data.Definition.SuccessId = System.Int32.Parse(s);
      Data.Definition.SuccessReaction = r.LoadSection(PasswordNodeNames.SuccessReactionString);

      r.StartSection(PasswordNodeNames.HintsString);
      int osize = r.Children();
      for (int i = 0; i < osize; i++)
      {
        r.LoadSection(i);
        HintItem item = new HintItem();
        item.Hint = r.LoadValue();
        Data.Definition.Hints.Add(item);
        r.EndSection();
      }
      r.EndSection();
      Create();
      return true;
    }
    
    private void CreateWrite()
    {
      PasswordWriteBox b = new PasswordWriteBox();
      Viewport = b;
      Viewport.DataContext = this;
      if (Outcomes.Count > 0)
      {
        b.x_fail.SelectedItem = Outcomes[Data.Definition.FailureId];
        b.x_success.SelectedItem = Outcomes[Data.Definition.SuccessId];
      }
      b.x_countdown.SelectedIndex = Data.Definition.Hints.Count;
      b.x_hints_holder.Visibility = Data.Definition.Hints.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
      b.RefillHints();
      b.x_countdown.SelectionChanged += b.x_countdown_SelectionChanged;
    }

    public void Create()
    {
      // setting are the same for the book
      Settings = new PasswordSetting();
      CreateWrite();      
      CreateReadOnly();
    }
  }
}
