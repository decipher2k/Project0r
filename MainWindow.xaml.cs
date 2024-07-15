using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ControlzEx.Standard;
using Microsoft.VisualBasic;

namespace ProjectOrganizer
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       

    public static MainWindow Instance { get; private set; }

        FloatingWindow w;
        public MainWindow()
        {                   
            Instance = this;
            InitializeComponent();
            tabMain.Items.IsLiveSorting = true;
            
        }

        void loadTabs()
        {
            
            if (Project.Instance.Projects.Count > 0)
            {
                tabMain.Items.Clear();
                var lst = Project.Instance.Projects.OrderBy(project => project.Key);

                foreach (KeyValuePair<String,Data> key in lst)
                {
                    TabItem tabItem = new TabItem();
                    tabItem.Header = key.Key;
                    tabItem.Content = new MainControl(key.Key) {  };
                    tabMain.Items.Add(tabItem);
                    tabMain.SelectedIndex = 0;
                }
            }
        }

        bool noProjects = true;
        private void mnuProject_Click(object sender, RoutedEventArgs e)
        {
            string input = VipMessageBox.MessageBox.InputBoxVIP.Show(this, "Project Name");
            if (input != null && input != "")
            {
                if(noProjects)
                    tabMain.Items.Clear();
                noProjects = false;
                Project.Instance.Projects.Add(input, new Data());
                loadTabs();
                Project.Save();
                tabMain.SelectedIndex = 0;
            }
        }

        private void mnuRenameProject_Click(object sender, RoutedEventArgs e)
        {
            string input = VipMessageBox.MessageBox.InputBoxVIP.Show(this, "Project Name");
            if(input != null && input !="")
            {
                Data tmp = Project.Instance.Projects[((TabItem)tabMain.SelectedItem).Header.ToString()];
                Project.Instance.Projects.Remove(((TabItem)tabMain.SelectedItem).Header.ToString());             
                Project.Instance.Projects.Add(input,tmp);
                Project.Save();
                loadTabs();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadTabs();
            
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
                Project.Instance.Projects.Remove(((TabItem)tabMain.SelectedItem).Header.ToString());
                Project.Save();
                tabMain.Items.Remove(tabMain.SelectedItem);
            }
        }

        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
