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

        public BookParagraph()
        {
          Content = new StringBuilder();
        }
        public void Load(XmlNode paragraph)
        {
          Content = new StringBuilder (paragraph.InnerText);
          Content = Content.Replace("\r", "");
          Content = Content.Replace("\n", "");
          Content = Content.Replace("\t", "");
        }

    }
}
