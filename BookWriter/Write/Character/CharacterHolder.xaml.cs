using MyBook.BookContent;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static MyBook.BookContent.BookSource;

namespace MyBook.Write.Character
{
  /// <summary>
  /// Interaction logic for CharacterInfo.xaml
  /// </summary>
  public partial class CharacterHolder : IGuiContent, INotifyPropertyChanged
  {
    public CharacterHolder()
    {
      InitializeComponent();
    }

    public CharacterContent Character { get; set; }
    
    public event PropertyChangedEventHandler PropertyChanged;
    void NotifyPropertyChanged(string property)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(property));
    }

    public void Load( CharacterContent c)
    {
      Character = c;
      //todo change for episodes
      x_c_name.Text = Character.Name;
      x_info.x_textContent.Text = Character.Info[0].Content;      
    }
    
    public override void Save()
    {
      Character.Name = x_c_name.Text;
      CharacterEpisodes ep = Character.Info[0];
      ep.Name = "Biography";
      ep.Content = x_info.x_textContent.Text;
      Character.Info[0] = ep;
    }
    
    private void ChangeImageClick(object sender, RoutedEventArgs e)
    {
      System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();

      //openFileDialog1.InitialDirectory = "c:\\";
      openFileDialog1.Filter = "Image files (*.jpg, *.png, *.bmp)|*.jpg;*.png;*.bmp";
      openFileDialog1.FilterIndex = 0;
      openFileDialog1.RestoreDirectory = true;

      if (openFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
        return;

      string selectedFileName = openFileDialog1.FileName;
      BitmapImage im = new BitmapImage();
      im.BeginInit();
      im.DecodePixelWidth = 200;
      im.UriSource = new Uri(selectedFileName);
      im.EndInit();
      x_characterImage.Source = im;
    }

    private void x_c_name_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
      NotifyPropertyChanged("Character");
      
    }
  }
}
