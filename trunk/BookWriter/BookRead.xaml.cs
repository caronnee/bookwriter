using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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


        public BookRead(String name)
        {
            _source = new BookSource(name) ; // TODO check the correct syntax of the file
            _source.Load();
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
        public bool IsValid()
        {
          return _source.IsValid();
        }
        void BookRead_Loaded(object sender, RoutedEventArgs e)
        {
          // fill first few pages
          BookPageLeft.Content = _source.GetPage(0);
          BookPageRight.Content = _source.GetPage(1);
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            _turnPage.Begin(BookContent, HandoffBehavior.SnapshotAndReplace);
        }
    }
}
