using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PasswordPlugin
{
  [Serializable]
  public class LamerData : IValueConverter
  {
    // from data to combobox
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      int t = (int) value;
      if (t < 0)
        return (int)parameter;
      return t;
    }

    // from combobox to data
    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      int max = Int32.Parse(parameter as String);
      int current = (int)value;
      if (current == max)
        return -1;
      return current;
    }
  }

  [Serializable]
  // What user tries
  public class AnswerData
  {
    // what User inserted
    public List<String> Answers { get; set; }
    
    // reaction t what user inserted
    public List<String> Reactions { get; set; }

    // id of where to jump
    public string Id { get; set; }

    // How many failures can be yet done
    public int Failures { get; set; }

    public AnswerData()
    {
      Answers = new List<string>();
      Reactions = new List<string>();
      Id = "";
      Failures = 0;
    }
  }

  public class HintItem
  {
    public String Hint { get; set; }
  }

  public class PasswordDefinition
  {
    // allowed failured from user. If this is -1, fail
    public int NAllowedFailures { get; set; }

    // description of the problem
    public String Description { get; set; }

    // answer
    public String AcceptableAnswer { get; set; }

    // what happens when succeeded
    public string SuccessId { get; set; }

    // 
    public String FailureReaction { get; set; }

    //
    public String SuccessReaction { get; set; }

    // where to go when failed
    public string FailureId { get; set; }

    // Count of hints is number of allowed failures
    public HintItem[] Hints { get; set; }

    // constructor
    public PasswordDefinition()
    {
      Hints = new HintItem[5];
      for ( int i =0; i < 5; i++)
      {
        Hints[i] = new HintItem();
      }
    }
  }

  public class PasswordData
  {
    // user input
    public AnswerData Answer { get; set; }

    //
    public PasswordDefinition Definition { get; set; }

    public PasswordData()
    {
      Definition = new PasswordDefinition();
      Answer = new AnswerData();
    }
  }
}
