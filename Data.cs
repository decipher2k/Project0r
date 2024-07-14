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
    }
}
