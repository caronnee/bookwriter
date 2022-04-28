using MyBook.BookContent;
using MyBook.Meta;
using MyBook.Write;
using MyBook.Write.Character;
using MyBook.Write.GroupHandler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MyBook
{
  /// <summary>
  /// Interaction logic for BookWrite.xaml
  /// </summary>
  public partial class BookWrite : UserControl, INotifyPropertyChanged
  {
    // loaded data
    private BookSource Cache;

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
      if(name.Length > 0)
        Cache.Load(name);
      _characterHolder = new CharacterHolder();
      _characterHolder.DataContext = Cache;
      _sceneHolder = new SceneHolder();
      _sceneHolder.OnSceneSaved += SceneSaved;
      _sceneHolder.OnReport += ShowProgress;
      FolderHandler = new GroupHandlerItem();

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

      if (Back != null)
        Back();
    }

    // saves whole data
    private void SetName()
    {
      //CurrentContent.Save();
      if (Cache.Name == null)
      {
        //
        SetBookName name = new SetBookName();
        name.ShowDialog();
        if (name.x_bookName.Text.Length > 0)
        {
          Cache.Name = name.x_bookName.Text;
        }
      }
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
      if (Cache.Name == null)
      {
        SetName();
      }
      Cache.Save();
      ShowProgress("Book saved");
    }
    private void SaveBookName_Click(object sender, RoutedEventArgs e)
    {
      SetName();
      Cache.Save();
      ShowProgress("Book saved");
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

    private GroupHandlerItem FolderHandler;
    private void AddScene_Click(object sender, RoutedEventArgs e)
    {
      Cache.CreateScene();
      x_scenes.Items.Refresh();
      FolderHandler.x_test.DataContext = null;
      FolderHandler.x_test.DataContext = Cache.Scenes;
    }
    private void AddCharacter_Click(object sender, RoutedEventArgs e)
    {
      _characterHolder.Load(Cache.CreateCharacter());
      x_characters.Items.Refresh();
      FolderHandler.x_test.DataContext = null;
      FolderHandler.x_test.DataContext = Cache.Characters;
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

    class CharacterConverter : IValueConverter
    {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        if (value == null)
          return null;
        List<PreviewFolder> fldr = new List<PreviewFolder>();
        List<BookSource.CharacterContent> sc = value as List<BookSource.CharacterContent>;
        if (sc == null)
          return null;
        for (int i = 0; i < sc.Count; i++)
        {
          PreviewFolder f = new PreviewFolder();
          f.DataContext = sc[i];
          fldr.Add(f);
        }
        return fldr;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
        throw new NotImplementedException();
      }
    }
    private void PreviewCharacters()
    {
      Binding b = new Binding(".");
      // source data context
      //b.Source
      b.Converter = new CharacterConverter();
      FolderHandler = new GroupHandlerItem();
      FolderHandler.DataContext = Cache.Characters;
      x_working_page.Content = FolderHandler;
      FolderHandler.x_test.SetBinding(ItemsControl.ItemsSourceProperty, b);
    }
    class SceneConverter : IValueConverter
    {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        if (value == null)
          return null;
        List<PreviewFolder> fldr = new List<PreviewFolder>();
        List<BookSource.SceneDescription> sc = value as List<BookSource.SceneDescription>;
        for (int i =0; i < sc.Count; i++)
        {
          PreviewFolder f = new PreviewFolder();
          f.DataContext = sc[i];
          fldr.Add(f);
        }
        return fldr;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
        throw new NotImplementedException();
      }
    }
    private void PreviewFolder()
    {
      Binding b = new Binding(".");
      // source data context
      //b.Source
      b.Converter = new SceneConverter();
      FolderHandler = new GroupHandlerItem();
      FolderHandler.DataContext = Cache.Scenes;
      x_working_page.Content = FolderHandler;
      FolderHandler.x_test.SetBinding(ItemsControl.ItemsSourceProperty, b);
    }

    private void Item_Selected(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      TreeView it = sender as TreeView;
      BookSource.SceneDescription i = it.SelectedItem as BookSource.SceneDescription;
      if ( i!=null)
      {
        Cache.SetScene(i);
        x_working_page.Content = _sceneHolder;
        return;
      }
      BookSource.CharacterContent c = it.SelectedItem as BookSource.CharacterContent;
      if ( c!= null )
      {
        x_working_page.Content = _characterHolder;
        return;
      }
      TreeViewItem parent = it.SelectedItem as TreeViewItem;
      System.Diagnostics.Debug.Assert(parent!=null);
      if (parent == x_characters)
        PreviewCharacters();
      if (parent == x_scenes)
        PreviewFolder();
    }
  }
}
