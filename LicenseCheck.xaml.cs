using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Project_Assistant
{
    /// <summary>
    /// Interaktionslogik für LicenseCheck.xaml
    /// </summary>
    public partial class LicenseCheck : Window
    {
        public LicenseCheck()
        {
            InitializeComponent();

         
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!(License.Status.Licensed && License.Status.License_HardwareID == License.Status.HardwareID))
                Process.GetCurrentProcess().Kill();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!(License.Status.Licensed && License.Status.License_HardwareID == License.Status.HardwareID))
                Process.GetCurrentProcess().Kill();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (License.Status.Licensed && License.Status.License_HardwareID == License.Status.HardwareID)
                this.Close();

            tbHWID.Text = License.Status.HardwareID;
        }
    }
}
