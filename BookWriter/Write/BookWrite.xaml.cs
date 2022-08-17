using MyBook.BookContent;
using MyBook.Meta;
using MyBook.Write;
using MyBook.Write.Character;
using MyBook.Write.Documents;
using MyBook.Write.GroupHandler;
using MyBook.Write.Model;
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
    private DocumentHandler _documentHandler;
    private ModelHandler _modelHandler;
    
    public BookWrite(String name)
    {
      // empty booksource
      Cache = new BookSource();
      DataContext = Cache;
      // TODO continue form the last time
      if (name.Length > 0)
        Cache.Load(name);
      _documentHandler = new DocumentHandler();
      _modelHandler = new ModelHandler();
      _worldHandler = new WorldHandler();
      _characterHolder = new CharacterHolder();
      _sceneHolder = new SceneHolder();
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
      FolderHandler.x_test.DataContext = Cache.World;
    }
    public void Done()
    {
      //BookSource s = DataContext as BookSource;
      //CurrentContent.Save();
      //s.Save();
    }

    private void x_bookViewer_Unloaded(object sender, RoutedEventArgs e)
    {
      Done();
    }

    class ModelConverter : IValueConverter
    {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        if (value == null)
          return null;
        List<PreviewFolder> fldr = new List<PreviewFolder>();
        List<ModelDescription> sc = value as List<ModelDescription>;
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

    class DocumentConverter : IValueConverter
    {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        if (value == null)
          return null;
        List<PreviewFolder> fldr = new List<PreviewFolder>();
        List<DocumentDescription> sc = value as List<DocumentDescription>;
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

    private void PreviewModelClick(object sender, RoutedEventArgs args)
    {
      ModelDescription m = (sender as Button).DataContext as ModelDescription;
      PreviewModel(m);
    }
    private void PreviewWorldClick(object sender, RoutedEventArgs args)
    {
      WorldDescription f = (sender as Button).DataContext as WorldDescription;
      PreviewWorld(f);
    }
    private void PreviewDocumentsClick(object sender, RoutedEventArgs args)
    {
      DocumentDescription f = (sender as Button).DataContext as DocumentDescription;
      PreviewDocument(f);
    }
    private void PreviewSceneClick(object sender, RoutedEventArgs args)
    {
      SceneDescription f = (sender as Button).DataContext as SceneDescription;
      PreviewScene(f);
    }
    private void PreviewModels()
    {
      Binding b = new Binding(".");
      // source data context
      //b.Source
      b.Converter = new ModelConverter();
      FolderHandler = new GroupHandlerItem();
      FolderHandler.DataContext = Cache.Models;
      x_working_page.Content = FolderHandler;
      FolderHandler.x_test.SetBinding(ItemsControl.ItemsSourceProperty, b);
      foreach (PreviewFolder folder in FolderHandler.x_test.Items)
      {
        folder.x_go.Click += PreviewModelClick;
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
      foreach (PreviewFolder folder in FolderHandler.x_test.Items)
      {
        folder.x_go.Click += PreviewCharacterClick;
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

    class WorldConverter : IValueConverter
    {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        if (value == null)
          return null;
        List<PreviewFolder> fldr = new List<PreviewFolder>();
        List<WorldDescription> sc = value as List<WorldDescription>;
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
    private void PreviewDocuments()
    {
      Binding b = new Binding(".");
      b.Converter = new DocumentConverter();
      FolderHandler = new GroupHandlerItem();
      FolderHandler.DataContext = Cache.Documents;
      x_working_page.Content = FolderHandler;
      FolderHandler.x_test.SetBinding(ItemsControl.ItemsSourceProperty, b);
      foreach (PreviewFolder folder in FolderHandler.x_test.Items)
      {
        folder.x_go.Click += PreviewDocumentsClick;
      }
    }
    private void PreviewWorld()
    {
      Binding b = new Binding(".");
      b.Converter = new WorldConverter();
      FolderHandler = new GroupHandlerItem();
      FolderHandler.DataContext = Cache.World;
      x_working_page.Content = FolderHandler;
      FolderHandler.x_test.SetBinding(ItemsControl.ItemsSourceProperty, b);
      foreach (PreviewFolder folder in FolderHandler.x_test.Items)
      {
        folder.x_go.Click += PreviewWorldClick;
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
      foreach (PreviewFolder folder in FolderHandler.x_test.Items)
      {
        folder.x_go.Click += PreviewSceneClick;
      }
    }
    private void PreviewModel(ModelDescription md)
    {
      _modelHandler.DataContext = md;
      x_working_page.Content = _modelHandler;
    }
    private void PreviewDocument(DocumentDescription c)
    {
      _documentHandler.DataContext = c;
      x_working_page.Content = _documentHandler;
    }

    
    private void PreviewWorld(WorldDescription c)
    {
      _worldHandler.DataContext = c;
      x_working_page.Content = _worldHandler;
    }
    private void PreviewWorkd(CharacterDescription c)
    {

    }
    private void PreviewCharacter(CharacterDescription c)
    {
      _characterHolder.DataContext = c;
      x_working_page.Content = _characterHolder;
      Cache.RefreshContent();
    }
    private void PreviewScene(SceneDescription i)
    {
      x_working_page.Content = _sceneHolder;
      Cache.SetScene(i);
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
      DocumentDescription d = it.SelectedItem as DocumentDescription;
      if (d != null)
      {
        PreviewDocument(d);
        return;
      }
      ModelDescription m = it.SelectedItem as ModelDescription;
      if ( m != null)
      {
        PreviewModel(m);
        return;
      }
      TreeViewItem parent = it.SelectedItem as TreeViewItem;
      System.Diagnostics.Debug.Assert(parent!=null);
      if (parent == x_models)
        PreviewModels();
      if (parent == x_characters)
        PreviewCharacters();
      if (parent == x_scenes)
        PreviewFolder();
      if (parent == x_world)
        PreviewWorld();
      if (parent == x_documents)
        PreviewDocuments();
    }
    private void add_model_click(object sender, RoutedEventArgs e)
    {
      Cache.CreateModel();
      x_models.Items.Refresh();
      FolderHandler.x_test.DataContext = null;
      FolderHandler.x_test.DataContext = Cache.Documents;
    }
    private void add_document_click(object sender, RoutedEventArgs e)
    {
      Cache.CreateDocument();
      x_documents.Items.Refresh();
      FolderHandler.x_test.DataContext = null;
      FolderHandler.x_test.DataContext = Cache.Documents;
    }
    private void add_world_click(object sender, RoutedEventArgs e)
    {
      Cache.CreateWorld();
      x_world.Items.Refresh();
      FolderHandler.x_test.DataContext = null;
      FolderHandler.x_test.DataContext = Cache.World;
    }
  }
}
