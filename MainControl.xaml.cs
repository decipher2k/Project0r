﻿using Microsoft.Win32;
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
using System.Windows.Interop;
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
            this.dat =(Data) Projects.Instance.Project.Where(a=>a.Key==project).First().Value;
            InitializeComponent();           

            lbApps.ItemsSource = dat.Apps;
            lbFiles.ItemsSource = dat.Files;
            lbNotes.ItemsSource = dat.Notes;
            lbTodo.ItemsSource = dat.ToDo;
            lbCalendar.ItemsSource = dat.Calendar;
        }


        public static class WindowHelper
        {
            public static void BringProcessToFront(Process process)
            {
                IntPtr handle = process.MainWindowHandle;
                if (IsIconic(handle))
                {
                    ShowWindow(handle, SW_RESTORE);
                }

                SetForegroundWindow(handle);
            }

            const int SW_RESTORE = 9;

            [System.Runtime.InteropServices.DllImport("User32.dll")]
            private static extern bool SetForegroundWindow(IntPtr handle);
            [System.Runtime.InteropServices.DllImport("User32.dll")]
            private static extern bool ShowWindow(IntPtr handle, int nCmdShow);
            [System.Runtime.InteropServices.DllImport("User32.dll")]
            private static extern bool IsIconic(IntPtr handle);
        }
        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {                
                Program p = (Program)((ListBox)sender).SelectedItem;
                if (p.startOnce == true)
                {
                    if (p.process != null && !p.process.HasExited)
                    {
                        WindowHelper.BringProcessToFront((Process)p.process);
                    }
                    else
                    {
                        p.process = Process.Start(p.executaleFile);
                        MainWindow.Instance.Close();
                    }
                }
                else
                {
                    p.process = Process.Start(p.executaleFile);
                    MainWindow.Instance.Close();
                }
            }
            catch (Exception) { }
        }

        
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        


        }


        public static ImageSource ToImageSource(Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }
        private void bnAddProgram_Click(object sender, RoutedEventArgs e)
        {
            AddEditFile wnd = new AddEditFile();
            wnd.isExe = true;
            if (wnd.ShowDialog()==true)
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
                    p.startOnce = wnd.startOnce;                   
                    System.Drawing.Icon result = (System.Drawing.Icon)null;

                    result = System.Drawing.Icon.ExtractAssociatedIcon(wnd.file);
                    if (result != null)
                    {
                        ImageSource img = ToImageSource(result);
                        p.picture = img;
                        dat.Apps.Add(p);
                        Projects.Instance.Project[project] = dat;
                        Projects.Save();
                    }
                }
            }
         
        }

        private void bnAddFile_Click(object sender, RoutedEventArgs e)
        {
            AddEditFile wnd = new AddEditFile();
            wnd.isExe = false;
            if (wnd.ShowDialog() == true)
            {
                File p = new File();
                p.fileName = wnd.file;
                p.description = wnd.description;
                p.name = wnd.name;
                p.startOnce = wnd.startOnce;
                System.Drawing.Icon result = (System.Drawing.Icon)null;
                ImageSource img;

                try
                {
                    result = System.Drawing.Icon.ExtractAssociatedIcon(wnd.file);
                    img = ToImageSource(result);
                }
                catch
                {
                    img = Imaging.CreateBitmapSourceFromHBitmap(new System.Drawing.Bitmap(System.Drawing.Image.FromFile(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)+ "\\folder.png")).GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                 
                p.picture = img;
                dat.Files.Add(p);
                Projects.Instance.Project[project] = dat;
                Projects.Save();
                
            }
        }
     
        
        // Code For OpenWithDialog Box
    

        public const uint SW_NORMAL = 1;

       
       
        private void lbFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                File p = (File)((ListBox)sender).SelectedItem;
                if (p.startOnce == true)
                {
                    if (p.process != null && !p.process.HasExited)
                    {
                        WindowHelper.BringProcessToFront((Process)p.process);
                    }
                    else
                    {                                                                     
                        p.process = Process.Start(p.fileName);                     
                        MainWindow.Instance.Close();
                    }
                }
                else
                {
                    p.process = Process.Start(p.fileName);
                    MainWindow.Instance.Close();
                }
            }
            catch (Exception) { }
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
                Projects.Instance.Project[project].Notes.Add(note);
                Projects.Save();
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
                Projects.Instance.Project[project].ToDo.Add(toDo);
                Projects.Save();
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
                    for (int i = 0; i < Projects.Instance.Project[project].Notes.Count; i++)
                    {
                        if (Projects.Instance.Project[project].Notes[i].name == note.name)
                        {

                            Projects.Instance.Project[project].Notes[i].text = addEditNote.note;
                            Projects.Instance.Project[project].Notes[i].description = addEditNote.description;
                            Projects.Instance.Project[project].Notes[i].name = addEditNote.caption;
                            Projects.Save();
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
                    for (int i = 0; i < Projects.Instance.Project[project].ToDo.Count; i++)
                    {
                        if (Projects.Instance.Project[project].ToDo[i].caption == note.caption)
                        {

                            Projects.Instance.Project[project].ToDo[i].description = addEditNote.note;
                            Projects.Instance.Project[project].ToDo[i].caption = addEditNote.caption;                            
                            Projects.Save();
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
                    Projects.Instance.Project[project].Calendar.Add(toDo);
                    Projects.Save();
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
                  for (int i = 0; i < Projects.Instance.Project[project].Apps.Count; i++)
                  {
                      if (Projects.Instance.Project[project].Apps[i].name == p.name)
                      {
                          Projects.Instance.Project[project].Apps.RemoveAt(i);
                          break;
                      }
                  }

                  Projects.Save();
              }*/

            if (lbApps.SelectedItems.Count > 0 && currentLB==lbApps)
            {
                Program p = lbApps.SelectedItem as Program;
                Projects.Instance.Project[project].Apps.Remove(p);
                Projects.Save();
            }
            else if(lbFiles.SelectedItems.Count>0 && currentLB==lbFiles)
            {
                File p = lbFiles.SelectedItem as File;
                Projects.Instance.Project[project].Files.Remove(p);
                Projects.Save();
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
                Projects.Instance.Project[project].Notes.Remove(p);
                Projects.Save();
            }
        }

        private void bnDeleteToDo_Click(object sender, RoutedEventArgs e)
        {
            if (lbTodo.SelectedItems.Count > 0)
            {
                ToDo p = lbTodo.SelectedItem as ToDo;
                Projects.Instance.Project[project].ToDo.Remove(p);
                Projects.Save();
            }
        }

        private void bnDeleteCalendar_Click(object sender, RoutedEventArgs e)
        {
            if (lbCalendar.SelectedItems.Count > 0)
            {
                Calendar p = lbCalendar.SelectedItem as Calendar;
                Projects.Instance.Project[project].Calendar.Remove(p);
                Projects.Save();
            }
        }

        private void mnuAddCalendar_Click(object sender, RoutedEventArgs e)
        {
            bnAddCalendar_Click(sender, e);
        }

        private void mnuEditCalendar_Click(object sender, RoutedEventArgs e)
        {
            lbCalendar_SelectionChanged(sender, null);
        }

        private void mnuRemoveCalendar_Click(object sender, RoutedEventArgs e)
        {
            if (lbCalendar.SelectedItems.Count > 0)
            {
                Projects.Instance.Project[project].Calendar.Remove((Calendar)lbCalendar.SelectedItem);
                tbCalendarCaption.Text = string.Empty;
                tbCalendarDetails.Text = string.Empty;
                tbCalendarFrom.Text = string.Empty;
                tbCalendarTo.Text = string.Empty;
                calCalendar.SelectedDate = null;
            }
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
                    for (int i = 0; i < Projects.Instance.Project[project].ToDo.Count; i++)
                    {
                        if (Projects.Instance.Project[project].ToDo[i].caption == note.caption)
                        {

                            Projects.Instance.Project[project].ToDo[i].description = addEditNote.note;
                            Projects.Instance.Project[project].ToDo[i].caption = addEditNote.caption;
                            Projects.Save();
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
                    for (int i = 0; i < Projects.Instance.Project[project].Notes.Count; i++)
                    {
                        if (Projects.Instance.Project[project].Notes[i].name == note.name)
                        {

                            Projects.Instance.Project[project].Notes[i].text = addEditNote.note;
                            Projects.Instance.Project[project].Notes[i].description = addEditNote.description;
                            Projects.Instance.Project[project].Notes[i].name = addEditNote.caption;
                            Projects.Save();
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
            File p=lbFiles.SelectedItem as File;
            if (p != null)
            {
                AddEditFile wnd = new AddEditFile();
                wnd.file = p.fileName;
                wnd.name = p.name;
                wnd.description = p.description;
                wnd.startOnce=p.startOnce;
                wnd.isExe = false;

                if (wnd.ShowDialog() == true)
                {
                    p.fileName = wnd.file;
                    p.description = wnd.description;
                    p.name = wnd.name;
                    p.startOnce = wnd.startOnce;

                    System.Drawing.Icon result = (System.Drawing.Icon)null;

                    result = System.Drawing.Icon.ExtractAssociatedIcon(wnd.file);
                    if (result != null)
                    {
                        ImageSource img = ToImageSource(result);
                        p.picture = img;
                        for (int i = 0; i < dat.Files.Count; i++)
                        {
                            if (dat.Files[i] == (File)lbFiles.SelectedItem)
                            {
                                dat.Files[i] = p;
                                break;
                            }
                        }

                        Projects.Instance.Project[project] = dat;
                        Projects.Save();
                    }
                }
            }
        }

        private void mnuRemoveDocument_Click(object sender, RoutedEventArgs e)
        {
            bnDeleteAppFile_Click(sender, e);
        }

        private void mnuAddApp_Click(object sender, RoutedEventArgs e)
        {
            bnAddProgram_Click(sender, e);
        }

        private void mnuEditApp_Click(object sender, RoutedEventArgs e)
        {
            Program p = lbApps.SelectedItem as Program;
            if (p != null)
            {
                AddEditFile wnd = new AddEditFile();
                wnd.name = p.name;
                wnd.description = p.description;
                wnd.file = p.executaleFile;
                wnd.startOnce = p.startOnce;
                wnd.isExe = true;

                if (wnd.ShowDialog() == true)
                {
                    if (!System.IO.File.Exists(wnd.file))
                    {
                        MessageBox.Show("File not found.");
                    }
                    else
                    {

                        p.executaleFile = wnd.file;
                        p.description = wnd.description;
                        p.name = wnd.name;
                        p.startOnce = wnd.startOnce;

                        System.Drawing.Icon result = (System.Drawing.Icon)null;

                        result = System.Drawing.Icon.ExtractAssociatedIcon(wnd.file);
                        if (result != null)
                        {
                            ImageSource img = ToImageSource(result);
                            p.picture = img;

                            for (int i = 0; i < dat.Apps.Count; i++)
                            {
                                if (dat.Apps[i] == (Program)lbApps.SelectedItem)
                                {
                                    dat.Apps[i] = p;
                                    break;
                                }
                            }

                            Projects.Instance.Project[project] = dat;
                            Projects.Save();
                        }
                    }
                }
            }
        }

        private void mnuRemoveApp_Click(object sender, RoutedEventArgs e)
        {
            bnDeleteAppFile_Click((object)sender, e);
        }
    }
}
