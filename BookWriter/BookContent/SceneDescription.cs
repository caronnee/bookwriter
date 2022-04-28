using RiddleInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.BookContent
{
  public struct PageSerializeData
  {
    public String type;
    public int order;
    public IRiddleHandler handler;
  }
  public struct SceneSerializeData
  {
    public String name;
    public PageSerializeData[] pages;
  }
  public struct ScenesSerializeData
  {
    public SceneSerializeData[] scenes;
  }
  public class SceneDescription
  {
    // name of the scene
    public String Name { get; set; }
    public String Summary { get; set; }

    // content of the book for writing
    public List<IRiddleHandler> Pages { get; set; }

    // constructor
    public SceneDescription()
    {
      Pages = new List<IRiddleHandler>();
    }
    public SceneSerializeData ToSerialize()
    {
      SceneSerializeData d = new SceneSerializeData();
      d.pages = new PageSerializeData[Pages.Count];
      d.name = Name;
      for (int i = 0; i < Pages.Count; i++)
      {
        PageSerializeData pd = new PageSerializeData();
        pd.type = Pages[i].Name;
        pd.order = i;
        pd.handler = Pages[i];
        d.pages[i] = pd;
      }
      return d;
    }
  }

}
