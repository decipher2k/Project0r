using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        Project project = new Project();

    public static MainWindow Instance { get; private set; }


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
                    tabItem.Content = new MainControl(key.Key) { VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch };
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
                project.Projects.Add(input, new Data());
                loadTabs();
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
/*                tabMain.Items.Remove(((TabItem)tabMain.SelectedItem));
                TabItem tabItem = new TabItem();
                tabItem.Header = input;
                tabItem.Content = new MainControl(input) { VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch };
                tabMain.Items.Add(tabItem);*/
                loadTabs();
            }
        }
    }
}
