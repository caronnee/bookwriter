﻿using MyBook.Pages.Write.Riddle;
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

namespace MyBook.Pages.Write.Info
{
  /// <summary>
  /// Interaction logic for BookSettings.xaml
  /// </summary>
  public partial class BookInfo : UserControl
  {
    public BookInfo()
    {
      InitializeComponent();
      DataContext = new CoverParagraph();
    }

    public void Load(CoverParagraph cover)
    {
      DataContext = cover;
    }
  }
}