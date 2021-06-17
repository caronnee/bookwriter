using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace ImagePlugin
{
  public class ImageConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      String str = value as String;
      return new Uri(str);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }

  /// <summary>
  /// Interaction logic for ImageBox.xaml
  /// </summary>
  public partial class ImageBox : UserControl
  {
    public ImageBox()
    {
      InitializeComponent();
    }
  }
}
