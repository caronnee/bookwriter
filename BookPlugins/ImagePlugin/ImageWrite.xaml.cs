using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ImagePlugin
{
  /// <summary>
  /// Interaction logic for ImageWrite.xaml
  /// </summary>
  public partial class ImageWrite : UserControl
  {
    public ImageWrite()
    {
      InitializeComponent();
    }

    private void click_load_image(object sender, RoutedEventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      if (openFileDialog.ShowDialog() == true)
      {
        int l = openFileDialog.FileName.LastIndexOf('\\');
        string title = openFileDialog.FileName.Substring(l);

        ImageCreator c = (DataContext as ImageCreator);
        string full = c.BaseFolder + title;
        File.Copy(openFileDialog.FileName, full,true);
        c.Data.ImagePath = full;
        x_image_path.Text = full;
      }
    }
  }
}
