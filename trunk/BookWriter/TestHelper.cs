using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyBook
{
    class TestHelper
    {
        static string dummyName = "../dummyContent.book";
        static public String CreateDummyBook()
        {
          SourceText source = new SourceText(dummyName);
          source.AddPage("Test 1");
          source.AddPage("Test 2");
          source.AddPage("Test 3");
          source.AddPage("Test 4");
          source.Save();
          return dummyName;
        }
    }
}
