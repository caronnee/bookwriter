using System;
using System.Windows;
using MyBook.Intro;

namespace MyBook
{
  /// <summary>
  /// Interaction logic for BookViewPort.xaml
  /// </summary>
  public partial class BookViewPort : Window
  {
    public BookViewPort()
    {
      InitializeComponent();
      ShowLibrary();
    }
    public void ShowLibrary()
    {
      Initial intro = new Initial();
      intro.LoadWriteBook += LoadWriteBook;
      intro.LoadReadBook += LoadReadBook;
      intro.NewBook += CreateBook;
      intro.OnSettingsPage += ShowSettingsBook;
      if (intro.Empty)
        CreateBook();
      else
        Content = intro;
    }

    public void CreateBook()
    {
      BookWrite write = new BookWrite("");
      write.Back += ShowLibrary;
      Content = write;
    }

    public void LoadReadBook(String str)
    {
      BookRead r = new BookRead(str);
      r.Back += ShowLibrary;
      Content = r;
    }

    public void LoadWriteBook(String str)
    {
      BookWrite workBook = new BookWrite(str);
      workBook.Back += ShowLibrary;
      Content = workBook;
      return;
    }

    public void ShowSettingsBook()
    {
      SettingPage page = new SettingPage();
      page.OnDone += ShowLibrary;
      Content = page;
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      BookWrite b = Content as BookWrite;
      if (b != null)
        b.Done();
    }
  }
}

