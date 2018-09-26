using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace MyBook.BookContent
{
  class WritingConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      double val = (double)value;
      val *= Settings.TextSizeCoef;
      return val / 3;
    }

    public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
    {
      double d = (double)value;
      return d * 3 / Settings.TextSizeCoef;
    }
  }
}
