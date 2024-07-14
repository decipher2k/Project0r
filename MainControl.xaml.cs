using Microsoft.Win32;
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
        }

       

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Program p = (Program)((ListBox)sender).SelectedItem;
            Process.Start(p.executaleFile);
        }

        
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        


        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddEditFile wnd = new AddEditFile();
            if(wnd.ShowDialog()==true)
            {
                Program p=new Program();
                p.executaleFile = wnd.file;
                p.description=wnd.description;
                p.name = wnd.name;
                Icon result = (Icon)null;

                result =  Icon.ExtractAssociatedIcon(wnd.file);
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
    }
}
