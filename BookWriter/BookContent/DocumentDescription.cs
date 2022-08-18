using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.BookContent
{
  public struct DocumentSerializeData
  {
    public string name;
    public string content;
    public string misc;
    public string summary;
  }
  public struct DocumentsSerializeData
  {
    public DocumentSerializeData[] documents;
  }

  public class DocumentDescription : IDescription
  {
    public string Content { get; set; }
    //public String Date { get; set; }
    //public String Author { get; set; }
    //public String Group { get; set; }
    // Where was the document found, some info, bloof sputters, etc etc
    public string Misc { get; set; }
    public DocumentSerializeData ToSerialize()
    {
      DocumentSerializeData ret = new DocumentSerializeData();
      ret.misc = Misc;
      ret.content = Content;
      ret.name = Name;
      ret.summary = Summary;
      return ret;
    }
    public void FromSerialize(DocumentSerializeData d)
    {
      Misc = d.misc;
      Content = d.content;
      Name = d.name;
      Summary = d.summary;
    }
  }
}
