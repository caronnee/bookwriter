using MyBook.BookContent;
using System.Windows.Controls;

namespace MyBook.Pages.Write.Text
{
  /// <summary>
  /// Interaction logic for WritingBox.xaml
  /// </summary>
  public partial class WritingBox : UserControl, IWrite
  {
    public WritingBox()
    {
      InitializeComponent();
    }

    public IContent Create()
    {
      /*if (Text.Length == 0)
        return null;
      TextParagraph par = new TextParagraph();
      par.Content = Text;
      return par;*/
      return null;
    }
  }
}
