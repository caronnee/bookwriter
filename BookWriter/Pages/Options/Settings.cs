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

    public static int LineCount
    {
      get;
      set;
    }

    public static int TextSizeCoef
    {
      get;
      set;
    }

    public static Settings BookSetting
    {
      get
      {
        return settings;
      }
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
      file.WriteLine("LineCount={0}", LineCount);
      file.Close();
    }

    private Settings()
    {
      Author = "Eva Pešková";
      // Default initialization
      BooksFolder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Books";
      TextSizeCoef = 1;
      LineCount = 3;

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
          if (split[0] == "LineCount")
          {
            int o;
            if (int.TryParse(split[1], out o) == true)
              LineCount = o;
            continue;
          }
          if ( split[0] == "textcoef" )
          {
            int o;
            if (int.TryParse(split[1],out o) == true)
              TextSizeCoef = o;
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
