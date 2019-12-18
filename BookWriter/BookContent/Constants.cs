using System;

namespace MyBook.BookContent
{
  static public class Constants
  {
    static public String Extension = ".book";
    static public String SearchExt = "*.book";
  }
  
  public enum CharacterGender
  {
    Man,
    Female
  }

  public enum CharacterStatus
  {
    Alive,
    Dead,
    Unknown
  }  
}
