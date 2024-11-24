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
        public int priority = 99;
        public long id;
        public override String ToString()
        {
            String token = "";
            if (priority == 1)
                token = "[H] ";
            else if (priority == 2)
                token = "[M] ";
            else if (priority == 3)
                token = "[L] ";
            return token+caption;
        }
    }
}
