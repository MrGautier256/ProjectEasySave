using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace ProjetConsole
{
    public class Model
    {
        public string sourcePath { get; set; } = string.Empty;
        public string targetPath { get; set; } = string.Empty;
        public string targetFile { get; set; } = string.Empty;
        public long totalSize { get; set; } = 0;
        public double countfile { get; set; } = 0;
        public double countSize { get; set; } = 0;
        private List<JsonData>? tableLog { get; set; } = new List<JsonData>();
        public int totalFileToCopy { get; set; } = 0;
        private Controller controller;

        public Model(Controller _controller)
        {
            this.controller = _controller;
        }

        // Calcul du nombre de fichier à copier
        // Calculating the number of files to copy
        public void setTotalFileToCopy() 
        {totalFileToCopy = Directory.GetFiles(sourcePath, ".", SearchOption.AllDirectories).Length;}

        // Calcul de la taille du contenu a sauvegarder 
        // Calculating the size of the contents to back-up
        public void setTotalSize(string sourceDir, string targetDir)
        {
            var dir = new DirectoryInfo(sourceDir);
            DirectoryInfo[] dirs = dir.GetDirectories();

            foreach (FileInfo file in dir.GetFiles())
            {
                totalSize += file.Length;

            }
            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(targetDir, subDir.Name);
                setTotalSize(subDir.FullName, newDestinationDir);

            }
        }

        // Sauvegarde des fichiers
        // Backing-up the files 
        public void saveFile(string sourceDir, string targetDir)
        {
            var dir = new DirectoryInfo(sourceDir);

            // Stock des dossiers dans un tableau
            // Cache directories in a array
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Création du dossier destination
            // Create the destination directory
            Directory.CreateDirectory(targetDir);

            //Récupération des fichiers dans le dossier source, copie vers le dossier destination, envoie des informations vers la view, ajout des informations à un objet de type JsonData et ajout à une liste de d'objets JsonData 
            // Get the files in the source directory, copy to the destination directory, send info to the view, add info to a JsonData object and add it to a list of JsonData objects
            foreach (FileInfo file in dir.GetFiles())
            {
                countfile++;
                countSize += file.Length;
                double percentage = Math.Round((countSize * 100) / totalSize, 2, MidpointRounding.AwayFromZero);

                string targetFilePath = Path.Combine(targetDir, file.Name);

                Stopwatch stopWatch = new Stopwatch();

                stopWatch.Start();
                file.CopyTo(targetFilePath, true);
                controller.sendProgressInfoToView(file.Name, countfile, this.totalFileToCopy, percentage);
                stopWatch.Stop();

                string ElapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    stopWatch.Elapsed.Hours, stopWatch.Elapsed.Minutes,
                    stopWatch.Elapsed.Seconds, stopWatch.Elapsed.Milliseconds / 100);

                JsonData jsonFileInfo = new JsonData(
                    targetFile,
                    file.Name,
                    file.FullName,
                    targetFilePath,
                    countfile,
                    this.totalFileToCopy,
                    file.Length,
                    this.totalFileToCopy - countfile,
                    percentage,
                    ElapsedTime
                    );
                tableLog.Add(jsonFileInfo);

            }

            // Si il y a des sous-dossiers, copie les sous-dossiers et appelle la méthode récursivement 
            // If there is subdirecories, copying subdirectories and recursively call this method
            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(targetDir, subDir.Name);
                saveFile(subDir.FullName, newDestinationDir);

            }
        }

        // Ecriture des logs contenus dans tableLog dans un fichier au format json dans C:/User/Utilisateur/Appdata/EasySave/Logs
        // Writing of the logs contained in tableLog in a json file in C:/User/Utilisateur/Appdata/EasySave/Logs
        public void writeLog()
        {
            string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string path = appdataPath + "/EasySave/Logs/";

            if (!Directory.Exists(path)){Directory.CreateDirectory(path);}

            string json = JsonConvert.SerializeObject(tableLog.ToArray());
            System.IO.File.AppendAllText(path + targetFile + " - " + DateTime.Now.ToString("MM.dd.yyyy") + ".json", json);
        }

    }
}