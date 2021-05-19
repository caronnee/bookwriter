using MyBook.BookContent;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MyBook.Intro
{
  /// <summary>
  /// Interaction logic for Intro.xaml
  /// </summary>
  public partial class Initial : UserControl
  {
    private String GetTitle(String fullpath)
    {
      String ext = fullpath;
      int start = ext.LastIndexOf('\\')+1;
      int end = ext.LastIndexOf('.');
      return ext.Substring(start, end - start);
    }

    private Control CreateBookControl(string name)
    {
      BookItem b = new BookItem();
      //Style s = TryFindResource("Rotated") as Style;
      b.x_bookname.Content = GetTitle( name );
      b.x_open_for_writing.DataContext = name;
      b.x_open_for_writing.Click += new RoutedEventHandler(Write);
      return b;
    }

    private Control CreateNewBookControl()
    {
      Button b = new Button();
      //Style s = TryFindResource("Rotated") as Style;
      
      //b.Style = s;
      b.Content = "New book";     
      b.Click += new RoutedEventHandler(CreateNew);
      return b;
    }
    
    public bool Empty { get; private set; }

    public Initial()
    {
      Empty = true;
      InitializeComponent();
      String folder = Settings.BooksFolder;
      try
      {
        string[] strings = Directory.GetFiles(folder, Constants.SearchExt);
        foreach (string name in strings)
        {
          Control b = CreateBookControl(name);
          this.x_shelf.Children.Add(b);
          Empty = false;
        }
      }
      catch (Exception)
      {
        // do nothing
      }
      Control createButton = CreateNewBookControl();
      this.x_shelf.Children.Add(createButton);
    }

    public delegate void LoadHandler(String str);
    public event LoadHandler LoadWriteBook;

    public delegate void NewBookHandler();
    public event NewBookHandler NewBook;

    public delegate void SettingsPage();
    public event SettingsPage OnSettingsPage;

    private void CreateNew(object sender, RoutedEventArgs e)
    {
      BookItem ctrl = sender as BookItem;
      NewBook(); 
    }
    

    private void Write(object sender, RoutedEventArgs e)
    {
      if (LoadWriteBook != null)
      {
        Button ctrl = sender as Button;
        String filename = ctrl.DataContext as String;
        LoadWriteBook(filename); // name of the book
      }
    }

    private void settingButton_Click(object sender, RoutedEventArgs e)
    {
      if (OnSettingsPage == null)
        return;

      BookItem ctrl = sender as BookItem;
      OnSettingsPage(); // name of the book
    }
  }
}
