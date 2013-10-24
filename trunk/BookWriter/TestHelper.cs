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
            return dummyName;
        }
    }
}
