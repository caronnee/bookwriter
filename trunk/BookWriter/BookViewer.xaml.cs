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
      intro.LoadBook += LoadBook;
      intro.OnSettingsPage += ShowSettingsBook;
      Content = intro;
    }
    public void LoadBook(String str, int flags)
    {
      if (flags == 0)
      {
        BookRead read = new BookRead(str);
        if (read.IsValid())
          Content = read;
        return;
      }

      if (flags == 1)
      {
        BookWrite workBook = new BookWrite(str);
        Content = workBook;
        return;
      }
      MessageBox.Show("Book was not found! ( which is weird and probably a bug )");
    }

    public void ShowSettingsBook()
    {
      SettingPage page = new SettingPage();
      page.OnDone += ShowShelf;
      Content = page;
    }
  }
}
