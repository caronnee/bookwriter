using MyBook.BookContent;
using System;
using System.Collections.Generic;
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

    public List<CharacterContent> Females { get; set; }
    public List<CharacterContent> Males { get; set; }
    public CharacterContent Character { get; set; }
    
    public event PropertyChangedEventHandler PropertyChanged;
    void NotifyPropertyChanged(string property)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(property));
    }

    public void Load( CharacterContent input)
    {
      Females = new List<CharacterContent>();
      Males = new List<CharacterContent>();
      Character = input;
      x_info.x_textContent.Text = Character.Info[0].Content;
      // find possible mothers and fathers
      BookSource s = DataContext as BookSource;
      foreach ( CharacterContent c in s.Characters)
      {
        if (c == input)
          continue;
        if (c.Gender == CharacterGender.Male)
          Males.Add(c);
        else
          Females.Add(c);
      }
      Males.Add(s.DummyCharacter);
      Females.Add(s.DummyCharacter);
      NotifyPropertyChanged("Males");
      NotifyPropertyChanged("Females");
      NotifyPropertyChanged("Character");
    }

    public override void Save()
    {
      CharacterEpisodes ep = Character.Info[0];
      ep.Title = "Biography";
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

    private void Button_Click(object sender, RoutedEventArgs e)
    {

    }

    //private void x_c_name_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    //{
    //  NotifyPropertyChanged("Character"); 
    //}
  }
}
