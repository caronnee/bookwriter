using MyBook.BookContent;
using RiddleInterface;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MyBook.Write
{
  /// <summary>
  /// Interaction logic for SceneHolder.xaml
  /// </summary>
  public partial class SceneHolder : UserControl
  {
    public SceneHolder()
    {
      InitializeComponent();
    }

    public delegate void OnSceneSavedDlg();
    public OnSceneSavedDlg OnSceneSaved;

    public delegate void ReportMsg(string message);
    public ReportMsg OnReport;

    private void saveScene(object sender, RoutedEventArgs e)
    {
      SavePage();
      //// changed number of added items
      //// select newly added scene
      if (OnSceneSaved != null)
        OnSceneSaved();
    }
    
    private void SavePage()
    {
    }

    private void startPage_Click(object sender, RoutedEventArgs e)
    {
      BookSource c = DataContext as BookSource;
      c.SetPage(0,0);
    }
    
    private void MoveBackClick(object sender, RoutedEventArgs e)
    {
      SavePage();
      BookSource s = DataContext as BookSource;
      s.MoveBack();
      OnReport("");
    }

    private void MoveForwardClick(object sender, RoutedEventArgs e)
    {
      SavePage();
      BookSource s = DataContext as BookSource;
      s.MoveForward();
      OnReport("");
    }

    public void Save()
    {
      BookSource s = DataContext as BookSource;
      System.Diagnostics.Debug.Assert(s != null);
      SavePage();
      s.Save();
      OnReport("Book saved");
    }

    private void CreateContentClick(object sender, RoutedEventArgs e)
    {
      BookSource s = DataContext as BookSource;
      AssemblyMap assembly = x_types.SelectedItem as AssemblyMap;
      s.CreatePage(assembly);
      OnReport("Page inserted");
    }
  }
}
