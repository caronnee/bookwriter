using MyBook.BookContent;
using MyBook.Pages.Write.Picture;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyBook.Pages.Write.Imaging
{
  /// <summary>
  /// Interaction logic for ImageSettings.xaml
  /// </summary>
  public partial class ImageSettings : UserControl
  {
    public ImageSettings()
    {
      InitializeComponent();
      Reset();
    }

    public void Reset()
    {
      DataContext = new ImageBox();
    }
    
    private void loadImage_Click(object sender, RoutedEventArgs e)
    {
      // open dialog
      System.Windows.Forms.OpenFileDialog imageDialog = new System.Windows.Forms.OpenFileDialog();
      imageDialog.Title = "Choose image file";
      imageDialog.Filter = "Image files (*.png,*.jpg,*.bmp)|*.png;*.jpg;*.bmp";
      if (imageDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK )
        return;
      ImageBox im = DataContext as ImageBox;
      // TODO remove the previous image?
      string title = imageDialog.FileName;
      title = title.Substring(title.LastIndexOf("\\")+1);
      File.Copy(imageDialog.FileName, Settings.TempFolder + title,true);
      im.SetImage(title);
    }
  }
}
