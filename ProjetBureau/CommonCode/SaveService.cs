using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System;
using System.IO;

namespace CommonCode
{
    public enum ProgressState { play, pause, stop };
    public static class SaveService
    {

        /// <summary>
        /// Ecriture des logs contenus dans tableLog dans un fichier au format jsonFile dans C:/User/Utilisateur/Appdata/EasySave/Logs
        /// Writing of the logs contained in tableLog in a jsonFile file in C:/User/Utilisateur/Appdata/EasySave/Logs
        /// </summary>

        public static void WriteLog(List<JsonData> tableLog, string targetFile, string logType)
        {
            string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string path = $"{appdataPath}\\EasySave\\Logs\\";
            string fileFullName = $"{path}{targetFile} - {DateTime.Now:MM.dd.yyyy}";
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

            string jsonFile = JsonConvert.SerializeObject(tableLog.ToArray());
            fileFullName = RenameLogFile(path, fileFullName, targetFile);
            if (logType == "xml")
            {
                var xml = XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(
                Encoding.ASCII.GetBytes(jsonFile), new XmlDictionaryReaderQuotas()));
                string xmlFile = xml.ToString();
                System.IO.File.AppendAllText($"{fileFullName}.xml", xmlFile);
            }
            else
            {
                System.IO.File.AppendAllText($"{fileFullName}.json", jsonFile);

            }
        }

        private static string RenameLogFile(string path, string fileFullName, string targetFile)
        {
            int i = 0;
            while (File.Exists(fileFullName + ".json") || File.Exists(fileFullName + ".xml"))
            {
                i++;
                fileFullName = $"{path}{targetFile}{i} - {DateTime.Now:MM.dd.yyyy}";
            }

            return fileFullName;
        }
    }
}