using MyBook.BookContent;
using System;
using System.Collections.Generic;
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
  public partial class ImageSettings : UserControl, IWrite
  {
    public ImageSettings()
    {
      InitializeComponent();
      DataContext = new ImageParagraph();
    }

    private void loadImage_Click(object sender, RoutedEventArgs e)
    {
      // open dialog
      System.Windows.Forms.OpenFileDialog imageDialog = new System.Windows.Forms.OpenFileDialog();
      imageDialog.Filter = "Image files (*.png,*.jpg,*.bmp)|*.png;*.jpg;*.bmp";
      if (imageDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK )
        return;

      ImageParagraph im = DataContext as ImageParagraph;
      String str = "file:///" + imageDialog.FileName;
      Uri uri = new Uri(str);
      im.Source = new BitmapImage(uri);
    }

    public IContent Create()
    {
      return new ImageParagraph();
    }
  }
}
