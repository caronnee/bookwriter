using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace MyBook
{
  class HalfConverter : IValueConverter
  {
    public object Convert(object value, Type targetType,
    object parameter, CultureInfo culture)
    {
      Double val = (Double)value;
      return val / 2;
    }

    public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
    {
      Double d = (Double)value;
      return d * 2;
    }
  }
}
