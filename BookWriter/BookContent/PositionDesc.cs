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

    // position in book
    public int ContentPos
    {
      get;
      set;
    }

    // size that this can occupy
    public int Lenght
    {
      get;
      set;
    }

    public void Clear()
    {
      Lenght = 0;
      ParagraphId = -1;
      ContentPos = 0;
    }

    bool IsInvalid()
    {
      return Lenght < 0;
    }
  }
}
