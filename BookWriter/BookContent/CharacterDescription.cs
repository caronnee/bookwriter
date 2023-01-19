using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.BookContent
{
  
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
    public EpisodesSerializeData[] episodes;
  }

  public struct CharactersSerializeData
  {
    public CharacterSerializeData[] characters;
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
    public List<EpisodeDetail> Episodes { get; set; }

    /// <summary>
    /// Functions
    /// </summary>
    public CharacterDescription()
    {
      Episodes = new List<EpisodeDetail>();
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
      Episodes = new List<EpisodeDetail>();
      foreach (EpisodesSerializeData s in d.episodes)
      {
        EpisodeDetail ep = new EpisodeDetail();
        ep.FromSerialize(s);
        Episodes.Add(ep);
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
      d.episodes = new EpisodesSerializeData[Episodes.Count];
      for (int i = 0; i < Episodes.Count; i++)
      {
        EpisodesSerializeData es = Episodes[i].ToSerialize();
        d.episodes[i] = es;
      }
      return d;
    }
  }

}
