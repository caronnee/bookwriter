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
    
    public string Name => "Image";

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
    public int Order { get; set; }

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

    #region Serialization

    struct ImageSerializationData
    {
      public string filename;
      public string footer;
      public string header;
    };

    private void Serialize(Serializer.BaseSerializer serialize, ref ImageSerializationData d)
    {
      serialize.SerializeString(ImageNodeNames.Path, ref d.filename);
      serialize.SerializeString(ImageNodeNames.Footer, ref d.footer);
      serialize.SerializeString(ImageNodeNames.Header, ref d.header);
    }

    public bool Load(Serializer.BaseSerializer s)
    {
      ImageSerializationData d = new ImageSerializationData();
      Serialize(s, ref d);
      Data.ImagePath = BaseFolder + d.filename;
      Data.Footer = d.footer;
      Data.Header = d.header;
      Create();
      return true;
    }

    private void Save(Serializer.BaseSerializer s)
    {
      ImageSerializationData d = new ImageSerializationData();
      if ( Data.ImagePath.Length!=0)
      {
        d.filename = Data.ImagePath.Substring(BaseFolder.Length);
      }
      d.footer = Data.Footer;
      d.header = Data.Header;
      Serialize(s, ref d);
    }

    public void Serialize(Serializer.BaseSerializer s)
    {
      if (s.IsLoading)
        Load(s);
      else
        Save(s);
    }
    #endregion
  }
}
