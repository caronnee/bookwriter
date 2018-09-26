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
      ShowShelf();
    }
    public void ShowShelf()
    {
      Initial intro = new Initial();
      intro.LoadBook += LoadWriteBook;
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
      write.Back += ShowShelf;
      Content = write;
    }

    public void LoadWriteBook(String str, int flags)
    {
      BookWrite workBook = new BookWrite(str);
      workBook.Back += ShowShelf;
      Content = workBook;
      return;
    }

    public void ShowSettingsBook()
    {
      SettingPage page = new SettingPage();
      page.OnDone += ShowShelf;
      Content = page;
    }
  }
}
