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
    /// Interaktionslogik für AddEditNote.xaml
    /// </summary>
    public partial class AddEditNote : Window
    {
        public string caption = "";
        public string description = "";
        public string note = "";

        public AddEditNote()
        {
            InitializeComponent();
            
        }

        private void bnOk_Click(object sender, RoutedEventArgs e)
        {
            if (tbCaption.Text == "")
            {
                MessageBox.Show("Please fill caption.");
            }
            else
            {
                caption = tbCaption.Text;
                description = tbDescription.Text;
                note = tbNote.Text;
                DialogResult = true;
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbCaption.Text = caption;
            tbDescription.Text = description;
            tbNote.Text = note;
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
