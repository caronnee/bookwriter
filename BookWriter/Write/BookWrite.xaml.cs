using MyBook.BookContent;
using MyBook.Meta;
using MyBook.Write;
using MyBook.Write.Character;
using MyBook.Write.GroupHandler;
using MyBook.Write.World;
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
    private WorldHandler _worldHandler;

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
      if (name.Length > 0)
        Cache.Load(name);
      _worldHandler = new WorldHandler();
      _worldHandler.DataContext = Cache;
      _characterHolder = new CharacterHolder();
      _characterHolder.DataContext = Cache;
      _sceneHolder = new SceneHolder();
      _sceneHolder.DataContext = Cache;
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
      x_progressText.Text = desc;
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
      CharacterDescription d = Cache.CreateCharacter();
      x_characters.Items.Refresh();
      FolderHandler.x_test.DataContext = null;
      FolderHandler.x_test.DataContext = Cache.Characters;
    }

    private void AddWorld_Click(object sender, RoutedEventArgs e)
    {
      x_world.Items.Refresh();
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
        List<CharacterDescription> sc = value as List<CharacterDescription>;
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
    private void PreviewCharacterClick(object sender, RoutedEventArgs args)
    {
      CharacterDescription f = (sender as Button).DataContext as CharacterDescription;
      PreviewCharacter(f);
    }
    private void PreviewSceneClick(object sender, RoutedEventArgs args)
    {
      SceneDescription f = (sender as Button).DataContext as SceneDescription;
      PreviewScene(f);
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
      foreach (PreviewFolder folder in FolderHandler.x_test.Items)
      {
        folder.x_go.Click += PreviewCharacterClick;
      }
    }
    class testCon<T> : IValueConverter
    {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        throw new NotImplementedException();
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
        throw new NotImplementedException();
      }
    }
    class SceneConverter : IValueConverter
    {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        if (value == null)
          return null;
        List<PreviewFolder> fldr = new List<PreviewFolder>();
        List<SceneDescription> sc = value as List<SceneDescription>;
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
    private void PreviewWorld()
    {

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
      foreach (PreviewFolder folder in FolderHandler.x_test.Items)
      {
        folder.x_go.Click += PreviewSceneClick;
      }
    }

    private void PreviewWorld(WorldDescription c)
    {
      _worldHandler.DataContext = c;
      x_working_page.Content = _worldHandler;
    }
    private void PreviewCharacter(CharacterDescription c)
    {
      _characterHolder.DataContext = c;
      x_working_page.Content = _characterHolder;
      Cache.RefreshContent();
    }
    private void PreviewScene(SceneDescription i)
    {
      Cache.SetScene(i);
      x_working_page.Content = _sceneHolder;
      Cache.RefreshContent();
    }
    private void Item_Selected(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      TreeView it = sender as TreeView;
      SceneDescription i = it.SelectedItem as SceneDescription;
      if ( i!=null)
      {
        PreviewScene(i);
        return;
      }
      CharacterDescription c = it.SelectedItem as CharacterDescription;
      if ( c!= null )
      {
        PreviewCharacter(c);
        return;
      }
      WorldDescription w = it.SelectedItem as WorldDescription;
      if (w != null)
      {
        PreviewWorld(w);
        return;
      }
      TreeViewItem parent = it.SelectedItem as TreeViewItem;
      System.Diagnostics.Debug.Assert(parent!=null);
      if (parent == x_characters)
        PreviewCharacters();
      if (parent == x_scenes)
        PreviewFolder();
      if (parent == x_world)
        PreviewWorld();
    }

    private void add_world_click(object sender, RoutedEventArgs e)
    {
      Cache.CreateWorld();
      x_world.Items.Refresh();
    }
  }
}
