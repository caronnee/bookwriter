using System.Collections.Generic;
using System.IO;
using RiddleInterface;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace ImagePlugin
{
  public class ImageCreator : IRiddleHandler
  {
    private ImageWrite _writer;
    private ImageBox _reader;
    public ImageData Data { get; set; }
    
    public override string Name => "Image";

    public override Control Settings => null;
    public override Control Viewport => _writer;
    public override Control DisplayPage => _reader;

    public ImageCreator()
    {
      ClearAnswer();
    }

    public override void ClearAnswer()
    {
      Data = new ImageData();
    }

    public override void Create()
    {
      _writer = new ImageWrite();
      _writer.DataContext = this;
    }

    public void CreateReadOnly()
    {
      _reader = new ImageBox();
      _reader.DataContext = this;
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
      if (Data.ImagePath.Length > BaseFolder.Length)
      {
        d.filename = Data.ImagePath.Substring(BaseFolder.Length);
      }
      d.footer = Data.Footer;
      d.header = Data.Header;
      Serialize(s, ref d);
    }

    public override void Serialize(Serializer.BaseSerializer s)
    {
      if (s.IsLoading)
        Load(s);
      else
        Save(s);
    }
    #endregion
  }
}
