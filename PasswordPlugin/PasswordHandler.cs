using RiddleInterface;
using System.Windows.Controls;
using System.Xml;

namespace PasswordPlugin
{
  public class PasswordHandler : IRiddleHandler
  {
    public PasswordHandler()
    {
      Name = "PasswordPlugin";
      // setting are the same for the book
      Settings = new PasswordSetting();
    }

    public string Name { get; set; }
    public UserControl Settings { get; set; }
    public UserControl Viewport { get; set; }
    
    public IContent CreateContent()
    {
      return null;
    }
    public void Create()
    {
      Viewport = new PasswordWriteBox();
    }

    public IContent CreateRiddle()
    {
      PasswordParagraph p = new PasswordParagraph();
      PasswordWriteBox box = Viewport as PasswordWriteBox;
      p.description = box.x_description.Text;
      // all hints
      p.items = new System.Collections.Generic.List<PasswordParagraph.PassItem>();
      foreach(HintItem hi in box.x_hintPanel.Children)
      {
        PasswordParagraph.PassItem item = new PasswordParagraph.PassItem();
        item.hint = hi.x_showText.Text;
        if (hi.x_isCorrect.IsChecked == true)
          item.pagedesc = hi.x_toSkip.SelectedValue.ToString();
        item.regexp = hi.x_regexp.Text;
        p.items.Add(item);
      }
      p.questionText = box.x_question.Text;
      return p;
    }

    public IContent Load(XmlNode node)
    {
      PasswordParagraph p = new PasswordParagraph();
      if (p.Load(node))
        return p;
      return null;
    }
  }
}
