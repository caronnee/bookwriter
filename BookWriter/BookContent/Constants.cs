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
    Male,
    Female
  }

  public enum CharacterStatus
  {
    Alive,
    Dead,
    Unknown
  }  
}
