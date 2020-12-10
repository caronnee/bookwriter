using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Book
{
  /// <summary>
  /// Interaction logic for Book.xaml
  /// </summary>
  public partial class BookContainer : ItemsControl
  {
    public BookContainer()
    {
      InitializeComponent();
    }

    internal object GetPage(int index)
    {
      if ((index >= 0) && (index < Items.Count))
        return Items[index];

      Canvas c = new Canvas();
      //c.RenderSize = new Size(300, 300);
      c.Background = Brushes.SandyBrown;

      return c;
    }

    private int _startPage;

    private void RefreshContent()
    {
      // show left and right content
      BookPage sheet0 = GetTemplateChild("x_sheet_0") as BookPage;
      ContentPresenter p0 = sheet0.FindName("x_page_0") as ContentPresenter;
      ContentPresenter p1 = sheet0.FindName("x_page_1") as ContentPresenter;
      ContentPresenter p2 = sheet0.FindName("x_page_2") as ContentPresenter;
      p2.Content = GetPage(_startPage + 0);
      p1.Content = GetPage(_startPage + 1);
      p0.Content = GetPage(_startPage + 2);
      sheet0.CanDrag = _startPage > -3;

      BookPage sheet1 = GetTemplateChild("x_sheet_1") as BookPage;
      ContentPresenter ps0 = sheet1.FindName("x_page_0") as ContentPresenter;
      ContentPresenter ps1 = sheet1.FindName("x_page_1") as ContentPresenter;
      ContentPresenter ps2 = sheet1.FindName("x_page_2") as ContentPresenter;
      ps0.Content = GetPage(_startPage + 3);
      ps1.Content = GetPage(_startPage + 4);
      ps2.Content = GetPage(_startPage + 5);
      sheet1.CanDrag = (_startPage +3) < Items.Count;

      // make corner visible
      ClearBoxes(sheet0);
      ClearBoxes(sheet1);
    }

    private void ClearBoxes(BookPage sheet)
    {
      // clear all paths
      PathGeometry p = sheet.x_clip_page_2;
      p.Figures.Clear();
      sheet.x_clip_page_1.Figures.Clear();
      sheet.x_rectangleRotate.Angle = 0;
      sheet.x_rectangleTranslate.X = 0;
      sheet.x_rectangleTranslate.Y = 0;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      _startPage = -3;
      Items.Insert(0, new Cover());
      if (Items.Count % 2 == 0)
      {
        // todo Empty
        Items.Add(new EmptyPage());
      }

      Items.Add(new EndCover());

      Assembly assem = Assembly.GetExecutingAssembly();
      Uri ur = new Uri(assem.CodeBase);
      FileInfo fi = new FileInfo(ur.AbsolutePath);
      string s = fi.Directory.FullName;

      for (int i = 0; i < Items.Count; i++)
      {
        UserControl el = Items[i] as UserControl;
        ImageSource source = new BitmapImage( new Uri( s+"\\..\\..\\..\\book\\back.jpg"));
        ImageBrush b = new ImageBrush(source);
        el.BorderBrush = b;
        el.BorderThickness = new Thickness(30);
        el.Background = Brushes.SandyBrown;
      }

      BookPage s1 = GetTemplateChild("x_sheet_1") as BookPage;
      s1.StartPoint = new Point( s1.RenderSize.Width, 0 );
      RefreshContent();
    }

    BookPage _isDragging = null;
    
    private void ContentControl_MouseDown(object sender, MouseButtonEventArgs e)
    {
      BookPage s = sender as BookPage;
      if ( s == null )
        return;
      if (s.CanDrag == false)
        return;

      Point p = e.GetPosition(s);
      if (((p.X < _cornerSize) && (p.Y < _cornerSize)) ||
          (((s.RenderSize.Width - p.X) < _cornerSize) && (p.Y < _cornerSize)))
      {
        _isDragging = s;
      }
      else
      {
        return;
      }

      BookPage s0 = GetTemplateChild("x_sheet_0") as BookPage;
      BookPage s1 = GetTemplateChild("x_sheet_1") as BookPage;
      Canvas.SetZIndex(s0, 0);
      Canvas.SetZIndex(s1, 0);

      Canvas.SetZIndex(s, 1);
      s.StartDragging(p);
    }

    private void AnimateLeftComplete()
    {
      _startPage += 2;
      RefreshContent();
    }

    private void AnimateRightComplete()
    {
      _startPage -= 2;
      //System.Diagnostics.Debug.Assert(_startPage >= 0);
      RefreshContent();
    }

    public static DependencyProperty FinalPointProperty;

    static BookContainer()
    {
      // register final property
      FinalPointProperty = DependencyProperty.Register("FinalPoint", typeof(Point), typeof(BookContainer));
    }

    public Point FinalPoint
    {
      get { return (Point)GetValue(BookContainer.FinalPointProperty); }
      set { SetValue(BookContainer.FinalPointProperty, value); }
    }

    private void ContentControl_MouseUp(object sender, MouseButtonEventArgs e)
    {
      if (_isDragging == null)
        return;
      // set animation to finish
      BookPage b = _isDragging;
      _isDragging = null;

      Point p = e.GetPosition(b);
      if ( p.X > b.RenderSize.Width)
      {
        PointAnimation anim = new PointAnimation();
        anim.Duration = new Duration(new TimeSpan(0,0,2));
        anim.To = new Point(2*b.RenderSize.Width, 0);
        anim.From = p;
        anim.CurrentTimeInvalidated += AnimateToRight;
        anim.Completed += Anim_Completed_Right;
        this.BeginAnimation(BookContainer.FinalPointProperty, anim);
      }
      else if (p.X < 0)
      {
        PointAnimation anim = new PointAnimation();
        anim.Duration = new Duration(new TimeSpan(0, 0, 2));
        anim.To = new Point(-b.RenderSize.Width, 0);
        anim.From = p;
        anim.CurrentTimeInvalidated += AnimateToLeft;
        anim.Completed += Anim_Completed_Left;
        this.BeginAnimation(BookContainer.FinalPointProperty, anim);
      }
      else
      {
        RefreshContent();
      }
    }

    private void Anim_Completed_Right(object sender, EventArgs e)
    {
      AnimateRightComplete();
    }

    private void Anim_Completed_Left(object sender, EventArgs e)
    {
      AnimateLeftComplete();
    }

    private void AnimateToRight(object sender, EventArgs e)
    {
      BookPage s0 = GetTemplateChild("x_sheet_0") as BookPage;
      Point p = FinalPoint;
      s0.StartDragging(p);
    }

    private void AnimateToLeft(object sender, EventArgs e)
    {
      BookPage s0 = GetTemplateChild("x_sheet_1") as BookPage;
      Point p = FinalPoint;
      s0.StartDragging(p);
    }

    private const int _cornerSize = 25;

    private void ContentControl_MouseMove(object sender, MouseEventArgs e)
    {
      if (_isDragging == null)
      {
        UIElement holder = sender as UIElement;
        Point p = e.GetPosition(holder);
        bool startDrag = false;
        BookPage b = GetTemplateChild("x_sheet_0") as BookPage;
        if (p.X < b.RenderSize.Width)
        {
          if (_startPage ==-3)
            return;
          startDrag = (p.X < _cornerSize) && (p.Y < _cornerSize);
        }
        else
        {
          if (_startPage == Items.Count-3)
            return;
          b = GetTemplateChild("x_sheet_1") as BookPage;
          startDrag = ((holder.RenderSize.Width - p.X) < _cornerSize) && (p.Y < _cornerSize);
        }
        if (startDrag)
        {
          b.StartDragging(e.GetPosition(b));
        }
        else
        {
          RefreshContent();
        }
        return;
      }
      _isDragging.StartDragging(e.GetPosition(_isDragging));
    }
  }

}
