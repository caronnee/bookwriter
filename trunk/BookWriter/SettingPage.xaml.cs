using System;
using System.Collections.Generic;
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
  /// Interaction logic for SettingPage.xaml
  /// </summary>
  public partial class SettingPage : UserControl
  {
    public SettingPage()
    {
      InitializeComponent();
    }

    private void pickFolder_Click(object sender, RoutedEventArgs e)
    {
      // create a browse folder that allow pick a folder where books will be stored
      // Create OpenFileDialog
      Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
      dlg.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

      // Display OpenFileDialog by calling ShowDialog method
      Nullable<bool> result = dlg.ShowDialog();

      // Get the selected file name and display in a TextBox
      if (result == true)
      {
        // Open document
        string filename = dlg.FileName;
        TextBox box = FindName("bFolder") as TextBox;
        box.Text = filename;
      }
    }

    public delegate void DoneHandler();
    public event DoneHandler OnDone;

    private void SaveAndBack_Click(object sender, RoutedEventArgs e)
    {
      XmlDocument doc = new XmlDocument();
      string path = System.AppDomain.CurrentDomain.BaseDirectory;
      path += "setting.xml";
      
      XmlNode node = doc.CreateNode(XmlNodeType.Element, "Setting","set");
      XmlNode n2 = doc.CreateNode(XmlNodeType.Element, "Setting", "set");
      XmlNode text = doc.CreateNode(XmlNodeType.Text, "something", "set");
      TextBox textbox = FindName("bFolder") as TextBox;
      text.Value = textbox.Text;
      n2.AppendChild(text);
      node.AppendChild(n2);
      doc.AppendChild(node);
      doc.Save(path);

      if (OnDone != null)
        OnDone();
    }

    private void CancelAndBack_Click(object sender, RoutedEventArgs e)
    {
      if (OnDone != null)
        OnDone();
    }
  }
}
