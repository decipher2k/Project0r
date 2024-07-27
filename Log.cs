using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjectOrganizer
{
    public class Log
    {
        public String caption;
        public String description;
        public DateTime date;
        public String dateString { get { return date.ToShortDateString(); } }
        public String captionString { get { return caption; } }

        public long id;
        public override String ToString()
        {
            return caption;
        }
    }
}
