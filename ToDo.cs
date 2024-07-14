using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjectOrganizer
{
    public class ToDo
    {
        public String caption;
        public String description;

        public override String ToString()
        {
            return caption;
        }
    }
}
