using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOrganizer
{
    public class Calendar
    {
        public DateTime date;
        public DateTime from;
        public DateTime to;
        public String text;
        public String caption;
        public bool handled;
        public long id;
        

        public override string ToString()
        {
            return caption;
        }
    }
}
