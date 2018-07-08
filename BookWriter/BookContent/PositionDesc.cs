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
    public int ParagraphId
    {
      get;
      set;
    }

    public int ContentPos
    {
      get;
      set;
    }

    public int Lenght
    {
      get;
      set;
    }

    public void Clear()
    {
      Lenght = 0;
      ParagraphId = 0;
      ContentPos = 0;
    }

    bool IsInvalid()
    {
      return Lenght < 0;
    }
  }
}
