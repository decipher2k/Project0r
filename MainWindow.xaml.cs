using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using Project_Assistant;

namespace ProjectOrganizer
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    /// 
    
    public partial class MainWindow : Window
    {

        public int VerticalTab=0;
    public static MainWindow Instance { get; private set; }

        FloatingWindow w;
        public MainWindow()
        {                   
            Instance = this;
            InitializeComponent();
            tabMain.Items.IsLiveSorting = true;

            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            try
            {
                String ret = (String)rk.GetValue("ProjectAssistant", "FALSE");
                if (ret != "FALSE")
                {
                    mnuStartWithWindows.IsChecked = true;
                }
            }
            catch (Exception ex) { }
        }

        void loadTabs()
        {
            
            if (Projects.Instance.Project.Count > 0)
            {
                tabMain.Items.Clear();
                var lst = Projects.Instance.Project.OrderBy(project => project.Key);

                foreach (KeyValuePair<String,Data> key in lst)
                {
                    TabItem tabItem = new TabItem();
                    tabItem.Header = key.Key;
                    tabItem.Content = new MainControl(key.Key) { Name="mainContent" };
                    tabMain.Items.Add(tabItem);
                    tabMain.SelectedIndex = 0;
                }
            }
        }

        bool noProjects = true;

        private void mnuExportData_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "JSON File (*.json)|*.json";
            saveFileDialog.DefaultExt="json";

            if (saveFileDialog.ShowDialog() == true)
            {
                Projects.Save(saveFileDialog.FileName);
            }
        }

		private void mnuProject_Click(object sender, RoutedEventArgs e)
        {
            MsgBox msgBox = new MsgBox("Project Name");
            if (msgBox.ShowDialog() == true)
            {
                string input = msgBox.ret;
                if (input != null && input != "")
                {
                    if (noProjects)
                        tabMain.Items.Clear();
                    noProjects = false;
                    Projects.Instance.Project.Add(input, new Data());
                    loadTabs();
                    Projects.Save();
                    tabMain.SelectedIndex = 0;
                }
            }
        }

        private void mnuRenameProject_Click(object sender, RoutedEventArgs e)
        {
            MsgBox msgBox = new MsgBox("Project Name");
            if (msgBox.ShowDialog() == true)
            {
                string input = msgBox.ret;

                if (input != null && input != "")
                {
                    Data tmp = Projects.Instance.Project[((TabItem)tabMain.SelectedItem).Header.ToString()];
                    Projects.Instance.Project.Remove(((TabItem)tabMain.SelectedItem).Header.ToString());
                    Projects.Instance.Project.Add(input, tmp);
                    Projects.Save();
                    loadTabs();
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadTabs();
            if(FloatingWindow.currentProject!="")
            {
                foreach (TabItem item in tabMain.Items)
                {
                    if(item.Header.ToString() ==FloatingWindow.currentProject)
                    {
                        tabMain.SelectedItem = item;
                        break;
                    }
                }            
            }
            loaded = true;
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            if(this.IsVisible) 
                this.Close();
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void bnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void mnuRemoveProject_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("You are about to delete the project " + ((TabItem)tabMain.SelectedItem).Header.ToString() + "!\r\nAre you sure?","Caution", MessageBoxButton.YesNo, MessageBoxImage.Warning)==MessageBoxResult.Yes)
            {
                Projects.Instance.Project.Remove(((TabItem)tabMain.SelectedItem).Header.ToString());
                Projects.Save();
                tabMain.Items.Remove(tabMain.SelectedItem);
            }
        }

        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void mnuStartWithWindows_Click(object sender, RoutedEventArgs e)
        {
   
                RegistryKey rk = Registry.CurrentUser.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                if (mnuStartWithWindows.IsChecked==true)
                    rk.SetValue("ProjectAssistant", System.Windows.Application.ResourceAssembly.Location);
                else
                    rk.DeleteValue("ProjectAssistant", false);

        }
        bool loaded = false;
        private void tabMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (loaded == true)
            {
                if (((TabItem)tabMain.SelectedItem) != null)
                {
                    FloatingWindow.currentProject = ((TabItem)tabMain.SelectedItem).Header.ToString();
                    ((MainControl)((TabItem)tabMain.SelectedItem).Content).setTab(VerticalTab);
                }
            }
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://www.paypal.com/donate/?hosted_button_id=9NHUZZDQDYYTS");
        }

		private void mnuImportData_Click(object sender, RoutedEventArgs e)
		{
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "JSON File (*.json)|*.json";
			openFileDialog.DefaultExt = "json";

            if(openFileDialog.ShowDialog()==true) 
            {
                if(System.Windows.MessageBox.Show("Warning! The import will overwrite your current data. Are you sure?", "Warning", MessageBoxButton.YesNo)==MessageBoxResult.Yes)
                {
                    Projects.Load(openFileDialog.FileName);
                    Projects.Save();
                    this.loadTabs();

				}
            }
		}
	}
}
