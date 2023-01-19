using MyBook.BookContent;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using static MyBook.BookContent.BookSource;

namespace MyBook.Write
{
  /// <summary>
  /// Interaction logic for EExpander.xaml
  /// </summary>
  public partial class EExpander : UserControl
  {
    public EExpander()
    {
      InitializeComponent();
    }

    bool Modified = false;
    private void Expander_Expanded(object sender, System.Windows.RoutedEventArgs e)
    {
      EpisodeDetail ep = DataContext as EpisodeDetail;
      x_episode_content.Blocks.Clear();
      for (int iBlock = 0; iBlock < ep.Blocks.Count; iBlock++)
      {
        BlockDefinition bd = ep.Blocks[iBlock];
        Paragraph activeParagraph = new Paragraph();
        x_episode_content.Blocks.Add(activeParagraph);
        for (int i = 0; i < bd.Paragraphs.Count; i++)
        {
          ParagraphDefinition d = bd.Paragraphs[i];
          switch (d.decoration)
          {
            case ParagraphDefinition.Decoration.Text:
              {
                activeParagraph.Inlines.Add(new Run(d.content));
                break;
              }
            case ParagraphDefinition.Decoration.Bold:
              {
                activeParagraph.Inlines.Add(new Bold(new Run(d.content)));
                break;
              }
            case ParagraphDefinition.Decoration.Link:
              {
                activeParagraph.Inlines.Add(new Hyperlink(new Run(d.content)));
                break;
              }
          }
        }
      }
    }

    private void Hyperlink_Click(object sender, RoutedEventArgs e)
    {
      MessageBox.Show("hyperlink clicked");
    }
    private void x_make_link(object sender, RoutedEventArgs e)
    {
      //RichTextBox1.Selection.Start.GetLineStartPosition(0).InsertParagraphBreak();
      Paragraph p = x_episode_textbox.Selection.Start.Paragraph;
      TextRange tr = new TextRange(x_episode_textbox.Selection.Start, x_episode_textbox.Selection.End);
      Hyperlink hlink = new Hyperlink(tr.Start, tr.End);
      //hlink.NavigateUri = new Uri("http://www.google.com/");
      hlink.Click += Hyperlink_Click;
    }
    private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      Modified = true;
    }

    private void RichTextBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
    {
      if (!Modified)
        return; // nothing to d
      // Save
      EpisodeDetail ep = DataContext as EpisodeDetail;
      // convert content to contentDetail
      ep.Blocks = new System.Collections.Generic.List<BlockDefinition>();
      for (Block block = x_episode_content.Blocks.FirstBlock
        ; block != null
        ; block = block.NextBlock
        )
      {
        // Save to the context episode
        Paragraph p = block as Paragraph;
        if (p == null)
          throw new System.Exception("Not implemented");
        BlockDefinition bd = new BlockDefinition();
        bd.Paragraphs = new System.Collections.Generic.List<ParagraphDefinition>();
        ep.Blocks.Add(bd);
        for ( Inline inline = p.Inlines.FirstInline; inline!=null; inline = inline.NextInline)
        {
          Run r = inline as Run;
          if(r!=null)
          {
            ParagraphDefinition def = new ParagraphDefinition();
            def.content = r.Text;
            def.decoration = ParagraphDefinition.Decoration.Text;
            bd.Paragraphs.Add(def);
            continue;
          }
          Hyperlink h = inline as Hyperlink;
          if (h!=null)
          {
            ParagraphDefinition def = new ParagraphDefinition();
            r = h.Inlines.FirstInline as Run;
            def.content = r.Text;
            def.decoration = ParagraphDefinition.Decoration.Link;
            // hash - todo
            def.parameter = 0;
            //def.content = h.Text;
            bd.Paragraphs.Add(def);
            continue;
          }
          Bold b = inline as Bold;
          if(b!=null)
          {
            ParagraphDefinition def = new ParagraphDefinition();
            r = b.Inlines.FirstInline as Run;
            def.content = r.Text;
            def.decoration = ParagraphDefinition.Decoration.Bold;
            //def.content = h.Text;
            bd.Paragraphs.Add(def);
          }
        }
      }
      Modified = false;
    }
  }
}
