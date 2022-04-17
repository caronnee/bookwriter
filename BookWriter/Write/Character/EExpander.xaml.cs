using System.ComponentModel;
using System.Windows.Controls;
using static MyBook.BookContent.BookSource;

namespace MyBook.Write.Character
{
  /// <summary>
  /// Interaction logic for EExpander.xaml
  /// </summary>
  public partial class EExpander : UserControl, INotifyPropertyChanged
  {
    public EExpander()
    {
      Episode = new CharacterEpisodes();
      InitializeComponent();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    void NotifyPropertyChanged(string property)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(property));
    }

    private CharacterEpisodes _episode;
    public CharacterEpisodes Episode
    {
      get { return _episode; }
      set { _episode = value; NotifyPropertyChanged("Episode"); }
    }

    public delegate void onAddBefore( EExpander ex);
    public event onAddBefore OnAddBefore;

    public delegate void onAddAfter( EExpander ex);
    public event onAddAfter OnAddAfter;

    public delegate void onRemove( EExpander ex);
    public event onRemove OnRemove;

    private void Remove_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      if (OnRemove == null)
        return;
      OnRemove(this);
    }

    private void AddAfter_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      if (OnAddAfter == null)
        return;
      OnAddAfter(this);
    }

    private void AddBefore_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      if (OnAddBefore == null)
        return;
      OnAddBefore(this);
    }
  }
}
