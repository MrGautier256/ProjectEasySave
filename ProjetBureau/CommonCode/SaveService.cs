using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System;

namespace CommonCode
{
    public enum ProgressState { play, pause, stop };
    public static class SaveService 
    {
        /// <summary>
        /// Sauvegarde des fichiers
        /// Backing-up the files 
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="targetDir"></param>

        public static List<JsonData> SaveFile(string sourceDir, string targetDir, string targetFile, Func<string, double, int, double, ProgressState> controlProgress)
        {
            var dir = new DirectoryInfo(sourceDir);
            long totalSize = 0;
            var files = dir.GetFiles("*", SearchOption.AllDirectories);
            var dirs = dir.GetDirectories("*", SearchOption.AllDirectories);
            double countFile = 0;
            double countSize = 0;
            List<JsonData> tableLog = new();

            // Calcul de la taille du contenu a sauvegarder  
            // Calculating the size of the contents to back-up 
            foreach (FileInfo file in files)
            {
                totalSize += file.Length;
            }

            // Calcul du nombre de fichier à copier
            // Calculating the number of files to copy
            int totalFileToCopy = files.Length;

            // Création du dossier destination
            // Create the destination directory
            Directory.CreateDirectory(targetDir);

            //Récupération des fichiers dans le dossier source, copie vers le dossier destination, envoie des informations vers la view, ajout des informations à un objet de type JsonData et ajout à une liste de d'objets JsonData 
            // Get the files in the source directory, copy to the destination directory, send info to the view, add info to a JsonData object and add it to a list of JsonData objects
            foreach (FileInfo file in files)
            {
                countFile++;
                countSize += file.Length;
                double percentage = Math.Round((countSize * 100) / totalSize, 2, MidpointRounding.AwayFromZero);

                string? fileDirectoryName = file.DirectoryName;
                string? targetFilePath = fileDirectoryName?.Replace(sourceDir, targetDir);

                if (targetFilePath == null) { throw new Exception("target file fath is empty"); }

                if (!Directory.Exists(targetFilePath)) { Directory.CreateDirectory(targetFilePath); }
                
                targetFilePath = Path.Combine(targetFilePath, file.Name);
                string ElapsedTime = ChronoTimer.Chrono(() =>
                {
                    file.CopyTo(targetFilePath, true);
                    //Thread threadCopy = new Thread(() => file.CopyTo(targetFilePath, true));
                    //threadCopy.Start();
                });

                if (controlProgress != null) 
                {
                    var result = controlProgress(file.Name, countFile, totalFileToCopy, percentage);
                    if (result == ProgressState.stop)
                    {
                        break;
                    }
                    else if (result == ProgressState.pause)
                    {
                        while (result == ProgressState.pause)
                        {result = controlProgress(file.Name, countFile, totalFileToCopy, percentage); Thread.Sleep(100); }
                    }
                }

                JsonData jsonFileInfo = new(
                    targetFile,
                    file.Name,
                    file.FullName,
                    targetFilePath,
                    countFile,
                    totalFileToCopy,
                    file.Length,
                    totalFileToCopy - countFile,
                    percentage,
                    ElapsedTime
                    );
                tableLog.Add(jsonFileInfo);
            }
            return tableLog;
        }
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