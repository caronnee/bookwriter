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
using System.IO;

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

    private void apply_Click(object sender, RoutedEventArgs e)
    {
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

    private string SETTINGS_FILENAME = System.AppDomain.CurrentDomain.BaseDirectory + "setting.xml";
    private string ROOT_NODE = "root";
    private string FOLDER_NODE = "folder";
    private string URI = "set";
    private string DEF_VALUE = "./Books";

    private void SaveAndBack_Click(object sender, RoutedEventArgs e)
    {
      XmlDocument doc = new XmlDocument();

      XmlNode node = doc.CreateNode(XmlNodeType.Element, ROOT_NODE, URI );
      XmlNode n2 = doc.CreateNode(XmlNodeType.Element, FOLDER_NODE, URI);
      XmlNode text = doc.CreateNode(XmlNodeType.Text, DEF_VALUE, URI);
      TextBox textbox = FindName("bFolder") as TextBox;
      text.Value = textbox.Text;
      n2.AppendChild(text);
      node.AppendChild(n2);
      doc.AppendChild(node);
      doc.Save(SETTINGS_FILENAME);

      if (OnDone != null)
        OnDone();
    }

    private void CancelAndBack_Click(object sender, RoutedEventArgs e)
    {
      if (OnDone != null)
        OnDone();
    }

    static public string Folder
    {
        get;
        set;
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        // if there is setting file, load from it
        if (!File.Exists(SETTINGS_FILENAME))
            return;
        XmlDocument doc = new XmlDocument();
        doc.Load(SETTINGS_FILENAME);
        XmlNode no = doc.FirstChild;
        XmlNodeList list = no.SelectNodes( FOLDER_NODE);
        string str = "/" + ROOT_NODE; 
        XmlNodeList root = doc.SelectNodes(str);
        TextBox textbox = FindName("bFolder") as TextBox;
        textbox.Text = root[0].InnerText;
        
    }
  }
}
