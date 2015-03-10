using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MyBook.BookContent
{
    public class BookParagraph 
    {
        public StringBuilder Content
        {
            get;
            set;
        }

        public void Load(XmlNode paragraph)
        {
          StringBuilder trim = new StringBuilder(paragraph.InnerText);
          trim = trim.Replace("\r", "");
          trim = trim.Replace("\n", "");
          trim = trim.Replace("\t", "");
          Content = trim;
        }

    }
}
