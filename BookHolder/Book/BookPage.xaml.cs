using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Book
{
  /// <summary>
  /// Interaction logic for BookPage.xaml
  /// </summary>
  public partial class BookPage : ContentControl
  {
    static private double offset = 100;

    public BookPage()
    {
      InitializeComponent();
    }

    public Point StartPoint { get; set; }

    public void StartDragging(Point p)
    {
      //offset = RenderSize.Width / 2;
      //p.X = RenderSize.Width - offset;
      //p.Y = offset;
      RefreshUpper(ref x_clip_page_2, p, RenderSize);
      RefreshMiddle(ref x_clip_page_1, p, ref x_rectangleRotate, ref x_rectangleTranslate, RenderSize);
    }

    private void RefreshUpper(ref PathGeometry pg, Point p, Size size)
    {
      pg.Clear();
      PathFigure fig = new PathFigure();
      fig.StartPoint = StartPoint;
      Point cPoint = CalcPoint(p);
      fig.Segments.Add(new LineSegment(new Point(cPoint.X, StartPoint.Y), true));
      fig.Segments.Add(new LineSegment(new Point(StartPoint.X, cPoint.Y), true));
      pg.Figures.Add(fig);
    }

    private Point CalcPoint(Point p)
    {
      double a2 = p.X * p.X;
      double b2 = p.Y * p.Y;
      double sx2 = StartPoint.X * StartPoint.X;
      double sy2 = StartPoint.Y * StartPoint.Y;
      double a2b2 = a2 + b2 - sx2 - sy2;
      double x = (0.5f * a2b2 - (p.Y - StartPoint.Y) * StartPoint.Y) / (p.X - StartPoint.X);
      double y = (0.5f * a2b2 - (p.X - StartPoint.X) * StartPoint.X) / (p.Y - StartPoint.Y);
      return new Point(x, y);
    }

    public bool CanDrag { get; set; }

    private void RefreshMiddle(ref PathGeometry path, Point p, ref RotateTransform rot, ref TranslateTransform transl, Size size)
    {
      path.Clear();
      Point cPoint = CalcPoint(p);
      PathFigure fig = new PathFigure();
      fig.StartPoint = new Point(size.Width - cPoint.X, 0);
      fig.Segments.Add(new LineSegment(new Point(size.Width - StartPoint.X, 0), true));
      fig.Segments.Add(new LineSegment(new Point(size.Width - StartPoint.X, cPoint.Y), true));
      path.Figures.Add(fig);

      // rotation
      double a = 2 * Math.Atan(p.Y / (p.X -StartPoint.X) );
      rot.Angle = 180 * a / Math.PI;
      rot.CenterX = cPoint.X;
      rot.CenterY = StartPoint.Y;

      // translation
      transl.X = 2 * cPoint.X - size.Width;

      // reflection setting
      Point d = new Point(Math.Abs(p.X - StartPoint.X) / (RenderSize.Width), (p.Y) / (RenderSize.Height));

      double o1 = (RenderSize.Width - StartPoint.X) / RenderSize.Width;
      // working right
      x_page_reflection.StartPoint = d;// new Point(1, 0);//d;// StartPoint;
      x_page_reflection.EndPoint = new Point(o1, 0);

      x_page_reflection.GradientStops[0].Color = Colors.White;
      x_page_reflection.GradientStops[1].Color = Colors.Transparent;

      if (o1 > 0.5)
      {
        x_page_reflection.StartPoint = new Point(o1, 0);
        x_page_reflection.GradientStops[0].Color = Colors.Transparent;
        x_page_reflection.GradientStops[1].Color = Colors.White;
        x_page_reflection.EndPoint = new Point(1-d.X,d.Y);
      }
    }

  }
}
