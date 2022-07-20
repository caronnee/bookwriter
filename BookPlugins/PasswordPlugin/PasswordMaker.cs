using RiddleInterface;
using Serializer;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PasswordPlugin
{
  public class PasswordMaker : IRiddleHandler
  {
    #region Provided by interface

    PasswordWriteBox _writer;
    PasswordBox _reader;
    public override string Name => "Password";

    public override Control Settings => null;
    public override Control Viewport => _writer;

    #endregion

    public PasswordData Data { get; set; }

    public PasswordMaker()
    {
      Data = new PasswordData();
      Data.Definition.AcceptableAnswer = "Hooray";
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

      if ( Data.Answer.Id == null )
      {
        // complete answer was done
        // hide "send" button
        b.x_answer_container.Visibility = Visibility.Collapsed;
      }
    }

    public void CreateReadOnly()
    {
      _reader = new PasswordBox();
      _reader.DataContext = Data;      
      // answer was done
      if ( Data.Answer.Id != null )
      {
        Answered();        
      }
      else
      {
        _reader.x_send_answer.Click += x_send_answer_Click;
      }
    }

    public override void ClearAnswer()
    {
      Data.Answer = new AnswerData();
    }

    private void x_send_answer_Click(object sender, RoutedEventArgs e)
    {
      //
      PasswordBox b = DisplayPage as PasswordBox;
      if (Data.Definition.AcceptableAnswer == b.x_answer.Text)
      {
        object id = Data.Definition.SuccessId;
        Data.Answer.Reactions.Add(Data.Definition.SuccessReaction);
        Data.Answer.Id = id;
        Answered();
      }
      else
      {
        if (Data.Answer.Failures == Data.Definition.NAllowedFailures)
        {
          object id = Data.Definition.FailureId;
          Data.Answer.Id = id;
          Data.Answer.Reactions.Add(Data.Definition.FailureReaction);
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
    public override Control DisplayPage => _reader;
    private void CreateWrite()
    {
      _writer = new PasswordWriteBox(this);
      
      int index = Data.Definition.NAllowedFailures;
      if (index < 0)
        index = _writer.x_countdown.Items.Count - 1;
      _writer.x_countdown.SelectionChanged += _writer.x_countdown_SelectionChanged;
      _writer.x_countdown.SelectedIndex = index;
    }

    public override void Create()
    {
      // setting are the same for the book
      CreateWrite();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////// Serialization ///////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////
    struct HintSerializeData
    {
      public string hint;
      public void Serialize(BaseSerializer s)
      {
        s.SerializeString(ref hint);
      }
    }
    struct PasswordSerializeData
    {
      public string description;
      public string acceptableAnswer;
      public int failureId;
      public string failureReaction;
      public int maxFailures;
      public int successId;
      public string successReaction;
      public HintSerializeData[] hints;
    }
    class TempId
    {
      public int hash;
    }
    public override void Finish(BaseSerializer s)
    {
      if (!s.IsLoading)
        return;
      // init crossreference
      TempId h = this.Data.Definition.SuccessId as TempId;
      System.Diagnostics.Debug.Assert(h != null);
      Data.Definition.SuccessId = s.fromId(h.hash);
      h = this.Data.Definition.FailureId as TempId;
      Data.Definition.FailureId = s.fromId( h.hash );
    }
    public void Save(Serializer.BaseSerializer serializer)
    {
      PasswordSerializeData psd = new PasswordSerializeData();
      psd.acceptableAnswer = Data.Definition.AcceptableAnswer;
      psd.description = Data.Definition.Description;
      psd.failureId = serializer.toId(Data.Definition.FailureId);
      psd.failureReaction = Data.Definition.FailureReaction;
      psd.maxFailures = Data.Definition.NAllowedFailures;
      psd.successId = serializer.toId(Data.Definition.SuccessId);
      psd.successReaction = Data.Definition.SuccessReaction;
      psd.hints = new HintSerializeData[5];
      for ( int i =0;i < Data.Definition.NAllowedFailures; i++)
      {
        psd.hints[i] = new HintSerializeData() { hint = Data.Definition.Hints[i].Hint };
      }
      Serialize(serializer,ref psd);
    }

    public bool Load(Serializer.BaseSerializer s)
    {
      PasswordSerializeData d = new PasswordSerializeData();
      Serialize(s, ref d);
      Data = new PasswordData();
      Data.Definition = new PasswordDefinition()
      {
        Description = d.description,
        AcceptableAnswer = d.acceptableAnswer,
        FailureId = new TempId() { hash = d.failureId },
        NAllowedFailures = d.maxFailures,
        FailureReaction = d.failureReaction,
        SuccessId = new TempId() { hash = d.successId },
        SuccessReaction = d.successReaction,
      };
      for ( int i =0;i < d.maxFailures; i++)
      {
        Data.Definition.Hints[i] = new HintItem() { Hint = d.hints[i].hint };
      }

      Create();
      return true;
    }

    private delegate bool HasNextHint(Serializer.BaseSerializer s, int num, ref PasswordSerializeData d);
    private HasNextHint hasNextHint;
    private HasNextHint hasHints;

    bool HasHintsLoad(Serializer.BaseSerializer s, int num, ref PasswordSerializeData d)
    {
      bool ret = s.PushSection(PasswordNodeNames.HintsString, num);
      d.hints = new HintSerializeData[5];
      return ret;
    }
    bool HasHintsSave(Serializer.BaseSerializer s, int num, ref PasswordSerializeData d)
    {
      if (d.hints == null)
        return false;
      return s.PushSection(PasswordNodeNames.HintsString, num);
    }

    bool HasNextHintSave(Serializer.BaseSerializer s, int num, ref PasswordSerializeData d)
    {
      if (num >= d.maxFailures)
        return false;
      return s.PushSection(PasswordNodeNames.HintString, num);
    }

    bool HasNextHintLoad(Serializer.BaseSerializer s, int num, ref PasswordSerializeData d)
    {
      bool ret = s.PushSection(PasswordNodeNames.HintString, num);
      //if (ret)
      //  d.hints.Add(new HintSerializeData() { hint = "unknown"});
      return ret;
    }

    private void Serialize( BaseSerializer serializer, ref PasswordSerializeData data)
    {
      serializer.SerializeString(PasswordNodeNames.DescString, ref data.description);
      serializer.SerializeString(PasswordNodeNames.AnswerString, ref data.acceptableAnswer);
      serializer.SerializeInt(PasswordNodeNames.FailureString, ref data.failureId);
      serializer.SerializeString(PasswordNodeNames.FailureReactionString, ref data.failureReaction);
      serializer.SerializeInt(PasswordNodeNames.FailuresString, ref data.maxFailures);
      serializer.SerializeInt(PasswordNodeNames.SuccessString, ref data.successId);
      serializer.SerializeString(PasswordNodeNames.SuccessReactionString, ref data.successReaction);
     
      if(hasHints(serializer,0,ref data))
      {
        int iHint = 0;
        while (hasNextHint(serializer, iHint,ref data))
        {
          data.hints[iHint].Serialize(serializer);
          serializer.PopSection();
          iHint++;
        }
        serializer.PopSection();
      }
    }
    public override void Serialize(BaseSerializer s)
    {
      if (s.IsLoading)
      {
        hasHints = HasHintsLoad;
        hasNextHint = HasNextHintLoad;
        Load(s);
      }
      else
      {
        hasHints = HasHintsSave;
        hasNextHint = HasNextHintSave;
        Save(s);
      }
    }
  }
}
