using MyBook.BookContent;
using RiddleInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace MyBook.Pages.Write.Riddle
{
    /// <summary>
    /// Interaction logic for RiddleSettings.xaml
    /// </summary>
    public partial class RiddleSettings : UserControl, ISettings
    {
        public struct PluginInfo
        {
            public String Name { get; set; }
        }
        public List<PluginInfo> Plugins;
        public RiddleSettings()
        {
            // find all dlls
            InitializeComponent();
            DataContext = new RiddleParagraph();
          
            Assembly assem = Assembly.GetExecutingAssembly();
            Uri ur = new Uri(assem.CodeBase);
            FileInfo fi = new FileInfo(ur.AbsolutePath);
            string s = fi.Directory.FullName;
            
            //find all dlls
            string[] dlls = Directory.GetFiles( s + "\\Plugins", "*.dll");
            ICollection<Assembly> assemblies = new List<Assembly>(dlls.Length);
            List<IRiddleHandler> riddles = new List<IRiddleHandler>();
            foreach ( string dllFile in dlls )
            {
                AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                Assembly assembly = Assembly.Load(an);
                assemblies.Add(assembly);
            }
            foreach( Assembly a in assemblies)
            {
                Type[] types = a.GetTypes();
                foreach(Type t in types)
                {
                    if (t.IsAbstract || t.IsNotPublic )
                        continue;
                    // TODO just write from the static methods
                    // create as instance
                    object iRiddle = a.CreateInstance(t.ToString());
                    IRiddleHandler riddle = iRiddle as IRiddleHandler;
                    riddles.Add(riddle);
                }
            }
            BindingGroup group = new BindingGroup();
            
            foreach (IRiddleHandler r in riddles)
            {
                RadioButton c = new RadioButton
                {
                    DataContext = r,
                    Content = r.Name
                };
                riddlePlugins.Children.Add(c);
                c.BindingGroup = group;
                c.Checked += new RoutedEventHandler(riddleChanged);
            }
        }

        private void riddleChanged(object sender, RoutedEventArgs e)
        {
            RadioButton box = sender as RadioButton;
            if (box.IsChecked == false)
                return;
            IRiddleHandler ir = box.DataContext as IRiddleHandler;
        }

        public IContent Create()
        {
            return new RiddleParagraph();
        }

        public void Reset()
        {
            // nothing to do right now
        }
    }
}
