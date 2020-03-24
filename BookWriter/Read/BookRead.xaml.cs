using MyBook.BookContent;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace MyBook
{
  /// <summary>
  /// Interaction logic for BookRead.xaml
  /// </summary>
  public partial class BookRead : UserControl
  {
    private BookSource _source;
    private Storyboard _turnPage;

    // back to main menyu
    public delegate void BackHandler();
    public event BackHandler Back;

    public BookRead(String name)
    {
      _source = new BookSource(); // TODO check the correct syntax of the file
      //_source.Load(name);
      InitializeComponent();

      _turnPage = (Storyboard)this.Resources["TurnPage"];
    }
    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      //_leftPageTemplate = (ControlTemplate)this.Resources["Flippable"];
      // Grid o = _leftPageTemplate.FindName("GridTest", this. ) as Grid;

      // System.Diagnostics.Debug.Assert(o!=null);
    }
  }
}
