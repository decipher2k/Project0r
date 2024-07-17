using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProjectOrganizer
{
    [Serializable]
    public class Projects
    {
        public double x { get; set; } = 0;
        public double y { get; set; } = 0; 

        public static Projects Instance;
        public Dictionary<String, Data> Project=new Dictionary<string, Data>();
        public Projects()
        {
            Instance = this;
        }

        public static void Save()
        {

            Instance.x=FloatingWindow.Instance.Left;
            Instance.y =FloatingWindow.Instance.Top;

            foreach (String project in Instance.Project.Keys)
            {
                for (int i = 0; i < ProjectOrganizer.Projects.Instance.Project[project].Apps.Count; i++)
                {
                    var app = ProjectOrganizer.Projects.Instance.Project[project].Apps[i];
                    String file = app.executaleFile;                                                         
                    app.picture = null;
                }
            }

            foreach (String project in Instance.Project.Keys)
            {
                for (int i = 0; i < ProjectOrganizer.Projects.Instance.Project[project].Files.Count; i++)
                {
                    var app = ProjectOrganizer.Projects.Instance.Project[project].Files[i];
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
            foreach (String project in Instance.Project.Keys)
            {
                for (int i = 0; i < ProjectOrganizer.Projects.Instance.Project[project].Apps.Count; i++)
                {
                    var app = ProjectOrganizer.Projects.Instance.Project[project].Apps[i];
                    String file = app.executaleFile;

                    Icon result = (Icon)null;
                    try
                    {
                        result = Icon.ExtractAssociatedIcon(file);
                        if (result != null)
                        {
                            ImageSource img = ToImageSource(result);
                            app.picture = img;
                            ProjectOrganizer.Projects.Instance.Project[project].Apps[i] = app;
                        }
                    }
                    catch (Exception ex) { }
                }

                

                for (int i = 0; i < ProjectOrganizer.Projects.Instance.Project[project].Files.Count; i++)
                {
                    var file = ProjectOrganizer.Projects.Instance.Project[project].Files[i];

                    Icon result = (Icon)null;
                    ImageSource img;

                    try
                    {
                        result = Icon.ExtractAssociatedIcon(file.fileName);
                        img = ToImageSource(result);
                    }
                    catch
                    {
                        img = Imaging.CreateBitmapSourceFromHBitmap(new System.Drawing.Bitmap(System.Drawing.Image.FromFile(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\folder.png")).GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    }
                                                        
                    file.picture = img;
                    ProjectOrganizer.Projects.Instance.Project[project].Files[i] = file;
                    
                }
            }
        }
        public static ImageSource ToImageSource(Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }
        public static void Load()
        {            
            if (System.IO.File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\Project0r\\settings.json"))
            {
                String text = System.IO.File.ReadAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\Project0r\\settings.json");
                Instance = (Projects)Newtonsoft.Json.JsonConvert.DeserializeObject<Projects>(text);

                foreach (String project in Instance.Project.Keys)
                {
                    for (int i = 0; i < ProjectOrganizer.Projects.Instance.Project[project].Apps.Count; i++)
                    {
                        var app = ProjectOrganizer.Projects.Instance.Project[project].Apps[i];
                        String file = app.executaleFile;

                        Icon result = (Icon)null;
                        try
                        {
                            result = Icon.ExtractAssociatedIcon(file);
                            if (result != null)
                            {
                                ImageSource img = ToImageSource(result);
                                app.picture = img;
                                ProjectOrganizer.Projects.Instance.Project[project].Apps[i] = app;
                            }
                        }
                        catch (Exception ex) { }
                    }

                    for (int i = 0; i < ProjectOrganizer.Projects.Instance.Project[project].Files.Count; i++)
                    {
                        var app = ProjectOrganizer.Projects.Instance.Project[project].Files[i];
                        String file = app.fileName;

                        Icon result = (Icon)null;

                        ImageSource img;
                        try
                        {
                            result = Icon.ExtractAssociatedIcon(file);
                            img = ToImageSource(result);
                        }
                        catch
                        {
                            img = Imaging.CreateBitmapSourceFromHBitmap(new System.Drawing.Bitmap(System.Drawing.Image.FromFile(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\folder.png")).GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                        }

                        app.picture = img;
                        ProjectOrganizer.Projects.Instance.Project[project].Files[i] = app;                        
                    }
                }
             
            }
            else
            {
                Instance = new Projects();
            }
        }

    }
}
