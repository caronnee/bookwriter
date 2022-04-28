using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.BookContent
{
  public struct EpisodesSerialization
  {
    public string episodeName;
    public string content;
  }
  public struct CharacterSerializeData
  {
    public String summary;
    public String name;
    public int id;
    public int father;
    public int mother;
    public int spouse;
    public int gender;
    public int status;
    public EpisodesSerialization[] episodes;
  }

  public struct CharactersSerializeData
  {
    public CharacterSerializeData[] characters;
  }

  public class CharacterEpisodes
  {
    public String Title { get; set; }
    public String Content { get; set; }
  }

  // whole info about character
  public class CharacterDescription
  {
    // unique identificator of the character
    public int Id { get; set; }

    // name of the character
    public String Name { get; set; }
    public String Summary { get; set; }

    // current status of the character
    public CharacterStatus Status { get; set; }

    // Gender. Male / female ;)
    public CharacterGender Gender { get; set; }

    // details
    public List<CharacterEpisodes> Info { get; set; }

    /// <summary>
    /// Functions
    /// </summary>
    public CharacterDescription()
    {
      Info = new List<CharacterEpisodes>();
    }

    public void FromSerialize(CharacterSerializeData d)
    {
      Name = d.name;
      Id = d.id;
      Summary = d.summary;
      Gender = (CharacterGender)d.gender;
      Status = (CharacterStatus)d.status;
      // todo 
      //Mother. father, sppuse
      Info = new List<CharacterEpisodes>();
      foreach (EpisodesSerialization s in d.episodes)
      {
        CharacterEpisodes ep = new CharacterEpisodes();
        ep.Content = s.content;
        ep.Title = s.episodeName;
        Info.Add(ep);
      }
    }
    public CharacterSerializeData ToSerialize()
    {
      CharacterSerializeData d = new CharacterSerializeData();
      d.name = Name;
      d.id = Id;
      d.summary = Summary;
      d.gender = ((int)Gender);
      d.status = ((int)Status);
      d.episodes = new EpisodesSerialization[this.Info.Count];
      for (int i = 0; i < Info.Count; i++)
      {
        EpisodesSerialization es = new EpisodesSerialization();
        es.episodeName = Info[i].Content;
        es.content = Info[i].Content;
        d.episodes[i] = es;
      }
      return d;
    }
  }

}
