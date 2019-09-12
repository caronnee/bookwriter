using System;
using System.Windows.Controls;
using System.Xml;

namespace RiddleInterface
{
  public interface IContent
  {
    bool IsLoaded();
    XmlNode ToXmlNode(XmlDocument doc);
    bool Load(XmlNode node);
    UserControl ConvertToReadonly();
  }

  public delegate void OnSuccessAction();

  public interface IRiddleHandler
  {
    // name of the content handler
    String Name { get; set; }

    // setting assotiated with the content handler
    UserControl Settings { get; set; }

    // showble content
    UserControl Viewport { get; set; }

    // create empty viewport to ve filled / converted to content
    void Create();

    // create riddle from what was in the viewport
    IContent CreateRiddle();

    // create content from stream
    IContent Load(XmlNode node);

    // Load the content into the viewport
    bool ToViewport(IContent content);
  }
}
