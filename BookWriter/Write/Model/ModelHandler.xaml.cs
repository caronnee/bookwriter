using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyBook.Write.Model
{
  /// <summary>
  /// Interaction logic for ModelHandler.xaml
  /// </summary>
  public partial class ModelHandler : UserControl
  {
    public ModelHandler()
    {
      InitializeComponent();
    }

    private void x_camera_KeyDown(object sender, KeyEventArgs e)
    {
      if ( e.Key == Key.A)
      {
        x_camera.Position = new System.Windows.Media.Media3D.Point3D(0,0,-8);
      }
    }

    delegate void ActionHandler();
    delegate void ActionObject();
    delegate void ActionAxis();

    Matrix3D transformationMatrix = Matrix3D.Identity;
    private void viewport_mouse_down(object sender, MouseButtonEventArgs e)
    {
    }

    private void Grid_MouseMove(object sender, MouseEventArgs e)
    {
    }

    private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
    {

    }
  }
}
