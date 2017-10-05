using MyBook.BookContent;
using MyBook.Pages.Meta;
using MyBook.Pages.Write;
using MyBook.Pages.Write.Bookmark;
using MyBook.Pages.Write.DataWriteContext;
using MyBook.Pages.Write.Imaging;
using MyBook.Pages.Write.Meta;
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

    public void Show( String desc )
    {
      IContent content = Cache.GetContent(Position);
      if (content == null)
      {
        Control x = writeSettings.Child as Control;
        content = x.DataContext as IContent;
      }

      workingPage.Child = content.Show(workingPage.Converter);
      ShowProgress(desc);
    }

    private BookSource Cache;

    public BookWrite(String name)
    {
      DataWriteContext data = new DataWriteContext();
      data.Init(name);
      DataContext = data;
      InitializeComponent();

      // empty booksource
      Cache = new BookSource("");
      TextSettingsControl = new TextSettings();
      ImageSettingsControl = new ImageSettings();
      RiddleSettingsControl = new RiddleSettings();
      workingPage.Converter = new CacheToWriteControl();
      // TODO continue form the last time
      Position = new PositionDesc();
      Position.Clear();

      // new book will always have as first thing writing box
      insertTextButton.IsChecked = true;
      if (name.Length > 0)
        Cache.Load(name);
      Show("At");
    }

    public delegate void BackHandler();
    public event BackHandler Back;

    private void Back_Click(object sender, RoutedEventArgs e)
    {
      SaveBook();
      if (Back != null)
        Back();
    }

    private void SaveBook()
    {
      SavePage();
      Cache.SaveChapter();
      if (Cache.Name == null)
      {
        //
        SetBookName name = new SetBookName();
        bool? set = name.ShowDialog();
        if (name.BookName.Text.Length == 0)
        {
          ShowProgress("Book not saved");
          return;
        }
        Cache.Name = name.BookName.Text;
      }
      Cache.Save();
      // export to XML format. DTD
      ShowProgress("Book saved");
    }

    private void Settings_Click(object sender, RoutedEventArgs e)
    {
      MetaData metadata = new MetaData();
      // metadata.DataContext = Cache.Metadata.clone();
      metadata.ShowDialog();
      // SetCover, Set Name
      // Cache.Metadata =  metadata.DataContext as Metadata;
    } 
    private void SaveBook_Click(object sender, RoutedEventArgs e)
    {
      SaveBook();
    }

    private void startPage_Click(object sender, RoutedEventArgs e)
    {
      Position.Clear();
      Show("Start page");
    }

    public PositionDesc Position
    {
      get;
      set;
    }

    private void setViewboxContent(object sender, RoutedEventArgs e)
    {
      // if sender 

      RadioButton b = sender as RadioButton;
      Control control = (Control)b.DataContext;
      // when this changes, child of the writing page must be changes also
      writeSettings.Child = control;

      CreateNewPage();
    }

    private void ShowProgress(String desc)
    {
      String str = String.Format("{4} ( Chapter {0}/{1}, Page {2}/{3} )", 
        Position.ChapterId + 1, 
        Cache.NChapters(), 
        Position.ParagraphId+1, 
        Cache.Paragraphs.Count, 
        desc );
      progressText.Text = str;
    }

    private void NewChapterClick(object sender, RoutedEventArgs e)
    {
      SavePage();
      Position.ChapterId = Cache.InsertChapter(Position.ChapterId);
      Position.ParagraphId = 0;
      workingPage.Child = null;
      CreateNewPage();
      Show("Chapter created");
    }

    private void StartPageClick(object sender, RoutedEventArgs e)
    {
      SavePage();
      Position.Clear();
      Show("At:");
    }

    private void LastChapterClick(object sender, RoutedEventArgs e)
    {
      SavePage();
      int chapterID =Cache.Chapters.Count - 1;
      Position.ParagraphId = 0;
      Position.ChapterId = Cache.Chapters.Count - 1;
      Show("At:");      
    }

    private void PreviousChapterClick(object sender, RoutedEventArgs e)
    {
      SavePage();
      Position.ParagraphId = 0;
      if (Position.ChapterId > 0)
        Position.ChapterId--;
      Show("At:");      
    }

    private void PreviousPageClick(object sender, RoutedEventArgs e)
    {
      SavePage();
      Position.ParagraphId--;
      if (Position.ParagraphId == 0)
      {
        if (Position.ChapterId > 0)
        {
          Position.ChapterId--;
          Cache.Load(Position.ChapterId);
          Position.ParagraphId = Cache.Paragraphs.Count - 1;
        }
        else
        {
          Position.Clear();
        }
      }
      Show("At:");      
    }

    private void NextPageClick(object sender, RoutedEventArgs e)
    {
      if (SavePage())
        Position.ParagraphId++;
      if (Position.ParagraphId == Cache.Paragraphs.Count)
      {
        if (Position.ChapterId < (Cache.Chapters.Count - 1))
        {
          Position.ChapterId++;
          Position.ParagraphId = 0;
        }
        else
        {
          Position.Clear();
        }
      }
      Show("At:");      
    }

    private void NextChapterClick(object sender, RoutedEventArgs e)
    {
      SavePage();
      Position.ParagraphId = 0;
      if (Position.ChapterId < (Cache.Chapters.Count - 1))
        Position.ChapterId++;
      Show("At:");      
    }

    private void CreateNewPage()
    {
      // save the current child
      if (SavePage())
      {
        Position.ParagraphId++;
      }
       
      // setEmpty
      Cache.InsertParagraph(Position.ParagraphId, null);
      
      Show("At");
    }

    private void SetPageDoneClick(object sender, RoutedEventArgs e)
    {
      CreateNewPage();
    }

    private bool SavePage()
    {
      // create another working page of the same type
      IContent content = workingPage.Create();
      if (content == null)
      {
        if (Cache.Paragraphs.Count > Position.ParagraphId 
          && Cache.Paragraphs[Position.ParagraphId] == null)
        {
          Cache.Paragraphs.RemoveAt(Position.ParagraphId);
        }
        return false;
      }
      Cache.SetParagraph(Position, content);
      ShowProgress("Page saved");
      return true;
    }

    private void showAboutClick(object sender, RoutedEventArgs e)
    {
      Window w = new AboutBox();
      w.Show();
    }
  }
}
