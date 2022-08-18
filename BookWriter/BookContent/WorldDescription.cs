using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.BookContent
{
  public struct MapSerializeData
  {
    public String name;
    public String content;
  }
  public struct LocationSerializeData
  {
    public string name;
    public string summary;
    public MapSerializeData[] maps;
    public MapSerializeData[] details;
  }
  public struct LocationsSerializeData
  {
    public LocationSerializeData[] locatios;
  }
  public class LocationPart
  {
    public String Title { get; set; }
    public String Content { get; set; }
  }
  public class MapPart
  {
    public String Name { get; set; }
    public String MapSource { get; set; }
  }
  public class WorldDescription : IDescription
  {
    public List<LocationPart> Info { get; set; }
    public List<MapPart> Maps { get; set; }
    public WorldDescription()
    {
      Info = new List<LocationPart>();
      Maps = new List<MapPart>();
    }
    // serialization
    public LocationSerializeData ToSerialize()
    {
      LocationSerializeData d = new LocationSerializeData();
      d.details = new MapSerializeData[Info.Count];
      d.maps = new MapSerializeData[Maps.Count];
      d.name = Name;
      d.summary = Summary;
      for (int i = 0; i < Info.Count; i++)
      {
        MapSerializeData data = new MapSerializeData();
        data.content = Info[i].Content;
        data.name = Info[i].Title;
        d.details[i] = data;
      }
      for (int i = 0; i < Maps.Count; i++)
      {
        MapSerializeData data = new MapSerializeData();
        // todo image
        data.content = Maps[i].MapSource;
        data.name = Maps[i].Name;
        d.maps[i] = data;
      }
      return d;
    }
    public void FromSerialize(LocationSerializeData s)
    {
      Name = s.name;
      Summary = s.summary;
      Info = new List<LocationPart>();
      Maps = new List<MapPart>();
      if (s.details!=null)
        {
        for (int i = 0; i < s.details.Length; i++)
        {
          LocationPart p = new LocationPart();
          p.Content = s.details[i].content;
          p.Title = s.details[i].name;
          Info.Add(p);
        }
      }
      if (s.maps!= null)
      {
        for (int i = 0; i < s.maps.Length; i++)
        {
          MapPart m = new MapPart();
          m.MapSource = s.maps[i].content;
          m.Name = s.maps[i].name;
          Maps.Add(m);
        }
      }
    }
  }
}

