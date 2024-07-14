using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOrganizer
{
    [Serializable]
    public class Project
    {
        public static Project Instance;
        public Dictionary<String, Data> Projects=new Dictionary<string, Data>();
        public Project()
        {
            Instance = this;
        }
    }
}
