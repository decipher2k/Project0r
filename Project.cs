using Orchestra;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ProjectOrganizer
{
    [Serializable]
    public class Project
    {
        public double x { get; set; } = 0;
        public double y { get; set; } = 0; 

        public static Project Instance;
        public Dictionary<String, Data> Projects=new Dictionary<string, Data>();
        public Project()
        {
            Instance = this;
        }

        public static void Save()
        {

            Instance.x=FloatingWindow.Instance.Left;
            Instance.y =FloatingWindow.Instance.Top;

            foreach (String project in Instance.Projects.Keys)
            {
                for (int i = 0; i < Project.Instance.Projects[project].Apps.Count; i++)
                {
                    var app = Project.Instance.Projects[project].Apps[i];
                    String file = app.executaleFile;                                                         
                    app.picture = null;
                }
            }

            foreach (String project in Instance.Projects.Keys)
            {
                for (int i = 0; i < Project.Instance.Projects[project].Files.Count; i++)
                {
                    var app = Project.Instance.Projects[project].Files[i];
                    String file = app.fileName;
                    app.picture = null;
                }
            }


            if (!Directory.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\Project0r"))
            {
                Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\Project0r");
            }
            String text=Newtonsoft.Json.JsonConvert.SerializeObject(Instance);
            System.IO.File.WriteAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData)+"\\Project0r\\settings.json", text);
            foreach (String project in Instance.Projects.Keys)
            {
                for (int i = 0; i < Project.Instance.Projects[project].Apps.Count; i++)
                {
                    var app = Project.Instance.Projects[project].Apps[i];
                    String file = app.executaleFile;

                    Icon result = (Icon)null;
                    result = Icon.ExtractAssociatedIcon(file);
                    if (result != null)
                    {
                        ImageSource img = result.ToImageSource();
                        app.picture = img;
                        Project.Instance.Projects[project].Apps[i] = app;
                    }
                }

                for (int i = 0; i < Project.Instance.Projects[project].Files.Count; i++)
                {
                    var file = Project.Instance.Projects[project].Files[i];                    

                    Icon result = (Icon)null;
                    result = Icon.ExtractAssociatedIcon(file.fileName);
                    if (result != null)
                    {
                        ImageSource img = result.ToImageSource();
                        file.picture = img;
                        Project.Instance.Projects[project].Files[i] = file;
                    }
                }
            }
        }

        public static void Load()
        {            
            if (System.IO.File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\Project0r\\settings.json"))
            {
                String text = System.IO.File.ReadAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\Project0r\\settings.json");
                Instance = (Project)Newtonsoft.Json.JsonConvert.DeserializeObject<Project>(text);

                foreach (String project in Instance.Projects.Keys)
                {
                    for (int i = 0; i < Project.Instance.Projects[project].Apps.Count; i++)
                    {
                        var app = Project.Instance.Projects[project].Apps[i];
                        String file = app.executaleFile;

                        Icon result = (Icon)null;
                        result = Icon.ExtractAssociatedIcon(file);
                        if (result != null)
                        {
                            ImageSource img = result.ToImageSource();
                            app.picture = img;
                            Project.Instance.Projects[project].Apps[i] = app;                            
                        }
                    }

                    for (int i = 0; i < Project.Instance.Projects[project].Files.Count; i++)
                    {
                        var app = Project.Instance.Projects[project].Files[i];
                        String file = app.fileName;

                        Icon result = (Icon)null;
                        result = Icon.ExtractAssociatedIcon(file);
                        if (result != null)
                        {
                            ImageSource img = result.ToImageSource();
                            app.picture = img;
                            Project.Instance.Projects[project].Files[i] = app;
                        }
                    }
                }
             
            }
            else
            {
                Instance = new Project();
            }
        }
    }
}
