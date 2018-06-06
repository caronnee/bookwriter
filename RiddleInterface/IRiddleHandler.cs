using System;
using System.Windows.Controls;
using System.Xml;

namespace RiddleInterface
{
  public interface IContent
  {
    XmlNode ToXmlNode(XmlDocument doc);
    bool Load(XmlNode node);
    UserControl ConvertToReadonly();
  }

  public delegate void OnSuccessAction();

  public interface IRiddleHandler
  {
    String Name { get; set; }
    UserControl Settings { get; set; }
    UserControl Viewport { get; set; }
    void Create();
    //void Load(IContent content);
    IContent CreateRiddle();
    IContent Load(XmlNode node);
  }
}
