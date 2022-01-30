using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyBook
{
  public class Settings
  {
    private static readonly Settings settings = new Settings();

    public static String TempFolder
    {
      get;
      set;
    }

    public static String BooksFolder
    {
      get;
      set;
    }

    public static String Author
    {
      get;
      set;
    }

    private string GetName()
    {
      string dir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      dir += "\\setting";
      return dir;
    }

    public void Save()
    {
      string str = GetName();
      System.IO.StreamWriter file = new System.IO.StreamWriter(str);
      file.WriteLine("booksfolder={0}", BooksFolder);
      file.Close();
    }

    private Settings()
    {
      TempFolder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      TempFolder += "\\temp\\";
      Author = "Eva Pešková";
      // Default initialization
      BooksFolder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Books";

      // load from the xml file
      string dir = GetName();
      try
      {
        System.IO.StreamReader file = new System.IO.StreamReader(dir);

        while (file.EndOfStream == false)
        {
          string str = file.ReadLine();
          string[] split = str.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

          // all the options
          if (split[0] == "Booksfolder")
          {
            BooksFolder = split[1];
            continue;
          }
        }
      }
      catch (Exception )
      {
      }
    }
  }
}
