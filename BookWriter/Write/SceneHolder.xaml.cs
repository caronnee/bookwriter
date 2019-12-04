using MyBook.BookContent;
using MyBook.Write.Picture;
using MyBook.Write.Text;
using RiddleInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace MyBook.Write
{
  /// <summary>
  /// Interaction logic for SceneHolder.xaml
  /// </summary>
  public partial class SceneHolder : IGuiContent 
  {
    public SceneHolder()
    {
      InitializeComponent();
    }
    
    IRiddleHandler Handler { get; set; }
    
     // Show page fro the position
    public void LoadScene()
    {
      BookSource Cache = DataContext as BookSource;
      Handler = Cache.GetCurrentHandler();
      PreparePage();
    }

    public delegate void OnSceneSavedDlg();
    public OnSceneSavedDlg OnSceneSaved;

    private void saveScene(object sender, RoutedEventArgs e)
    {
      SavePage();
      //// changed number of added items
      //// select newly added scene
      if (OnSceneSaved != null)
        OnSceneSaved();
      //SelectionPickup();
      //x_scenes.Items.Refresh();
    }

    private void CreatePage()
    {
      if (Handler == null)
      {
        return;
      }

      SavePage();
      PreparePage();
    }

    private void SavePage()
    {
      BookSource Cache = DataContext as BookSource;
      if (Handler == null)
      {
        Cache.SetPage(null);
        return;
      }
      // create another working page of the same type
      IContent content = Handler.CreateRiddle();
      Cache.SetPage(content);
    }

    private void startPage_Click(object sender, RoutedEventArgs e)
    {
      BookSource c = DataContext as BookSource;
      c.Position.Clear();
    }

    private void PreparePage()
    {
      // create new page according to the handler
      Handler.Viewport.DataContext = DataContext;
      x_sceneContent.Content = Handler.Viewport;
    }

    private void MoveBackClick(object sender, RoutedEventArgs e)
    {
      SavePage();
      BookSource s = DataContext as BookSource;
      s.MoveBack();
      LoadScene();
    }

    private void MoveForwardClick(object sender, RoutedEventArgs e)
    {
      SavePage();
      BookSource s = DataContext as BookSource;
      s.MoveForward();
      LoadScene();
    }

    public override void Save()
    {
      BookSource s = DataContext as BookSource;
      System.Diagnostics.Debug.Assert(s != null);
      SavePage();
      s.Save();
    }
  }
}
