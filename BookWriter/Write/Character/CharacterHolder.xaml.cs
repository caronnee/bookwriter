using MyBook.BookContent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
    
    public event PropertyChangedEventHandler PropertyChanged;
    void NotifyPropertyChanged(string property)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(property));
    }

    private EExpander CreateExpander (CharacterEpisodes ep)
    {
      EExpander e = new EExpander();
      e.DataContext = ep;
      return e;
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

    private void x_add_episode_Click(object sender, RoutedEventArgs e)
    {
      CharacterDescription s = DataContext as CharacterDescription;
      s.Episodes.Add(new CharacterEpisodes());
      // refresh
      DataContext = null;
      DataContext = s;
    }
  }
  public class EpisodeConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      List<CharacterEpisodes> episodes = value as List<CharacterEpisodes>;
      if (episodes == null)
        return null;
      List<EExpander> xExpanders = new List<EExpander>();
      for ( int i =0; i < episodes.Count; i++ )
      {
        EExpander exp = new EExpander();
        exp.DataContext = episodes[i];
        xExpanders.Add(exp);
      }
      return xExpanders;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
