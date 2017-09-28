using MyBook.BookContent;
using MyBook.Pages.Intro;
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

namespace MyBook
{
  /// <summary>
  /// Interaction logic for Intro.xaml
  /// </summary>
  public partial class Intro : UserControl
  {
    private String GetTitle(String fullpath)
    {
      String ext = fullpath;
      int start = ext.LastIndexOf('\\');
      int end = ext.LastIndexOf('.');
      return ext.Substring(start, end - start);
    }
    private Control CreateBookControl(string name)
    {
      BookItem b = new BookItem();
      //Style s = TryFindResource("Rotated") as Style;
      b.Filename = name;
      //b.Style = s;
      b.buttonLoader.Content = GetTitle(name);
      b.buttonLoader.Click += new RoutedEventHandler(Load);
      return b;
    }

    private Control CreateNewBookControl()
    {
      BookItem b = new BookItem();
      //Style s = TryFindResource("Rotated") as Style;
      b.Filename = "New Book";
      //b.Style = s;
      b.buttonLoader.Content = "New book";     
      b.buttonLoader.Click += new RoutedEventHandler(CreateNew);
      return b;
    }
    
    public Intro()
    {
      InitializeComponent();
      String folder = Settings.BooksFolder;
      try
      {
        string[] strings = Directory.GetFiles(folder, Constants.SearchExt);
        foreach (string name in strings)
        {
          Control b = CreateBookControl(name);
          this.Shelf.Children.Add(b);
        }
      }
      catch (Exception)
      {
      }
      Control createButton = CreateNewBookControl();
      this.Shelf.Children.Add(createButton);
    }

    public delegate void LoadHandler(String str, int flags);
    public event LoadHandler LoadBook;

    public delegate void NewBookHandler();
    public event NewBookHandler NewBook;

    public delegate void SettingsPage();
    public event SettingsPage OnSettingsPage;

    private void CreateNew(object sender, RoutedEventArgs e)
    {
      BookItem ctrl = sender as BookItem;
      int typeFlags = writeButton.IsChecked == true ? 1 : 0;
      NewBook(); 
    }

    private void Load(object sender, RoutedEventArgs e)
    {
      if (LoadBook != null)
      {
        BookItem ctrl = sender as BookItem;
        int typeFlags = writeButton.IsChecked == true ? 1 : 0;
        LoadBook(ctrl.Filename, typeFlags); // name of the book
      }
    }

    private void settingButton_Click(object sender, RoutedEventArgs e)
    {
      if (OnSettingsPage == null)
        return;

      BookItem ctrl = sender as BookItem;
      int typeFlags = writeButton.IsChecked == true ? 1 : 0;
      OnSettingsPage(); // name of the book
    }
  }
}
