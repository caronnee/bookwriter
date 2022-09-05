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

    private void myViewport_MouseDown(object sender, MouseButtonEventArgs e)
    {
      Point3D point3D = x_camera.Position;
      point3D.Offset(1,1,1);
      x_camera.Position = point3D;
    }
  }
}
