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
    public double[] vertices;
  }
  public struct ModelsSerializeData
  {
    public ModelSerializeData[] models;
  }
  public class ModelDescription
  {
    public String Name { get; set; }
    public String Desc { get; set; }
    public List<double> Vertices { get; set; }
    public void FromSerialize(ModelSerializeData data)
    {
      Name = data.name;
      Desc = data.description;
      Vertices = new List<double>();
      for ( int i =0;i < data.vertices.Length;i++ )
      {
        Vertices.Add(data.vertices[i]);
      }
    }
    public ModelSerializeData ToSerialize()
    {
      ModelSerializeData m = new ModelSerializeData();
      m.vertices = new double[Vertices.Count];
      for ( int i = 0; i < Vertices.Count; i++ )
      {
        m.vertices[i] = Vertices[i];
      }
      return m;
    }
  }
}
