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
  public partial class CharacterHolder : UserControl
  {
    public CharacterHolder()
    {
      InitializeComponent();
    }

    public List<CharacterDescription> Females { get; set; }
    public List<CharacterDescription> Males { get; set; }
    public CharacterDescription Character { get; set; }
    
    public event PropertyChangedEventHandler PropertyChanged;
    void NotifyPropertyChanged(string property)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(property));
    }

    public void Load( CharacterDescription input)
    {
      Females = new List<CharacterDescription>();
      Males = new List<CharacterDescription>();
      Character = input;
      // find possible mothers and fathers
      BookSource s = DataContext as BookSource;
      foreach (CharacterDescription c in s.Characters)
      {
        if (c == input)
          continue;
        if (c.Gender == CharacterGender.Male)
          Males.Add(c);
        else
          Females.Add(c);
      }
      x_episodesHolder.Children.Clear();
      foreach (CharacterEpisodes ep in Character.Info)
      {
        x_episodesHolder.Children.Add(CreateExpander(ep));
      }
      // notify all
      NotifyPropertyChanged("Males");
      NotifyPropertyChanged("Females");
      NotifyPropertyChanged("Character");
    }

    private EExpander CreateExpander (CharacterEpisodes ep)
    {
      EExpander e = new EExpander();
      e.DataContext = ep;
      e.OnAddBefore += AddBefore;
      e.OnRemove += Remove;
      e.OnAddAfter += OnAddAfter;
      return e; ;
    }
    private void OnAddAfter(EExpander ex)
    {
      CharacterEpisodes nep = new CharacterEpisodes();
      nep.Title = "Episode";
      int index = Character.Info.IndexOf(ex.Episode);
      Character.Info.Insert(index+1, nep);
      x_episodesHolder.Children.Insert(index+1, CreateExpander(nep));
    }

    private void Remove(EExpander ee)
    {
      Character.Info.Remove(ee.Episode);
      x_episodesHolder.Children.Remove(ee);
    }

    private void AddBefore(EExpander ee)
    {
      CharacterEpisodes nep = new CharacterEpisodes();
      nep.Title = "Episode";
      int index = Character.Info.IndexOf(ee.Episode);
      Character.Info.Insert(index, nep);
      x_episodesHolder.Children.Insert(index,CreateExpander(nep));
    }

    public void Save()
    {

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
  }
}
