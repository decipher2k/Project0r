using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOrganizer
{
    [Serializable]
    public class Note
    {
        public String name { get; set; }
        public String description { get; set; }
        public String text { get; set; }
        public long id;
        public override String ToString()
        {
            return text;
        }
    }
}
