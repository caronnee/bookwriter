using MyBook.BookContent;
using MyBook.Write.GroupHandler;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyBook.Write.Converter
{
  class WorldConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        return null;
      List<PreviewFolder> fldr = new List<PreviewFolder>();
      List<WorldDescription> sc = value as List<WorldDescription>;
      for (int i = 0; i < sc.Count; i++)
      {
        PreviewFolder f = new PreviewFolder();
        f.DataContext = sc[i];
        fldr.Add(f);
      }
      return fldr;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
