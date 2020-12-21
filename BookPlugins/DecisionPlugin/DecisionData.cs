using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionPlugin
{
  [Serializable]
  public class PostAnswerData
  {
    // what was answered
    public String ActionTaken;

    // written after answer
    public String Reaction;

    // where to go
    public int id;
  }

  [Serializable]
  public class DecisionPossibilities
  {
    // id of the outcome
    public int Id { get; set; }

    // reaction
    public String Reaction { get; set; }

    // what is the authors decision
    public String Action { get; set; }

    // Item to gain
    public String Item { get; set; }

    public DecisionPossibilities()
    {
      Action ="User would like to do this:...";
      Reaction = "reaction";
      Item = "item to gain";
    }
  }

  [Serializable]
  public class DecisionData
  {
    // description of the problem
    public String Description { get; set; }

    // what was set up as possible outcomes
    public List<DecisionPossibilities> Posibilities { get; set; }

    public DecisionData()
    {
      Posibilities = new List<DecisionPossibilities>();
      Posibilities.Add(new DecisionPossibilities());
      Description = "Here comes the text";
    }

    void Clear()
    {
      Posibilities.Clear();
      Description = "";
    }
  }
}
