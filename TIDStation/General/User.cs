using System.IO;

namespace TIDStation.General
{
    public static class User
    {
        static User()
        {
            if(!Directory.Exists(ConfigFolder))
                Directory.CreateDirectory(ConfigFolder);
        }
        public static string Documents => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string ConfigFolder => Path.Combine(Documents, "TIDStation");
        public static string ConfigFile => Path.Combine(ConfigFolder, "settings.conf");
    }
}
