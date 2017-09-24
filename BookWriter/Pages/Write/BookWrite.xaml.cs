using MyBook.BookContent;
using MyBook.Pages.Write;
using MyBook.Pages.Write.Bookmark;
using MyBook.Pages.Write.DataWriteContext;
using MyBook.Pages.Write.Entity;
using MyBook.Pages.Write.Info;
using MyBook.Pages.Write.Picture;
using MyBook.Pages.Write.Riddle;
using MyBook.Pages.Write.Text;
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
using System.Xml;

namespace MyBook
{
  /// <summary>
  /// Interaction logic for BookWrite.xaml
  /// </summary>
  public partial class BookWrite : UserControl
  {
    public static readonly DependencyProperty TextSettingsProperty =
DependencyProperty.Register(
"TextSettingsControl", typeof(TextSettings), typeof(BookWrite));

    public TextSettings TextSettingsControl
    {
      get
      {
        return (TextSettings)GetValue(TextSettingsProperty);
      }
      set
      {
        SetValue(TextSettingsProperty, value);
      }
    }

    public static readonly DependencyProperty ImageSettingsProperty =
DependencyProperty.Register(
"ImageSettingsControl", typeof(ImageSettings), typeof(BookWrite));

    public ImageSettings ImageSettingsControl
    {
      get
      {
        return (ImageSettings)GetValue(ImageSettingsProperty);
      }
      set
      {
        SetValue(ImageSettingsProperty, value);
      }
    }

    public static readonly DependencyProperty RiddleSettingsProperty =
DependencyProperty.Register(
"RiddleSettingsControl", typeof(RiddleSettings), typeof(BookWrite));

    public RiddleSettings RiddleSettingsControl
    {
      get
      {
        return (RiddleSettings)GetValue(RiddleSettingsProperty);
      }
      set
      {
        SetValue(RiddleSettingsProperty, value);
      }
    }

    public static readonly DependencyProperty BookSettingsProperty =
DependencyProperty.Register(
"BookSettingsControl", typeof(BookInfo), typeof(BookWrite));

    public BookInfo BookSettingsControl
    {
      get
      {
        return (BookInfo)GetValue(BookSettingsProperty);
      }
      set
      {
        SetValue(BookSettingsProperty, value);
      }
    }

    public void Show()
    {
      IContent content = Cache.GetContent(workingPage.Position);
      workingPage.Child = content.Show(workingPage.Converter);
    }

    private BookSource Cache;

    public BookWrite(String name)
    {
      DataWriteContext data = new DataWriteContext();
      data.Init(name);
      DataContext = data;
      InitializeComponent();

      Cache = new BookSource(name);
      TextSettingsControl = new TextSettings();
      ImageSettingsControl = new ImageSettings();
      RiddleSettingsControl = new RiddleSettings();
      BookSettingsControl = new BookInfo();
      workingPage.Converter = new CacheToWriteControl();
      // TODO continue form the last time
      workingPage.Position.Clear();

      // new book will a;lways have as first thing writing box
      insertTextButton.IsChecked = true; 
    }

    private void SaveBook(object sender, RoutedEventArgs e)
    {
      // export to XML format. DTD
      //SourceText.UpdateCache();
      //SourceText.Cache.Save();
    }

    private void startPage_Click(object sender, RoutedEventArgs e)
    {
      //SourceText.Start();
      workingPage.Position.Clear();
      Show();
    }

    //public void MoveForward()
    //{
    //  workingPage.Position = rightPage.Position;
    //  rightPage.Position.ContentPos += rightPage.Position.Lenght;
    //  Show();
    //}

    //public void MoveBackward()
    //{
    //  rightPage.Position = workingPage.Position;
    //  workingPage.Position = leftPage.Position;
    //  leftPage.Position.ContentPos -= leftPage.Position.Lenght;
    //  Show();
    //}
   


    private void setViewboxContent(object sender, RoutedEventArgs e)
    {
      RadioButton b = sender as RadioButton;
      Control control = (Control)b.DataContext;
      // when this changes, child of the writing page must be changes also
      writeSettings.Child = control;
      // save the current child
      SavePage();
      IContent content = control.DataContext as IContent;
      workingPage.Child = content.Show(workingPage.Converter);
    }

    private void SetPageDoneClick(object sender, RoutedEventArgs e)
    {
      SavePage();
      Control c = writeSettings.Child as Control;
      IContent content = c.DataContext as IContent;
      workingPage.Child = content.Show(workingPage.Converter);
    }

    private void SavePage()
    {
      // create another working page of the same type
      IContent content = workingPage.Create();
      if (content == null)
        return;
      Cache.SetParagraph(workingPage.Position, content);
      workingPage.Position.ParagraphId++;
      String str = String.Format("Page saved ( Chapter {0}/{1}, Page {2}/{3} )", workingPage.Position.ChapterId, Cache.NChapters(), workingPage.Position.ParagraphId,Cache.NPages());
      progressText.Text = str;
    }

    private void showAboutClick(object sender, RoutedEventArgs e)
    {
      Window w = new AboutBox();
      w.Show();
    }
  }
}
