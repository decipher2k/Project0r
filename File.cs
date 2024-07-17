using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ProjectOrganizer
{
    [Serializable]
    public class File
    {
        public String name { get; set; }
        public String description { get; set; }
        public String fileName { get; set; }

        public ImageSource picture { get; set; }
        public bool startOnce { get; set; }

        [NonSerialized]
        public Process process;
        public long id;
        public override String ToString()
        {
            return name;
        }

    }
}
