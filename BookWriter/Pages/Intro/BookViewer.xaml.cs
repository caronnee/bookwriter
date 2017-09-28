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
using System.Windows.Shapes;

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
      Intro intro = new Intro();
      intro.LoadBook += LoadWriteBook;
      intro.NewBook += CreateBook;
      intro.OnSettingsPage += ShowSettingsBook;
      Content = intro;
    }

    public void CreateBook()
    {
      string name = "New Book";
      BookWrite write = new BookWrite(name);
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
