using MyBook.BookContent;
using MyBook.Pages.Meta;
using MyBook.Pages.Write;
using MyBook.Pages.Write.DataWriteContext;
using MyBook.Pages.Write.Imaging;
using MyBook.Pages.Write.Meta;
using MyBook.Pages.Write.Picture;
using MyBook.Pages.Write.Riddle;
using MyBook.Pages.Write.Text;
using RiddleInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace MyBook
{
  /// <summary>
  /// Interaction logic for BookWrite.xaml
  /// </summary>
  public partial class BookWrite : UserControl, CacheToControlConverter
  {
    // what to do when type is tex
    public UIElement Resolve(TextParagraph textParagraph)
    {
      return insertText;
    }

    // What to do when type is image 
    public UIElement Resolve(ImageParagraph imagesParagraph)
    {
      return insertImage;
    }

    // Default resolve
    public UIElement Resolve(object o)
    {
      return null;
    }

    public void Show( String desc )
    {
      IContent content = Cache.GetContent(Position);
      if (content != null)
      {
        // find out which setting should eb shown
        MenuItem button = content.Show(this) as MenuItem;
        //workingPage.Child = content.Show(workingPage.Converter);
      }
      ShowProgress(desc);
    }

    private BookSource Cache;

    private void InitPlugins()
    {
      Assembly assem = Assembly.GetExecutingAssembly();
      Uri ur = new Uri(assem.CodeBase);
      FileInfo fi = new FileInfo(ur.AbsolutePath);
      string s = fi.Directory.FullName;

      //find all dlls
      string[] dlls = Directory.GetFiles(s + "\\Plugins", "*.dll");
      ICollection<Assembly> assemblies = new List<Assembly>(dlls.Length);
      List<IRiddleHandler> riddles = new List<IRiddleHandler>();
      foreach (string dllFile in dlls)
      {
        AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
        Assembly assembly = Assembly.Load(an);
        assemblies.Add(assembly);
      }

      Type rh = typeof(IRiddleHandler);

      foreach (Assembly a in assemblies)
      {
        Type[] types = a.GetTypes();
        foreach (Type t in types)
        {
          if (t.IsAbstract || t.IsNotPublic || !rh.IsAssignableFrom(t))
            continue;
          // create as instance
          object iRiddle = a.CreateInstance(t.ToString());
          IRiddleHandler riddle = iRiddle as IRiddleHandler;
          if (riddle != null)
            riddles.Add(riddle);
        }
      }

      foreach (IRiddleHandler r in riddles)
      {
        MenuItem menu = new MenuItem();
        menu.Header = r.Name;
        menu.DataContext = r;
        menu.Click += new RoutedEventHandler(riddleChanged);
        menu.Click += new RoutedEventHandler(setViewboxContent);
        riddleSwitch.Items.Add(menu);
      }
    }
    
    private void riddleChanged(object sender, RoutedEventArgs e)
    {
      MenuItem box = sender as MenuItem;
      (box.Parent as MenuItem).IsChecked = true;
     
      if (box.IsChecked == false)
        return;
      Control c = box.DataContext as Control;
      RiddleSettings rs = new RiddleSettings();
      rs.DataContext = this;
      rs.pluginControl.Children.Add(c);      
    }

    public BookWrite(String name)
    {
      DataWriteContext data = new DataWriteContext();
      data.Init(name);
      DataContext = data;
      InitializeComponent();

      // initialize also all plugins
      InitPlugins();
      // empty booksource
      Cache = new BookSource("");
     // workingPage.Converter = new CacheToWriteControl();
      insertText.DataContext = new TextHandler();
      insertImage.DataContext = new ImageHandler();
      // TODO continue form the last time
      Position = new PositionDesc();
      Position.Clear();

      // new book will always have as first thing writing box
      
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
      // SetCover, Set Name, save to the cache
      
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

    private void HelperEnableMenu(MenuItem parentMenu, MenuItem exc)
    {
      foreach (MenuItem menu in parentMenu.Items)
      {
        if (menu == exc)
          continue;
        menu.IsEnabled = true;
        menu.IsChecked = false;
        HelperEnableMenu(menu, exc);
      }
    }
    private void HelperEnableMenu(Menu parentMenu, MenuItem exc)
    {
      foreach (MenuItem menu in parentMenu.Items)
      {
        if (menu == exc)
          continue;
        menu.IsEnabled = true;
        menu.IsChecked = false;
        HelperEnableMenu(menu,exc);
      }
    }

    private void setViewboxContent(object sender, RoutedEventArgs e)
    {
      MenuItem it = sender as MenuItem;
      // enable all
      HelperEnableMenu(contentMenu, it);
      it.IsEnabled = false;
      IRiddleHandler handler = it.DataContext as IRiddleHandler;      
      System.Diagnostics.Debug.Assert(handler != null);
      handler.Create();
      Control control = handler.Settings;
      writeSettings.Child = handler.Settings;
      handler.Settings.Height = 200;
      // TODO in regard of the font, this should ne be handled by handler
      workingPage.Content = handler.Viewport;
      PreparePage();
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
      PreparePage();
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
          Position.ParagraphId--;
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

    private void PreparePage()
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
      PreparePage();
    }

    private bool SavePage()
    {
      // create another working page of the same type
      IContent content = null;// workingPage.Create();
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
