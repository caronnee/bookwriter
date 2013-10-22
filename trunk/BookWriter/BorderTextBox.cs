using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyBook
{
    class BorderTextBox : LimitedTextBox
    {
        String pagesContent;

        public BorderTextBox()
        {
            pagesContent = "";
        }
        public void Load(String bookName)
        {
            StreamReader reader = new StreamReader(bookName);
            pagesContent = reader.ReadToEnd();
            reader.Close(); // TODO save end of the page
        }
        public void Save(StreamWriter writer)
        {
            writer.Write(pagesContent);
        }
       // any method that takes a string as the parameter and returns no value
        public delegate void MoveHandler(int caret);

        // Define an Event based on the above Delegate
        public event MoveHandler moveHandler;

        // we have prev and next
        public override void MakeActive(String str)
        {
            if (moveHandler == null)
                return;
            moveHandler(str.Length);
        }
        public override bool Move(PageMove move)
        {
            
            return true;
        }
    }
}
