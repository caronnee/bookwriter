using static MyBook.BookContent.BookSource;

namespace MyBook.BookContent
{
  // identifier what we are actually showing
  public class PositionDesc
  {
    public SceneDescription Scene
    {
      get;set;
    }

    // Index of the page in the scene. Needed for counting statistics
    public int ParagraphId
    {
      get;
      set;
    }

    public void Clear()
    {
      ParagraphId = 0;
    }

  }
}
