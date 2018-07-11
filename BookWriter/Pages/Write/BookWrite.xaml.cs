using MyBook.BookContent;
using MyBook.Pages.Meta;
using MyBook.Pages.Write.Bookmark;
using MyBook.Pages.Write.Meta;
using MyBook.Pages.Write.Picture;
using MyBook.Pages.Write.Text;
using RiddleInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using static MyBook.BookContent.BookSource;

namespace MyBook
{
  /// <summary>
  /// Interaction logic for BookWrite.xaml
  /// </summary>
  public partial class BookWrite : UserControl
  {
    private BookSource Cache;

    private List<IRiddleHandler> InitPlugins()
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
        x_riddleSwitch.Items.Add(menu);
      }
      return riddles;
    }
    
    private void riddleChanged(object sender, RoutedEventArgs e)
    {
      MenuItem box = sender as MenuItem;
      (box.Parent as MenuItem).IsChecked = true;
    }

    public List<BookmarksHeader> Bookmarks;

    public List<IRiddleHandler> ContentHandlers;

    public BookWrite(String name)
    {
      //DataWriteContext data = new DataWriteContext();
      //data.Init(name);
      //DataContext = data;
      // initialize also all plugins
      // empty booksource
      Cache = new BookSource();
      DataContext = Cache;

      // TODO continue form the last time
      TextHandler th = new TextHandler();
      ImageHandler ih = new ImageHandler();

      ContentHandlers = new List<IRiddleHandler>();
      ContentHandlers.Add(th);
      ContentHandlers.Add(ih);

      // initialize UI
      InitializeComponent();
      x_insertText.DataContext = th;
      x_insertImage.DataContext = ih;
      ContentHandlers.AddRange(InitPlugins());

      if (name.Length > 0)
        Cache.Load(name, ContentHandlers);

      Show();
      ShowProgress("Book loaded");
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
      Cache.SaveScene(x_sceneName.Text);
      if (Cache.Name == null)
      {
        //
        SetBookName name = new SetBookName();
        bool? set = name.ShowDialog();
        if (name.x_bookName.Text.Length == 0)
        {
          ShowProgress("Book not saved");
          return;
        }
        Cache.Name = name.x_bookName.Text;
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
      Cache.Position.Clear();
      ShowProgress("Start page");
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
    private void SetHandler(IRiddleHandler handler)
    {
      System.Diagnostics.Debug.Assert(handler != null);
      actualHandler = handler;
      Control control = actualHandler.Settings;
      x_writeSettings.Child = actualHandler.Settings;
    }
    private void setViewboxContent(object sender, RoutedEventArgs e)
    {
      SavePage();
      MenuItem it = sender as MenuItem;
      // enable all
      HelperEnableMenu(x_contentMenu, it);
      it.IsEnabled = false;
      SetHandler(it.DataContext as IRiddleHandler);
      // TODO in regard of the font, this should ne be handled by handler
      PreparePage();
    }
    
    private void ShowProgress(String desc)
    {
      String str = String.Format("{0} ( Page {1}/{2} )", 
        desc,
        Cache.Position.ParagraphId+1, 
        Cache.Position.Scene.Pages.Count);
      x_progressText.Text = str;
    }
    
    private void PreparePage()
    {
      // create new page according to the handler
      actualHandler.Create();
      actualHandler.Viewport.DataContext = Cache;
      x_workingPage.Content = actualHandler.Viewport;      
    }

    // Show page fro the position
    private void Show()
    {
      IContent content = Cache.GetContent();
      if ( content == null )
      {
        SetHandler(ContentHandlers[0]);
        PreparePage();
        return;
      }
      // first find the handler - we have ensured that 
      foreach (IRiddleHandler h in ContentHandlers)
      {
        if ( h.ToViewport(content) )
        {
          SetHandler(h);
          x_workingPage.Content = actualHandler.Viewport;
          break;
        }
      }
    }

    private void createPage(object sender, RoutedEventArgs e)
    {
      SavePage();
      CreatePage();
    }

    private void moveBack(object sender, RoutedEventArgs e)
    {
      SavePage();
      Cache.MoveBack();
      Show();
      ShowProgress("At");
    }

    private void moveForward(object sender, RoutedEventArgs e)
    {
      SavePage();
      Cache.MoveForward();
      Show();
      ShowProgress("At");
    }

    private void removePage(object sender, RoutedEventArgs e)
    {
      Cache.RemovePage();
    }

    private void saveAndCreateScene(object sender, RoutedEventArgs e)
    {
      string name = x_sceneName.Text;
      Cache.SaveScene(name);
      Cache.CreateScene();
      Cache.Position.ParagraphId = 0;
      PreparePage();
      SceneDescription scene = Cache.Position.Scene;
      x_scenes.Items.Refresh();
      // changed number of added items
      // select newly added scenel
      object o = x_scenes.ItemContainerGenerator.ContainerFromItem(scene);
      TreeViewItem i = o as TreeViewItem;
      if (i != null)
        i.IsSelected = true;
    }

    private IRiddleHandler actualHandler { get; set; }

    private void CreatePage()
    {
      if (actualHandler == null)
      {
        ShowProgress("No content chosen");      
        return;
      }
      SavePage();

      PreparePage();
      Cache.CreatePage();
     
      ShowProgress("Page created");
    }

    private void SavePage()
    {
      if (actualHandler == null)
      {
        Cache.SetPage(null);
        return;
      } 
      // create another working page of the same type
      IContent content = actualHandler.CreateRiddle();
      Cache.SetPage(content);
      ShowProgress("Page saved");
    }

    private void showAboutClick(object sender, RoutedEventArgs e)
    {
      Window w = new AboutBox();
      w.Show();
    }

    private void x_scenes_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      string name = x_sceneName.Text;
      Cache.SaveScene(name);
      Cache.SetScene(x_scenes.SelectedValue as SceneDescription);
      Show();
      ShowProgress("At");
    }

    private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      //System.Diagnostics.Debug.Assert(false);
    }

    private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
    {
      //System.Diagnostics.Debug.Assert(false);
    }
  }
}
