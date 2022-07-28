using RiddleInterface;
using System;
using System.Collections.Generic;

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
    public String id;
    public String name;
    public PageSerializeData[] pages;
  }
  public struct ScenesSerializeData
  {
    public SceneSerializeData[] scenes;
  }
  public class SceneDescription
  {
    public int Id { get;set; }
    // name of the scene
    public String Name { get; set; }
    public String Summary { get; set; }

    // content of the book for writing
    public List<IRiddleHandler> Pages { get => _pages; }

    public IRiddleHandler CurrentPage { get => _currentPage; }
    public int CurrentPosition { get => _position; }
    // constructor
    private int _position;
    private IRiddleHandler _currentPage;
    
    private List<IRiddleHandler> _pages;
    public SceneDescription()
    {
      _pages = new List<IRiddleHandler>();
      _currentPage = null;
      _position = -1;
    }
    public void SetPage(int i)
    {
      _position = i;
      _currentPage = Pages[i];
    }

    public SceneSerializeData ToSerialize()
    {
      SceneSerializeData d = new SceneSerializeData();
      d.pages = new PageSerializeData[Pages.Count];
      d.id = Id.ToString();
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
