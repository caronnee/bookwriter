using MyBook.BookContent;
using MyBook.Meta;
using MyBook.Write;
using MyBook.Write.Bookmark;
using MyBook.Write.Character;
using MyBook.Write.Timeline;
using RiddleInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MyBook
{
  /// <summary>
  /// Interaction logic for BookWrite.xaml
  /// </summary>
  public partial class BookWrite : UserControl, INotifyPropertyChanged
  {
    // loaded data
    private BookSource Cache;

    public List<BookmarksHeader> Bookmarks;

    private SceneHolder _sceneHolder;
    private CharacterHolder _characterHolder;
    
    private void SceneSaved()
    {
      SelectionPickup();
      x_scenes.Items.Refresh();
      ShowProgress("Scene saved");
    }

    public BookWrite(String name)
    {
      // empty booksource
      Cache = new BookSource();
      DataContext = Cache;
      // TODO continue form the last time
      if ( name.Length != 0)
        Cache.Load(name);
      _characterHolder = new CharacterHolder();
      _sceneHolder = new SceneHolder();
      _sceneHolder.OnSceneSaved += SceneSaved;
      _sceneHolder.OnReport += ShowProgress;
      // initialize UI
      InitializeComponent();
      x_working_page.Content = _sceneHolder;
      SelectionPickup();
      ShowProgress("Book loaded");
    }

    private void ShowProgress(String desc)
    {
      BookSource c = DataContext as BookSource;
      String str = String.Format("{0} ( Page {1}/{2} )",
        desc,
        c.Position.ParagraphId + 1,
        c.Position.Scene.Pages.Count);
      x_progressText.Text = str;
    }
    // back to main menu
    public delegate void BackHandler();
    public event BackHandler Back;

    private void Back_Click(object sender, RoutedEventArgs e)
    {
      SaveBook();
      if (Back != null)
        Back();
    }

    // saves whole data
    private void SaveBook()
    {
      //CurrentContent.Save();
      if (Cache.Name == null)
      {
        //
        SetBookName name = new SetBookName();
        bool? set = name.ShowDialog();
        if (set == null || set == false)
        {
          ShowProgress("Book not saved");
          return;
        }
        Cache.Name = name.x_bookName.Text;
      }
      Cache.Save();
      ShowProgress("Book saved");
    }

    // settings of the book
    private void Settings_Click(object sender, RoutedEventArgs e)
    {
      MetaData metadata = new MetaData();
      // metadata.DataContext = Cache.Metadata.clone();
      metadata.ShowDialog();
      // SetCover, Set Name, save to the cache
    }

    // saves whole data handler
    private void SaveBook_Click(object sender, RoutedEventArgs e)
    {
      SaveBook();
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
        HelperEnableMenu(menu, exc);
      }
    }

    // create timeline show
    private void ShowTimeline()
    {
      Timeline t = new Timeline(Cache.Scenes);
      t.OnFinished += SelectionPickup;
      // todo change current content to timeline. Save not implemented
    }

    public event PropertyChangedEventHandler PropertyChanged;
    void NotifyPropertyChanged(string property)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(property));
    }

    private void SelectionPickup()
    {
      //x_scenes_holder.IsExpanded = true;
      //object o = x_scenes.ItemContainerGenerator.ContainerFromItem(Cache.Position.Scene);
      //TreeViewItem i = o as TreeViewItem;
      //if (i != null)
      //  i.IsSelected = true;
      //CurrentContent = _sceneHolder;
     // _sceneHolder.LoadScene();
    }

    private void showAboutClick(object sender, RoutedEventArgs e)
    {
      Window w = new AboutBox();
      w.Show();
    }

    private void x_characters_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      //CurrentContent.Save();
      //CurrentContent = _characterHolder;
      //_characterHolder.Load(x_characters.SelectedValue as CharacterContent);
    }

    private void x_scenes_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      //if (CurrentContent == null)
      //  return;
      //CurrentContent.Save();
      //Cache.SetScene(x_scenes.SelectedValue as SceneDescription);
      SelectionPickup();
    }

    private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      //System.Diagnostics.Debug.Assert(false);
    }

    private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
    {
      //System.Diagnostics.Debug.Assert(false);
    }

    private void SetTimeline(object sender, RoutedEventArgs e)
    {
      ShowTimeline();
    }
    
    private void AddScene_Click(object sender, RoutedEventArgs e)
    {
      Cache.CreateScene();
    }
    private void AddCharacter_Click(object sender, RoutedEventArgs e)
    {
      _characterHolder.Load(Cache.CreateCharacter());
      //CurrentContent = _characterHolder;
      //x_characters.Items.Refresh();
    }
    
    public void Done()
    {
      BookSource s = DataContext as BookSource;
      //CurrentContent.Save();
      s.Save();
    }

    private void x_bookViewer_Unloaded(object sender, RoutedEventArgs e)
    {
      Done();
    }
  }
}
