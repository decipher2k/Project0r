using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOrganizer
{
    [Serializable]
    public class Data
    {
        private ObservableCollection<File> files;
        public ObservableCollection<File> Files
        {
            get
            {
                if (files == null)
                    files = new ObservableCollection<File>();
                return files;
            }
        }

        private ObservableCollection<Program> apps;
        public ObservableCollection<Program> Apps
        {
            get
            {
                if (apps == null)
                    apps = new ObservableCollection<Program>();
                return apps;
            }
        }

        private ObservableCollection<Note> notes;
        public ObservableCollection<Note> Notes
        {
            get
            {
                if (notes == null)
                    notes = new ObservableCollection<Note>();
                return notes;
            }
        }

        private ObservableCollection<ToDo> todo;
        public ObservableCollection<ToDo> ToDo
        {
            get
            {
                if (todo == null)
                    todo = new ObservableCollection<ToDo>();
                return todo;
            }
        }

        private ObservableCollection<Calendar> calendar;
        public ObservableCollection<Calendar> Calendar
        {
            get
            {
                if (calendar == null)
                    calendar = new ObservableCollection<Calendar>();
                return calendar;
            }
        }

        private ObservableCollection<Log> log;
        public ObservableCollection<Log> Log
        {
            get
            {
                if (log == null)
                    log = new ObservableCollection<Log>();
                return log;
            }
            set
            {
                log=(ObservableCollection<Log>)value;
            }
        }

    }
}
