﻿using MyBook.BookContent;
using MyBook.Write.Imaging;
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

namespace MyBook.Write.Picture
{
  /// <summary>
  /// Interaction logic for ImageBox.xaml
  /// </summary>
  public partial class ImageBox : UserControl
  {
    public ImageBox()
    {
      InitializeComponent();
      SetImage("pack://application:,,,/Resources/workinprogress.png");
    }

    String SourceName;

    public void SetImage(String name)
    {
      String str;
      if ( name.StartsWith("pack:") )
        str = name;
      else
        str = "file:///" + Settings.TempFolder + name;
      SourceName = name;
      Uri uri = new Uri(str);
      x_picture.Source = new BitmapImage(uri);
    }
  }
}
