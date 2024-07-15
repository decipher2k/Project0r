using Microsoft.Win32;
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
    /// Interaktionslogik für AddEditFile.xaml
    /// </summary>
    public partial class AddEditFile : Window
    {
        public String file;
        public String name;
        public String description;
        public bool relative;
        public bool startOnce;

        public AddEditFile()
        {
            InitializeComponent();
        }

        private void bnOk_Click(object sender, RoutedEventArgs e)
        {
            if (tbFile.Text == "" || tbCaption.Text == "")
            {
                MessageBox.Show("Please fill caption and file.");
            }
            else
            {
                file = tbFile.Text;
                name = tbCaption.Text;
                description = tbDescription.Text;
                relative = cbRelativePath.IsChecked == true;
                startOnce=cbStartOnce.IsChecked == true;
                this.DialogResult = true;
                this.Close();
            }
        }

        private void bnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog()==true)
            {
                tbFile.Text = ofd.FileName;
            }
                
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbCaption.Text = name;
            tbDescription.Text = description;
            tbFile.Text = file;
            cbStartOnce.IsChecked = startOnce;
        }
    }
}
