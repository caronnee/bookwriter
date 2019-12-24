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
    public Control Settings { get; set; }
    public Control Viewport { get; set; }
    public void Create()
    {
      Viewport = new PasswordWriteBox();
    }

    public IContent CreateRiddle()
    {
      ContentPassword p = new ContentPassword();
      PasswordWriteBox box = Viewport as PasswordWriteBox;
      p.description = box.x_description.Text;
      // all hints
      p.items = new System.Collections.Generic.List<ContentPassword.PassItem>();
      foreach(HintItem hi in box.x_hintPanel.Children)
      {
        ContentPassword.PassItem item = new ContentPassword.PassItem();
        item.hint = hi.x_showText.Text;
        if (hi.x_isCorrect.IsChecked == true)
          item.sceneId = hi.x_toSkip.SelectedIndex.ToString();
        item.regexp = hi.x_regexp.Text;
        p.items.Add(item);
      }
      p.questionText = box.x_question.Text;
      return p;
    }

    public bool CanLoad(XmlNode node)
    {
      if (node.Name != PasswordXmlNames.Name)
        return false;
      return true;
    }

    public IContent Load(XmlNode node)
    {
      ContentPassword p = new ContentPassword();
      if (p.Load(node))
        return p;
      return null;
    }

    public bool ToViewport(IContent content)
    {
      ContentPassword p = content as ContentPassword;
      if (p == null)
        return false;
      PasswordWriteBox box = new PasswordWriteBox();
      box.x_description.Text = p.description;
      box.x_question.Text = p.questionText;
      box.x_hintPanel.Children.Clear();
      foreach ( var a in p.items )
      {
        HintItem item = new HintItem();
        box.x_hintPanel.Children.Add(item);
        item.x_regexp.Text = a.regexp;
        item.x_showText.Text = a.hint;
        if (a.sceneId!=null)
        {
          item.x_toSkip.SelectedIndex = System.Int32.Parse(a.sceneId);
          item.x_isCorrect.IsChecked = true;
        }
      }
      Viewport = box;
      return true;
    }
  }
}
