using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static MyBook.BookContent.ParagraphDefinition;

namespace MyBook.BookContent
{ 

  public struct ParagraphSerialization
  {
    public string text;
    public int type;
    public int parameter;
  }

  public struct BlockSerialization
  {
    public ParagraphSerialization[] paragraphs;
  }
  public struct EpisodesSerializeData
  {
    public string episodeName;
    public BlockSerialization[] blocks;
  }
  
  public class ParagraphDefinition
  {
    public enum Decoration
    {
      Text,
      Link,
      Bold,
      Color
    }

    public String content;
    public Decoration decoration;
    public int parameter;
    public void FromSerialize(ParagraphSerialization s)
    {
      content = s.text;
      decoration = (Decoration)s.type;
      parameter = s.parameter;
    }
    public ParagraphSerialization ToSerialize()
    {
      ParagraphSerialization s = new ParagraphSerialization();
      s.text = content;
      s.parameter = parameter;
      s.type = (int)decoration;
      return s;
    }
  }
  public class BlockDefinition
  {
    public List<ParagraphDefinition> Paragraphs { get; set; }
    public void FromSerialize(BlockSerialization s)
    {
      for (int i = 0; i < s.paragraphs.Length; i++)
      {
        ParagraphDefinition p = new ParagraphDefinition();
        p.FromSerialize(s.paragraphs[i]);
        Paragraphs.Add(p);
      }
    }
    public BlockSerialization ToSerialize()
    {
      BlockSerialization s = new BlockSerialization();
      s.paragraphs = new ParagraphSerialization[Paragraphs.Count];
      for (int i = 0; i < Paragraphs.Count; i++)
      {
        s.paragraphs[i] = Paragraphs[i].ToSerialize();
      }
      return s;
    }
    public BlockDefinition()
    {
      Paragraphs = new List<ParagraphDefinition>();
    }
  }
  public class EpisodeDetail
  {
    public String Title { get; set; }
    public List<BlockDefinition> Blocks { get; set; }
    public EpisodeDetail()
    {
      Blocks = new List<BlockDefinition>();
    }
    public EpisodesSerializeData ToSerialize()
    {
      EpisodesSerializeData es = new EpisodesSerializeData();
      es.episodeName = Title;
      es.blocks = new BlockSerialization[Blocks.Count];
      for(int p =0; p < Blocks.Count; p++)
      {
        es.blocks[p] = Blocks[p].ToSerialize();
      }
      return es;
    }
    public void FromSerialize(EpisodesSerializeData d)
    {
      Title = d.episodeName;
      Blocks = new List<BlockDefinition>();

      for (int i = 0; i < d.blocks.Length; i++)
      {
        BlockDefinition def = new BlockDefinition();
        def.FromSerialize(d.blocks[i]);
        Blocks.Add(def);
      }
    }
  }
}
