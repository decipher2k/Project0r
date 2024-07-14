using Microsoft.Win32;
using Orchestra;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectOrganizer
{

    /// <summary>
    /// Interaktionslogik für MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
        Data dat;
        public MainControl(String project)
        {
            this.dat =(Data) Project.Instance.Projects.Where(a=>a.Key==project).First().Value;
            InitializeComponent();           

            lbApps.ItemsSource = dat.Apps;
            lbFiles.ItemsSource = dat.Files;
            lbNotes.ItemsSource = dat.Notes;
        }

        

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Program p = (Program)((ListBox)sender).SelectedItem;
            Process.Start(p.executaleFile);
        }

        
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        


        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Topmost = false;
            AddEditFile wnd = new AddEditFile();
            if(wnd.ShowDialog()==true)
            {
                Program p=new Program();
                p.executaleFile = wnd.file;
                p.description=wnd.description;
                p.name = wnd.name;
                Icon result = (Icon)null;

                result =  Icon.ExtractAssociatedIcon(wnd.file);
                if (result != null)
                {
                    ImageSource img = result.ToImageSource();
                    p.picture = img;
                    dat.Apps.Add(p);
                }
            }
            MainWindow.Instance.Topmost = true;
        }
    }
}
