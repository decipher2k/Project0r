using ProjectOrganizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
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
    /// Interaktionslogik für Reminder.xaml
    /// </summary>
    ///     
    public partial class Reminder : Window
    {
        Data _calendar;
        Action<String> _callback;
        String _title;
        public static Reminder instance;
        public Reminder(Data cal, String project, Action<String> callback)
        {
            instance = this;
            InitializeComponent();
            _calendar = cal;
            _callback = callback;
            _title = project;
            lbReminder.ItemsSource = _calendar.Calendar;

            this.Title="Reminder: "+project;
        }

        public void UpdateItems(Data cal)
        {
            _calendar = cal;
            lbReminder.ItemsSource= _calendar.Calendar.Where(a=>a.date>DateTime.Now);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void bnOK_Click(object sender, RoutedEventArgs e)
        {     
            _callback.Invoke(_title);
            this.Close();
        }
    }
}
