using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

/** Here are only the function to create dummy content, dymmy tests */
namespace MyBook
{
    class TestHelper
    {
        static string dummyName = "../dummyContent.book";
        static public String CreateDummyBook()
        {
            if (!File.Exists(dummyName))
            {
                BookSource source = new BookSource(dummyName);
                source.AddPage("Test 1");
                source.AddPage("Test 2");
                source.AddPage("Test 3");
                source.AddPage("Test 4");
                source.Save();
            }
          return dummyName;
        }
    }
}
