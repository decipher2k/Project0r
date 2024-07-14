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
        [DllImport("Shell32.dll")]
        public extern static int ExtractIconEx(
    string libName,
    int iconIndex,
    IntPtr[] largeIcon,
    IntPtr[] smallIcon,
    uint nIcons
);
        public static Icon GetIconForExtension(string extension)
        {
            RegistryKey keyForExt = Registry.ClassesRoot.OpenSubKey(extension);
            if (keyForExt == null) return null;

            string className = Convert.ToString(keyForExt.GetValue(null));
            RegistryKey keyForClass = Registry.ClassesRoot.OpenSubKey(className);
            if (keyForClass == null) return null;

            RegistryKey keyForIcon = keyForClass.OpenSubKey("DefaultIcon");
            if (keyForIcon == null)
            {
                RegistryKey keyForCLSID = keyForClass.OpenSubKey("CLSID");
                if (keyForCLSID == null) return null;

                string clsid = "CLSID\\"
                    + Convert.ToString(keyForCLSID.GetValue(null))
                    + "\\DefaultIcon";
                keyForIcon = Registry.ClassesRoot.OpenSubKey(clsid);
                if (keyForIcon == null) return null;
            }

            string[] defaultIcon = Convert.ToString(keyForIcon.GetValue(null)).Split(',');
            int index = (defaultIcon.Length > 1) ? Int32.Parse(defaultIcon[1]) : 0;

            IntPtr[] handles = new IntPtr[1];
            if (ExtractIconEx(defaultIcon[0], index,
                handles, null, 1) > 0)
                return Icon.FromHandle(handles[0]);
            else
                return null;
        }

        public MainControl()
        {
            InitializeComponent();
            BitmapImage image = new BitmapImage(new Uri("file:///e:/file.png"));


            dat.Notes.Add(new Note() { name = "Note" });
            dat.Apps.Add(new Program() { name = "Program", picture = image });


            dat.Files.Add(new File() { name = "File" });

            lbApps.ItemsSource = dat.Apps;
            lbFiles.ItemsSource = dat.Files;
            lbNotes.ItemsSource = dat.Notes;
        }

        [Serializable]
        public class Data
        {
            private  ObservableCollection<File> files;
            public  ObservableCollection<File> Files
            {
                get
                {
                    if (files == null)
                        files = new ObservableCollection<File>();
                    return files;
                }
            }

            private  ObservableCollection<Program> apps;
            public  ObservableCollection<Program> Apps
            {
                get
                {
                    if (apps == null)
                        apps = new ObservableCollection<Program>();
                    return apps;
                }
            }

            private  ObservableCollection<Note> notes;
            public  ObservableCollection<Note> Notes
            {
                get
                {
                    if (notes == null)
                        notes = new ObservableCollection<Note>();
                    return notes;
                }
            }
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Program p = (Program)((ListBox)sender).SelectedItem;
            Process.Start(p.executaleFile);
        }

        Data dat = new Data();
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
                    String tmp = System.IO.Path.GetTempPath() + "\\";
                    FileStream fs = new FileStream(tmp + "tmp.ico", FileMode.Create);
                    result.Save(fs);
                    fs.Close();
                    ImageSource img = result.ToImageSource();
                    p.picture = img;
                    dat.Apps.Add(p);
                }
            }
            MainWindow.Instance.Topmost = true;
        }
    }
}
