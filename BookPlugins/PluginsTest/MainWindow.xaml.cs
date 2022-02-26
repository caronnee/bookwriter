using RiddleInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;

namespace PluginsTest
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    BookTest _book;

    public MainWindow()
    {
      _book = new BookTest();
      Assembly assem = Assembly.GetExecutingAssembly();
      Uri ur = new Uri(assem.CodeBase);
      FileInfo fi = new FileInfo(ur.AbsolutePath);
      string s = fi.Directory.FullName;

      FileName = s + "\\test.gamebook";
      // all handlers that can be downloaded from dll
      InitializeComponent();
      RefreshAll();
    }
   
    private void ProcessAnswer(int id)
    {
      x_info.Text = $"Chosen outcome {id}";
    }

    private string FileName { get; set; }
    private void RefreshAll()
    {
      x_test.Items.Clear();
      for (int i = 0; i < _book.Handlers.Count; i++)
      {
        Tabcontainer cont = new Tabcontainer(_book.Handlers[i]);
        //Handlers[i].onAnswer += ProcessAnswer;
        x_test.Items.Add(cont);
      }
    }
    
    private void x_load_click(object sender, RoutedEventArgs e)
    {      
      _book.Load(FileName);
      MessageBox.Show($"Load of {FileName} successfull");
      RefreshAll();
    }
    
    private void SaveClick(object sender, RoutedEventArgs e)
    {
      _book.Save(FileName);
      MessageBox.Show($"Save succesfull to {FileName}");
    }
  }
}
