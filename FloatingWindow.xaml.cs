using Orchestra;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace ProjectOrganizer
{
    /// <summary>
    /// Interaktionslogik für FloatingWindow.xaml
    /// </summary>
    public partial class FloatingWindow : Window
    {
        public static FloatingWindow Instance;
        bool draged = false;
        bool startDragin=false;
        MainWindow mainWindow=new MainWindow();
        public FloatingWindow()
        {
            Instance = this;
            InitializeComponent();
            Project.Load();
            Left = Project.Instance.x;
            Top = Project.Instance.y;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            startDragin = true;
            if (!draged)
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    if (!mainWindow.IsVisible)
                    {

                        mainWindow = new MainWindow();
                        mainWindow.Show();

                    }
                    else
                    {
                        mainWindow.Activate();
                        mainWindow.WindowState = WindowState.Normal;
                        mainWindow.BringIntoView();
                    }
                }
            }
            else
            {
                Project.Save();
            }
            draged = false;
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            startDragin=false;
            if (!draged)
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    if (!mainWindow.IsVisible)
                    {

                        mainWindow = new MainWindow();
                        mainWindow.Show();

                    }
                    else
                    {
                        mainWindow.Activate();
                        mainWindow.WindowState = WindowState.Normal;
                        mainWindow.BringIntoView();
                    }
                }
            }
            else
            {
                Project.Save();
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if(Mouse.LeftButton==MouseButtonState.Pressed)
                this.DragMove();
            draged = true;
        }

        private void mnuCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow.IsVisible)
            {
                mainWindow.Close();
            }
            Application.Current.Shutdown();
        }

        private void mnuCloseWindow_Click(object sender, MouseButtonEventArgs e)
        {
            if (mainWindow.IsVisible)
            {
                mainWindow.Close();               
            }
            Application.Current.Shutdown();
        }

        private void closeAllWindows()
        {
            System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcesses();
            for (int i = 0; i < process.Length; i++)
            {
                try
                {
                    if (process[i].MainWindowHandle != IntPtr.Zero && process[i].Id != Process.GetCurrentProcess().Id && process[i].ProcessName!="olk"&& process[i].ProcessName != "Teams" && !process[i].ProcessName.ToLower().Contains("explorer") && !process[i].ProcessName.Contains("ShellExperienceHost") && !process[i].ProcessName.ToLower().Contains("outlook") && !process[i].ProcessName.ToLower().Contains("teams"))
                    {
                        process[i].Kill();
                    }
                }
                catch (Exception e)
                {
                    
                }
            }
           
        }

        private void mnuCloseAllWindows_Click(object sender, RoutedEventArgs e)
        {
           closeAllWindows();
        }
    }
}
