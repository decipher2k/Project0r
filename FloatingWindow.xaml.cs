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
            draged = false;
            startDragin = true;
           
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            startDragin=false;
            if (!draged)
            {
                if (!mainWindow.IsVisible)
                {
                    mainWindow=new MainWindow();
                    mainWindow.Show();
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
    }
}
