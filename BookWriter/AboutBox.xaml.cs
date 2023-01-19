using System;
using System.Windows;
using System.Windows.Documents;

namespace MyBook
{
  /// <summary>
  /// Interaction logic for Window1.xaml
  /// </summary>
  public partial class AboutBox : Window
  {
    public AboutBox()
    {
      InitializeComponent();
    }

    private void Hyperlink_Click(object sender, RoutedEventArgs e)
    {
      // show possiblieties for linking
      // character -> episodes
      // scene -> page
      // world -> episode
      // model
    }

    private void button1_Click(object sender, RoutedEventArgs e)
    {
      //RichTextBox1.Selection.Start.GetLineStartPosition(0).InsertParagraphBreak();
      Paragraph p = RichTextBox1.Selection.Start.Paragraph;
      TextRange tr = new TextRange(RichTextBox1.Selection.Start, RichTextBox1.Selection.End);
      Hyperlink hlink = new Hyperlink(tr.Start, tr.End);
      hlink.NavigateUri = new Uri("http://www.google.com/");
      hlink.Click += Hyperlink_Click;

      //Hyperlink h = new Hyperlink(RichTextBox1.Selection.Start, RichTextBox1.Selection.End);
      //h.Click += Hyperlink_Click;
      //RichTextBox1.Selection.Start.GetInsertionPosition(LogicalDirection.Forward).InsertTextInRun("xx_xxx_xx");
      //TextPointer tp = RichTextBox1.Selection.Start;
      ////tp.inli
      //p.Inlines.Add(h);
      // RichTextBox1.Selection.rep
      //p.Inlines.Add(new Run());
      //Paragraph pp = new Paragraph(new Run("kgtkuygk lg y <Bold>Go ahead.</Bold> k"));
      //pp.Inlines.Add(new Bold(new Run("kwakwa")));
      //pp.Inlines.Add(new Run("  sjhfge lfhj"));
      //h = new Hyperlink(new Run("second click"));
      //pp.Inlines.Add(h);
      //x_flow.Blocks.Add(pp);

      //Text = "<Hyperlink Click=\"Hyperlink_Click\">click me again</Hyperlink>";
    }
  }
}
