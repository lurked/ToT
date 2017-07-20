using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace ToT
{
    public static class FileManager
    {
        public const string GAMEFILES_EXT_LEVEL = ".totl";
        public static string SerializeLevel(Level levelToSerialize)
        {
            string level;

            level = JsonConvert.SerializeObject(levelToSerialize);

            return level;
        }

        public static Level DeserializeLevel(string srlzdLevel)
        {
            Level tLevel = null;
            tLevel = JsonConvert.DeserializeObject<Level>(srlzdLevel);

            return tLevel;
        }

        public static Level LoadLevel(string path)
        {
            Level tLevel = null;
            tLevel = JsonConvert.DeserializeObject<Level>(File.ReadAllText(path));

            return tLevel;
        }

        public static void SaveToFile(string strToWrite, string path)
        {
            StreamWriter file = new System.IO.StreamWriter(path, false);
            file.WriteLine(strToWrite);

            file.Close();
        }

        public static List<string> GetSaves(string path)
        {
            DirectoryInfo d = new DirectoryInfo(path);
            List<string> levels = new List<string>();

            foreach (var file in d.GetFiles("*" + GAMEFILES_EXT_LEVEL))
                levels.Add(file.Name.Replace(GAMEFILES_EXT_LEVEL, ""));

            return levels;
        }

    }
}
