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

namespace Project_Assistant
{
    /// <summary>
    /// Interaktionslogik für MsgBox.xaml
    /// </summary>
    public partial class MsgBox : Window
    {
        public String ret;
        public MsgBox(String caption)
        {
            InitializeComponent();
            lblCaption.Content = caption;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ret=tbValue.Text;
            this.DialogResult = true;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void tbValue_KeyUp(object sender, KeyEventArgs e)
        {
           
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
