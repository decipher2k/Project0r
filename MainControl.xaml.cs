using Microsoft.Win32;
using Project_Assistant;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
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
using static System.Net.WebRequestMethods;

namespace ProjectOrganizer
{

    /// <summary>
    /// Interaktionslogik für MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
   
        String project;
        public MainControl(String _project)
        {
            project = _project;
            InitializeComponent();

            reloadItems();
        }

        private void reloadItems()
        {
            lbApps.ItemsSource = null;
            lbFiles.ItemsSource = null;
            lbNotes.ItemsSource = null;
            lbTodo.ItemsSource = null;
            lbCalendar.ItemsSource = null;
            lbLog.ItemsSource = null;

            lbApps.ItemsSource = Projects.Instance.Project[project].Apps;
            lbFiles.ItemsSource = Projects.Instance.Project[project].Files;
            lbNotes.ItemsSource = Projects.Instance.Project[project].Notes;
            lbTodo.ItemsSource = Projects.Instance.Project[project].ToDo.OrderBy(a=>a.priority).AsEnumerable();
            lbCalendar.ItemsSource = Projects.Instance.Project[project].Calendar;
            lbLog.ItemsSource = Projects.Instance.Project[project].Log;
            
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

        public void setTab(int idx)
        {
            tabMain.SelectedIndex = idx;
        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            MainWindow.Instance.VerticalTab = tabMain.SelectedIndex;


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
                    if(Projects.Instance.Project[project].Apps.Count == 0)
                        p.id = 0;
                    else
                        p.id=Projects.Instance.Project[project].Apps.Max(a => a.id)+1;

                    p.startOnce = wnd.startOnce;                   
                    System.Drawing.Icon result = (System.Drawing.Icon)null;

                    result = System.Drawing.Icon.ExtractAssociatedIcon(wnd.file);
                    if (result != null)
                    {
                        ImageSource img = ToImageSource(result);
                        p.picture = img;
                        Projects.Instance.Project[project].Apps.Add(p);                       
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
                p.id = Projects.Instance.Project[project].Files.Count==0?0:Projects.Instance.Project[project].Files.Max(a => a.id) + 1;
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
                Projects.Instance.Project[project].Files.Add(p);                
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
                if(Projects.Instance.Project[project].Notes.Count==0)
                    note.id = 0;
                else
                    note.id = Projects.Instance.Project[project].Notes.Max(a => a.id) + 1;
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
                toDo.id = Projects.Instance.Project[project].ToDo.Count == 0 ? 0 : Projects.Instance.Project[project].ToDo.Max(a => a.id) + 1;
                toDo.description = wnd.note;
                Projects.Instance.Project[project].ToDo.Add(toDo);
                Projects.Save();
                lbTodo.ItemsSource = Projects.Instance.Project[project].ToDo;
            }

        }

        private void bnAddLog_Click(object sender, RoutedEventArgs e)
        {
            AddEditNote wnd = new AddEditNote();
            if (wnd.ShowDialog() == true)
            {
                Log log = new Log();
                log.caption = wnd.caption;
                log.id = Projects.Instance.Project[project].Log.Count == 0 ? 0 : Projects.Instance.Project[project].Log.Max(a => a.id) + 1;
                log.description = wnd.note;
                log.date=DateTime.Now;
                Projects.Instance.Project[project].Log.Insert(0,log);                           
                Projects.Save();
                lbLog.ItemsSource = Projects.Instance.Project[project].Log;
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
            ToDo toDo = lbLog.SelectedItem as ToDo;
            if (toDo != null)
            {
                AddEditNote addEditNote = new AddEditNote();
                addEditNote.caption = toDo.caption;
                addEditNote.note = toDo.description;


                if (addEditNote.ShowDialog() == true)
                {
                    for (int i = 0; i < Projects.Instance.Project[project].ToDo.Count; i++)
                    {
                        if (Projects.Instance.Project[project].ToDo[i].caption == toDo.caption)
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

        private void lbLog_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Log log = lbLog.SelectedItem as Log;
            if (log != null)
            {
                AddEditNote addEditNote = new AddEditNote();
                addEditNote.caption = log.caption;
                addEditNote.note = log.description;
                

                if (addEditNote.ShowDialog() == true)
                {
                    for (int i = 0; i < Projects.Instance.Project[project].Log.Count; i++)
                    {
                        if (Projects.Instance.Project[project].Log[i].caption == log.caption)
                        {

                            Projects.Instance.Project[project].Log[i].description = addEditNote.note;
                            Projects.Instance.Project[project].Log[i].caption = addEditNote.caption;                            
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
                    if (bnCreateCalendar.Content.ToString().Equals("Save")==false)
                    {
                        Calendar toDo = new Calendar();
                        
                        toDo.caption = tbCalendarCaption.Text;
                        toDo.text = tbCalendarDetails.Text;
                        toDo.from = DateTime.Parse(tbCalendarFrom.Text);
                        toDo.to = DateTime.Parse(tbCalendarTo.Text);
                        toDo.handled = false;
                        toDo.date = (DateTime)calCalendar.SelectedDate;
                        toDo.id = Projects.Instance.Project[project].Calendar.Count == 0 ? 0 : Projects.Instance.Project[project].Calendar.Max(a => a.id) + 1;
                        Projects.Instance.Project[project].Calendar.Add(toDo);
                        Projects.Save();
                        
                        bnAddCalendar_Click(sender, e);
                    }
                    else
                    {
                       
                        Calendar c=(Calendar)lbCalendar.SelectedItem;                        
                        for (int i = 0; i < Projects.Instance.Project[project].Calendar.Count; i++)
                        {
                            
                            if (Projects.Instance.Project[project].Calendar[i].id == c.id)
                            {
                                Projects.Instance.Project[project].Calendar[i].from = DateTime.Parse(tbCalendarFrom.Text);
                                Projects.Instance.Project[project].Calendar[i].to = DateTime.Parse(tbCalendarTo.Text);
                                Projects.Instance.Project[project].Calendar[i].text = tbCalendarDetails.Text;
                                Projects.Instance.Project[project].Calendar[i].caption = tbCalendarCaption.Text;
                                Projects.Save();
                                reloadItems();
                                break;
                            }
                        }                        

                    }
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
                bnCreateCalendar.Content = "Save";
                
                tbCalendarCaption.Text=calendar.caption;
                tbCalendarDetails.Text = calendar.text;
                tbCalendarFrom.Text=calendar.from.ToShortTimeString();
                tbCalendarTo.Text = calendar.to.ToShortTimeString();
                calCalendar.SelectedDate = calendar.date;
            }
            else
            {
                bnCreateCalendar.Content = "Create";
                
                tbCalendarCaption.Text = "";
                tbCalendarFrom.Text = "";
                tbCalendarTo.Text = "";
                tbCalendarDetails.Text = "";
                calCalendar.SelectedDate = null;
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
                if (MessageBox.Show("Move to Log?", "Move to Log", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {                          
                    Log log = new Log();
                    log.caption = p.caption;
                    log.id = Projects.Instance.Project[project].Log.Count == 0 ? 0 : Projects.Instance.Project[project].Log.Max(a => a.id) + 1;
                    log.description = p.description;
                    log.date = DateTime.Now;
                    Projects.Instance.Project[project].Log.Add(log);                
                }
                Projects.Instance.Project[project].ToDo.Remove(p);
                Projects.Save();
                reloadItems();
            }
        }

        private void bnDeleteLog_Click(object sender, RoutedEventArgs e)
        {
            if (lbLog.SelectedItems.Count > 0)
            {
                Log p = lbLog.SelectedItem as Log;
                Projects.Instance.Project[project].Log.Remove(p);
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

        private void mnuRemoveLog_Click(object sender, RoutedEventArgs e)
        {
            bnDeleteLog_Click(sender, e);
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
                        if (Projects.Instance.Project[project].ToDo[i].id == note.id)
                        {

                            Projects.Instance.Project[project].ToDo[i].description = addEditNote.note;
                            Projects.Instance.Project[project].ToDo[i].caption = addEditNote.caption;
                            Projects.Save();
                            reloadItems();
                            break;
                        }
                    }
                }
            }
        }

        private void mnuEditLog_Click(object sender, RoutedEventArgs e)
        {
            Log log = lbLog.SelectedItem as Log;
            if (log != null)
            {
                AddEditNote addEditNote = new AddEditNote();
                addEditNote.caption = log.caption;
                addEditNote.note = log.description;


                if (addEditNote.ShowDialog() == true)
                {
                    for (int i = 0; i < Projects.Instance.Project[project].Log.Count; i++)
                    {
                        if (Projects.Instance.Project[project].Log[i].id == log.id)
                        {
                            Log l = Projects.Instance.Project[project].Log[i];
                            l.description = addEditNote.note;
                            l.caption = addEditNote.caption;
                            Projects.Instance.Project[project].Log.RemoveAt(i);
                            Projects.Instance.Project[project].Log.Insert(i, l);
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

        private void mnuAddLog_Click(object sender, RoutedEventArgs e)
        {
            bnAddLog_Click(sender, e);
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
                        if (Projects.Instance.Project[project].Notes[i].id == note.id)
                        {

                            Projects.Instance.Project[project].Notes[i].text = addEditNote.note;
                            Projects.Instance.Project[project].Notes[i].description = addEditNote.description;
                            Projects.Instance.Project[project].Notes[i].name = addEditNote.caption;                            
                            Projects.Save();
                            reloadItems();
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
                        for (int i = 0; i < Projects.Instance.Project[project].Files.Count; i++)
                        {
                            if (Projects.Instance.Project[project].Files[i] == (File)lbFiles.SelectedItem)
                            {
                                Projects.Instance.Project[project].Files[i] = p;
                                break;
                            }
                        }

                       
                        reloadItems();
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

                            for (int i = 0; i < Projects.Instance.Project[project].Apps.Count; i++)
                            {
                                if (Projects.Instance.Project[project].Apps[i] == (Program)lbApps.SelectedItem)
                                {
                                    Projects.Instance.Project[project].Apps[i] = p;
                                    break;
                                }
                            }
                            
                            reloadItems();
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

        private void lbApps_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (String file in files)
                {
                    if (file.EndsWith(".exe") || file.EndsWith(".bat") || file.EndsWith(".cmd") || file.EndsWith(".lnk") || file.EndsWith(".ps1"))
                    {
                        Program p = new Program();
                        p.executaleFile = file;
                        p.description = "";
                        MsgBox msgBox = new MsgBox("Please enter a title for " + System.IO.Path.GetFileName(file));
                        if (msgBox.ShowDialog()==true)
                        {
                            p.name = msgBox.ret;
                            if (Projects.Instance.Project[project].Apps.Count == 0)
                                p.id = 0;
                            else
                                p.id = Projects.Instance.Project[project].Apps.Max(a => a.id) + 1;

                            p.startOnce = false;
                            System.Drawing.Icon result = (System.Drawing.Icon)null;

                            result = System.Drawing.Icon.ExtractAssociatedIcon(file);
                            if (result != null)
                            {
                                ImageSource img = ToImageSource(result);
                                p.picture = img;
                                Projects.Instance.Project[project].Apps.Add(p);                       
                                Projects.Save();
                            }
                        }
                    }
                }
            }
        }

        private void lbFiles_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (String file in files)
                {
                    File p = new File();
                    p.id = Projects.Instance.Project[project].Files.Count == 0 ? 0 : Projects.Instance.Project[project].Files.Max(a => a.id) + 1;
                    p.fileName = file;
                    p.description = "";
                    MsgBox msgBox = new MsgBox("Please enter a title for " + System.IO.Path.GetFileName(file));
                    if (msgBox.ShowDialog() == true)
                    {
                        p.name = msgBox.ret;

                        p.startOnce = false;
                        System.Drawing.Icon result = (System.Drawing.Icon)null;
                        ImageSource img;

                        try
                        {
                            result = System.Drawing.Icon.ExtractAssociatedIcon(file);
                            img = ToImageSource(result);
                        }
                        catch
                        {
                            img = Imaging.CreateBitmapSourceFromHBitmap(new System.Drawing.Bitmap(System.Drawing.Image.FromFile(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\folder.png")).GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                        }

                        p.picture = img;
                        Projects.Instance.Project[project].Files.Add(p);
                        Projects.Save();
                    }
                }
            }
        }

		private void mnuHighToDo_Click(object sender, RoutedEventArgs e)
		{
			ToDo note = lbTodo.SelectedItem as ToDo;
			if (note != null)
			{
					for (int i = 0; i < Projects.Instance.Project[project].ToDo.Count; i++)
					{
						if (Projects.Instance.Project[project].ToDo[i].id == note.id)
						{

                            Projects.Instance.Project[project].ToDo[i].priority = 1;
							Projects.Save();
							reloadItems();
							break;
						}
					}
			}
		}

		private void mnuMediumToDo_Click(object sender, RoutedEventArgs e)
		{
			ToDo note = lbTodo.SelectedItem as ToDo;
			if (note != null)
			{
				for (int i = 0; i < Projects.Instance.Project[project].ToDo.Count; i++)
				{
					if (Projects.Instance.Project[project].ToDo[i].id == note.id)
					{

						Projects.Instance.Project[project].ToDo[i].priority = 2;
						Projects.Save();
						reloadItems();
						break;
					}
				}
			}
		}

		private void mnuLowToDo_Click(object sender, RoutedEventArgs e)
		{
			ToDo note = lbTodo.SelectedItem as ToDo;
			if (note != null)
			{
				for (int i = 0; i < Projects.Instance.Project[project].ToDo.Count; i++)
				{
					if (Projects.Instance.Project[project].ToDo[i].id == note.id)
					{

						Projects.Instance.Project[project].ToDo[i].priority = 3;
						Projects.Save();
						reloadItems();
						break;
					}
				}
			}
		}
	}
}
