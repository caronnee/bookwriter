using System.Collections.Generic;
using System.IO;
using RiddleInterface;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace ImagePlugin
{
  public class ImageCreator : IRiddleHandler
  {
    public ImageData Data { get; set; }

    public OnSuccessAction onAnswer { get; set; }

    public string Name => "Image";

    public List<Outcome> Outcomes { get; set; }
    public Control Settings { get; set; }
    public Control Viewport { get; set; }
    public Control DisplayPage { get; set; }

    public ImageCreator()
    {
      Settings = null;
      ClearAnswer();
    }

    // folder for data. Not used for password. Yet
    public string BaseFolder { get; set; }

    public void ClearAnswer()
    {
      Data = new ImageData();
    }

    public void Create()
    {
      Viewport = new ImageWrite();
      Viewport.DataContext = this;
    }

    public void CreateReadOnly()
    {
      DisplayPage = new ImageBox();
      DisplayPage.DataContext = this;
    }

    public bool Load(IRiddleSerializer r)
    {
      string s = r.LoadParameter(ImageNodeNames.Id);
      if ( s != Name )
        return false;

      Data.Header = r.LoadSection(ImageNodeNames.Header);
      Data.Footer = r.LoadSection(ImageNodeNames.Footer);
      Data.ImagePath = BaseFolder + r.LoadSection(ImageNodeNames.Path);
      Create();
      return true;
    }

    public void Save(IRiddleSerializer r)
    {
      r.SaveParameter(ImageNodeNames.Id, Name);
      r.SaveValue(ImageNodeNames.Path, Data.ImagePath.Substring(BaseFolder.Length));
      r.SaveValue(ImageNodeNames.Header, Data.Header);
      r.SaveValue(ImageNodeNames.Footer, Data.Footer);
    }
  }
}
