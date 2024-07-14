using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOrganizer
{
    [Serializable]
    public class File
    {
        public String name { get; set; }
        public String description { get; set; }
        public String file { get; set; }
        public override String ToString()
        {
            return name;
        }

    }
}
