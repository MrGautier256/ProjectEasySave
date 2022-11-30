using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProjetConsole
{
    public class Model
    {
        public string sourcePath { get; set; } = string.Empty;
        public string targetPath { get; set; } = string.Empty;
        public string targetFile { get; set; } = string.Empty;
        public string logType { get; set; } = string.Empty;
        public double countfile { get; set; } = 0;
        public double countSize { get; set; } = 0;
        private List<JsonData> tableLog { get; set; } = new List<JsonData>();
        private Controller controller;

        public Model(Controller _controller)
        {
            this.controller = _controller;
        }


        // Sauvegarde des fichiers
        // Backing-up the files 
        public void saveFile(string sourceDir, string targetDir)
        {
            var dir = new DirectoryInfo(sourceDir);
            long totalSize = 0;
            var files = dir.GetFiles("*", SearchOption.AllDirectories);
            var dirs = dir.GetDirectories("*", SearchOption.AllDirectories);

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
                countfile++;
                countSize += file.Length;
                double percentage = Math.Round((countSize * 100) / totalSize, 2, MidpointRounding.AwayFromZero);

                string? fileDirectoryName = file.DirectoryName;
                string? targetFilePath = fileDirectoryName?.Replace(sourceDir, targetDir);

                if (!Directory.Exists(targetFilePath)) { Directory.CreateDirectory(targetFilePath); }

                targetFilePath = Path.Combine(targetFilePath, file.Name);
                string ElapsedTime = ChronoTimer.Chrono(() =>
                {
                    file.CopyTo(targetFilePath, true);
                });

                controller.sendProgressInfoToView(file.Name, countfile, totalFileToCopy, percentage);

                JsonData jsonFileInfo = new JsonData(
                    targetFile,
                    file.Name,
                    file.FullName,
                    targetFilePath,
                    countfile,
                    totalFileToCopy,
                    file.Length,
                    totalFileToCopy - countfile,
                    percentage,
                    ElapsedTime
                    );
                tableLog.Add(jsonFileInfo);

            }
        }

        // Ecriture des logs contenus dans tableLog dans un fichier au format jsonFile dans C:/User/Utilisateur/Appdata/EasySave/Logs
        // Writing of the logs contained in tableLog in a jsonFile file in C:/User/Utilisateur/Appdata/EasySave/Logs
        public void writeLog()
        {
            string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string path = $"{appdataPath}\\EasySave\\Logs\\";
            string fileFullName = $"{path}{targetFile} - {DateTime.Now.ToString("MM.dd.yyyy")}";
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

            string jsonFile = JsonConvert.SerializeObject(tableLog.ToArray());
            int i = 0;
            while (File.Exists(fileFullName+
                ".json") || File.Exists(fileFullName + ".xml"))
            {
                i++;
                fileFullName = $"{path}{targetFile}{i} - {DateTime.Now.ToString("MM.dd.yyyy")}";
            }
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

    }
}