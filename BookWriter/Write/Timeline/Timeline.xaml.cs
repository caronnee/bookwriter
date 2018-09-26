using MyBook.BookContent;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static MyBook.BookContent.BookSource;

namespace MyBook.Write.Timeline
{
  /// <summary>
  /// Interaction logic for Timeline.xaml
  /// </summary>
  public partial class Timeline : UserControl
  {
    // initial width of the button
    private static int _width = 80;

    // initial height of the button
    private static int _height = 30;
    
    // margin between the columns
    private static int _margin = 10;
    
    // step by which the length of the canvas is done
    static double step = 100;

    private void MoveButton(Button b, int timePosition, int column)
    {
      Canvas.SetTop(b, timePosition);
      int x = (_width + _margin) * column + _margin;
      Canvas.SetLeft(b, x);
      if (timePosition >= x_timeline_show.ActualHeight - _height)
      {
        x_timeline_show.Height = Math.Max( timePosition + step, x_timeline_show.Height);
      }
      if (x > x_timeline_show.ActualWidth - _width)
        x_timeline_show.Width = Math.Max( x + step, x_timeline_show.Width);
    }

    private List<Button> _todos;
    public Timeline( List<SceneDescription> scenes )
    {
      toMove = null;
      InitializeComponent();
      _todos = new List<Button>();
      x_timeline_show.Height = 0;
      x_timeline_show.Width = 0;
      foreach (SceneDescription d in scenes)
      {
        Button b = new Button();
        b.Content = d.Name;
        b.ToolTip = d.Description;
        b.DataContext = d;
        b.Height = _height * d.Duration;
        b.Width = _width;
        b.MouseDoubleClick += new MouseButtonEventHandler(SwitchToPage);
        b.MouseDown += new MouseButtonEventHandler(Button_MouseDown);
        x_timeline_show.Children.Add(b);
        if (d.TimePosition < 0)
        {
          _todos.Add(b);
        }
        else
          MoveButton(b, d.TimePosition, d.Column);
      }
      RearrangeTodos();
    }

    public delegate void Finalizer();
    public Finalizer OnFinished;

    public void SwitchToPage(object o, RoutedEventArgs args)
    {
      Button b = o as Button;
      BookSource c = DataContext as BookSource;
      c.SetScene(b.DataContext as SceneDescription);
      OnFinished();
    }
    
    private Button toMove;

    private void Button_MouseDown(object sender, MouseButtonEventArgs e)
    {
      toMove = sender as Button;
      _todos.Remove(toMove);
      for (int i = 0; i < _todos.Count; i++)
        _todos[i].Visibility = Visibility.Hidden;
    }
    
    private void RearrangeTodos()
    {
      for (int i = 0; i < _todos.Count; i++) 
      {
        Button b = _todos[i];
        b.Visibility = Visibility.Visible;
        MoveButton(b, (int)(x_timeline_holder.ContentVerticalOffset), i);
      }
    }
    private void Button_MouseUp(object sender, MouseButtonEventArgs e)
    {
      if (toMove == null)
        return;
      SceneDescription scene = toMove.DataContext as SceneDescription;
      Point p = e.MouseDevice.GetPosition(x_timeline_show);
      if (p.Y < _height)
      {
        scene.TimePosition = -1;
        _todos.Add(toMove);
      }
      else
      {
        MoveButton(toMove, scene.TimePosition, scene.Column);
      }
      toMove = null;
      // in case on of the todos was removed
      RearrangeTodos();
    }

    private void x_timeline_show_MouseMove(object sender, MouseEventArgs e)
    {
      if (toMove == null)
        return;
      Point p = e.MouseDevice.GetPosition(x_timeline_show);
      SceneDescription desc = toMove.DataContext as SceneDescription;
      desc.Column = (int)p.X/(_width+_margin);
      desc.TimePosition = Math.Max((int)p.Y,_height+_margin);
      MoveButton(toMove, desc.TimePosition, desc.Column);      
    }
  }
}
