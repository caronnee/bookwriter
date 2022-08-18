using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.BookContent
{
  public struct ModelSerializeData
  {
    public string name;
    public string description;
    public string file;
    public string summary;
  }
  public struct ModelsSerializeData
  {
    public ModelSerializeData[] models;
  }
  public class ModelDescription :IDescription
  {
    public String Desc { get; set; }
    public String File { get; set; }
    
    public void FromSerialize(ModelSerializeData data)
    {
      Name = data.name;
      Desc = data.description;
      File = data.file;
      Summary = data.summary;
    }
    public ModelSerializeData ToSerialize()
    {
      ModelSerializeData m = new ModelSerializeData();
      m.summary = Summary;
      m.name = Name;
      m.description = Desc;
      m.file = File;
      return m;
    }
  }
}
