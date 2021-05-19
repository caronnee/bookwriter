using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        x_image_path.Text = openFileDialog.FileName;
    }
  }
}
