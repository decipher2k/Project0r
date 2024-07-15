using Microsoft.Win32;
using Orc.Controls;
using Orchestra;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.InteropServices;
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

namespace ProjectOrganizer
{

    /// <summary>
    /// Interaktionslogik für MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
        Data dat;
        String project;
        public MainControl(String _project)
        {
            project = _project;
            this.dat =(Data) Project.Instance.Projects.Where(a=>a.Key==project).First().Value;
            InitializeComponent();           

            lbApps.ItemsSource = dat.Apps;
            lbFiles.ItemsSource = dat.Files;
            lbNotes.ItemsSource = dat.Notes;
            lbTodo.ItemsSource = dat.ToDo;
            lbCalendar.ItemsSource = dat.Calendar;
        }

       

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Program p = (Program)((ListBox)sender).SelectedItem;
                Process.Start(p.executaleFile);
                MainWindow.Instance.Close();
            }
            catch (Exception) { }
        }

        
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        


        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddEditFile wnd = new AddEditFile();
            if(wnd.ShowDialog()==true)
            {
                if (!System.IO.File.Exists(wnd.file))
                {
                    MessageBox.Show("File not found.");
                }
                else
                {
                    Program p = new Program();
                    p.executaleFile = wnd.file;
                    p.description = wnd.description;
                    p.name = wnd.name;
                    Icon result = (Icon)null;

                    result = Icon.ExtractAssociatedIcon(wnd.file);
                    if (result != null)
                    {
                        ImageSource img = result.ToImageSource();
                        p.picture = img;
                        dat.Apps.Add(p);
                        Project.Instance.Projects[project] = dat;
                        Project.Save();
                    }
                }
            }
         
        }

        private void bnAddFile_Click(object sender, RoutedEventArgs e)
        {
            AddEditFile wnd = new AddEditFile();
            if (wnd.ShowDialog() == true)
            {
                File p = new File();
                p.fileName = wnd.file;
                p.description = wnd.description;
                p.name = wnd.name;
                Icon result = (Icon)null;

                result = Icon.ExtractAssociatedIcon(wnd.file);
                if (result != null)
                {
                    ImageSource img = result.ToImageSource();
                    p.picture = img;
                    dat.Files.Add(p);
                    Project.Instance.Projects[project] = dat;
                    Project.Save();
                }
            }
        }

        private void lbFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            File p = (File)((ListBox)sender).SelectedItem;
            Process.Start(p.fileName);
            MainWindow.Instance.Close();
        }

        private void bnAddNote_Click(object sender, RoutedEventArgs e)
        {
            
            AddEditNote wnd = new AddEditNote();
            if (wnd.ShowDialog() == true)
            {
                Note note = new Note();
                note.text = wnd.note;
                note.description = wnd.description;
                note.name = wnd.caption;
                Project.Instance.Projects[project].Notes.Add(note);
                Project.Save();
            }
           
        }

        private void bnAddToDo_Click(object sender, RoutedEventArgs e)
        {
            AddEditNote wnd = new AddEditNote();
            if (wnd.ShowDialog() == true)
            {
                ToDo toDo = new ToDo();
                toDo.caption = wnd.caption;
                toDo.description = wnd.note;
                Project.Instance.Projects[project].ToDo.Add(toDo);
                Project.Save();
            }

        }

        private void lbNotes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Note note=lbNotes.SelectedItem as Note;
            if (note != null)
            {
                AddEditNote addEditNote = new AddEditNote();
                addEditNote.caption = note.name;
                addEditNote.description = note.description;
                addEditNote.note = note.text;

                if(addEditNote.ShowDialog() == true)
                {
                    for (int i = 0; i < Project.Instance.Projects[project].Notes.Count; i++)
                    {
                        if (Project.Instance.Projects[project].Notes[i].name == note.name)
                        {

                            Project.Instance.Projects[project].Notes[i].text = addEditNote.note;
                            Project.Instance.Projects[project].Notes[i].description = addEditNote.description;
                            Project.Instance.Projects[project].Notes[i].name = addEditNote.caption;
                            Project.Save();
                            break;
                        }
                    }
                }
            }
        }

        private void lbTodo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ToDo note = lbTodo.SelectedItem as ToDo;
            if (note != null)
            {
                AddEditNote addEditNote = new AddEditNote();
                addEditNote.caption = note.caption;
                addEditNote.note = note.description;
                

                if (addEditNote.ShowDialog() == true)
                {
                    for (int i = 0; i < Project.Instance.Projects[project].ToDo.Count; i++)
                    {
                        if (Project.Instance.Projects[project].ToDo[i].caption == note.caption)
                        {

                            Project.Instance.Projects[project].ToDo[i].description = addEditNote.note;
                            Project.Instance.Projects[project].ToDo[i].caption = addEditNote.caption;                            
                            Project.Save();
                            break;
                        }
                    }
                }
            }
        }

        private void bnCreateCalendar_Click(object sender, RoutedEventArgs e)
        {

            if (calCalendar.SelectedDate == null)
            {
                MessageBox.Show("Please select a date.");
            }
            else
            {
                try
                {
                    Calendar toDo = new Calendar();
                    toDo.caption = tbCalendarCaption.Text;
                    toDo.text = tbCalendarDetails.Text;
                    toDo.from = DateTime.Parse(tbCalendarFrom.Text);
                    toDo.to = DateTime.Parse(tbCalendarTo.Text);
                    toDo.handled = false;
                    toDo.date = (DateTime)calCalendar.SelectedDate;
                    Project.Instance.Projects[project].Calendar.Add(toDo);
                    Project.Save();
                }
                catch (Exception)
                {
                    MessageBox.Show("Invalid time format.");
                }
            }
        }

        private void lbCalendar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Calendar calendar = (Calendar)lbCalendar.SelectedItem;
            if (calendar != null)
            {
                tbCalendarCaption.Text=calendar.caption;
                tbCalendarDetails.Text = calendar.text;
                tbCalendarFrom.Text=calendar.from.ToShortTimeString();
                tbCalendarTo.Text = calendar.to.ToShortTimeString();
                calCalendar.SelectedDate = calendar.date;
            }
        }
        Control currentLB=null;
        private void bnAddCalendar_Click(object sender, RoutedEventArgs e)
        {
            lbCalendar.SelectedItem = null;

            tbCalendarCaption.Text = "";
            tbCalendarDetails.Text = "";
            tbCalendarFrom.Text = "";
            tbCalendarTo.Text = "";
            calCalendar.SelectedDate = DateTime.Now;
        }

        private void bnDeleteAppFile_Click(object sender, RoutedEventArgs e)
        {
            /*  if(lbApps.IsFocused && lbApps.SelectedItems.Count > 0)
              {
                  Program p=lbApps.SelectedItem as Program;
                  for (int i = 0; i < Project.Instance.Projects[project].Apps.Count; i++)
                  {
                      if (Project.Instance.Projects[project].Apps[i].name == p.name)
                      {
                          Project.Instance.Projects[project].Apps.RemoveAt(i);
                          break;
                      }
                  }

                  Project.Save();
              }*/

            if (lbApps.SelectedItems.Count > 0 && currentLB==lbApps)
            {
                Program p = lbApps.SelectedItem as Program;
                Project.Instance.Projects[project].Apps.Remove(p);
                Project.Save();
            }
            else if(lbFiles.SelectedItems.Count>0 && currentLB==lbFiles)
            {
                File p = lbFiles.SelectedItem as File;
                Project.Instance.Projects[project].Files.Remove(p);
                Project.Save();
            }
        }

        private void lbApps_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (lbApps.SelectedItems.Count > 0)
                currentLB = lbApps;
        }

        private void lbFiles_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (lbFiles.SelectedItems.Count > 0)
                currentLB = lbFiles;
        }

        private void bnDeleteNote_Click(object sender, RoutedEventArgs e)
        {
            if (lbNotes.SelectedItems.Count > 0)
            {
                Note p = lbNotes.SelectedItem as Note;
                Project.Instance.Projects[project].Notes.Remove(p);
                Project.Save();
            }
        }

        private void bnDeleteToDo_Click(object sender, RoutedEventArgs e)
        {
            if (lbTodo.SelectedItems.Count > 0)
            {
                ToDo p = lbTodo.SelectedItem as ToDo;
                Project.Instance.Projects[project].ToDo.Remove(p);
                Project.Save();
            }
        }

        private void bnDeleteCalendar_Click(object sender, RoutedEventArgs e)
        {
            if (lbCalendar.SelectedItems.Count > 0)
            {
                Calendar p = lbCalendar.SelectedItem as Calendar;
                Project.Instance.Projects[project].Calendar.Remove(p);
                Project.Save();
            }
        }

        private void mnuAddCalendar_Click(object sender, RoutedEventArgs e)
        {
            bnAddCalendar_Click(sender, e);
        }

        private void mnuEditCalendar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mnuRemoveCalendar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mnuRemoveToDo_Click(object sender, RoutedEventArgs e)
        {
            bnDeleteToDo_Click(sender, e);
        }

        private void mnuEditToDo_Click(object sender, RoutedEventArgs e)
        {
            ToDo note = lbTodo.SelectedItem as ToDo;
            if (note != null)
            {
                AddEditNote addEditNote = new AddEditNote();
                addEditNote.caption = note.caption;
                addEditNote.note = note.description;


                if (addEditNote.ShowDialog() == true)
                {
                    for (int i = 0; i < Project.Instance.Projects[project].ToDo.Count; i++)
                    {
                        if (Project.Instance.Projects[project].ToDo[i].caption == note.caption)
                        {

                            Project.Instance.Projects[project].ToDo[i].description = addEditNote.note;
                            Project.Instance.Projects[project].ToDo[i].caption = addEditNote.caption;
                            Project.Save();
                            break;
                        }
                    }
                }
            }
        }

        private void mnuAddToDo_Click(object sender, RoutedEventArgs e)
        {
            bnAddToDo_Click(sender, e);
        }

        private void mnuAddNote_Click(object sender, RoutedEventArgs e)
        {
            bnAddNote_Click(sender, e);
        }

        private void mnuEditNote_Click(object sender, RoutedEventArgs e)
        {
            Note note = lbNotes.SelectedItem as Note;
            if (note != null)
            {
                AddEditNote addEditNote = new AddEditNote();
                addEditNote.caption = note.name;
                addEditNote.description = note.description;
                addEditNote.note = note.text;

                if (addEditNote.ShowDialog() == true)
                {
                    for (int i = 0; i < Project.Instance.Projects[project].Notes.Count; i++)
                    {
                        if (Project.Instance.Projects[project].Notes[i].name == note.name)
                        {

                            Project.Instance.Projects[project].Notes[i].text = addEditNote.note;
                            Project.Instance.Projects[project].Notes[i].description = addEditNote.description;
                            Project.Instance.Projects[project].Notes[i].name = addEditNote.caption;
                            Project.Save();
                            break;
                        }
                    }
                }
            }
        }

        private void mnuRemoveNote_Click(object sender, RoutedEventArgs e)
        {
            bnDeleteNote_Click(sender, e);
        }

        private void mnuAddDocument_Click(object sender, RoutedEventArgs e)
        {
            bnAddFile_Click(sender, e);
        }

        private void mnuEditDocument_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void mnuRemoveDocument_Click(object sender, RoutedEventArgs e)
        {
            bnDeleteAppFile_Click(sender, e);
        }

        private void mnuAddApp_Click(object sender, RoutedEventArgs e)
        {
            Button_Click(sender, e);
        }

        private void mnuEditApp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mnuRemoveApp_Click(object sender, RoutedEventArgs e)
        {
            bnDeleteAppFile_Click((object)sender, e);
        }
    }
}
