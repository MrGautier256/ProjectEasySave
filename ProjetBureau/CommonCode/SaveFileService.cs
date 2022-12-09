using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CommonCode
{
    public class SaveFileService : ISaveFileServiceCommand
    {
        public SaveFileService(string sourceDir, string targetDir, string targetFile)
        {
            SourceDir = sourceDir;
            TargetDir = targetDir;
            TargetFile = targetFile;
        }

        public string SourceDir { get; }
        public string TargetDir { get; }
        public string TargetFile { get; }

        public event Action<string, double, int, double>? ProgressEvent;

        public event Action<List<JsonData>,bool>? Finished;

        #region Private fields
        private ProgressState _progressState = ProgressState.play;
        private List<JsonData> tableLogs = new();
        private Thread? _threadCopy;
        private ManualResetEvent _manualResetEvent = new ManualResetEvent(true);
        #endregion Private fields

        public void Pause()
        {
            if (_threadCopy == null)
                throw new Exception("Cannot call Pause before starting");
            if (_progressState != ProgressState.play)
                throw new Exception("Cannot call Pause if state is not Playing");
            _progressState = ProgressState.pause;
            _manualResetEvent.Reset();
        }

        public void Resume()
        {
            if (_threadCopy == null)
                throw new Exception("Cannot call Resume before starting");
            if (_progressState != ProgressState.pause)
                throw new Exception("Cannot call Resume if state is not Paused");
            _progressState = ProgressState.play;
            _manualResetEvent.Set();
        }

        public void Stop()
        {
            if (_threadCopy == null)
                throw new Exception("Cannot call Stop before starting");

            _progressState = ProgressState.stop;
        }

        private void CallFinished(bool finishedNormaly)
        {
            if (Finished != null)
                Finished(tableLogs, finishedNormaly);
        }

        private void CallProgressEvent(string fileName, double countfile, int totalFileToCopy, double percentage)
        {
            if (ProgressEvent != null)
                ProgressEvent(fileName, countfile, totalFileToCopy, percentage);
        }

        public void Start()
        {
            _threadCopy = new Thread(() =>
            {
                var dir = new DirectoryInfo(SourceDir);
                long totalSize = 0;
                var files = dir.GetFiles("*", SearchOption.AllDirectories);
                var dirs = dir.GetDirectories("*", SearchOption.AllDirectories);
                double countFile = 0;
                double countSize = 0;

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
                Directory.CreateDirectory(TargetDir);

                //Récupération des fichiers dans le dossier source, copie vers le dossier destination, envoie des informations vers la view, ajout des informations à un objet de type JsonData et ajout à une liste de d'objets JsonData 
                // Get the files in the source directory, copy to the destination directory, send info to the view, add info to a JsonData object and add it to a list of JsonData objects
                foreach (FileInfo file in files)
                
                {
                    if (_progressState == ProgressState.stop)
                    {
                        CallFinished(false);
                        return;
                    }
                    _manualResetEvent.WaitOne();
                    countFile++;
                    countSize += file.Length;
                    double percentage = Math.Round((countSize * 100) / totalSize, 2, MidpointRounding.AwayFromZero);

                    string? fileDirectoryName = file.DirectoryName;
                    string? targetFilePath = fileDirectoryName?.Replace(SourceDir, TargetDir);

                    if (targetFilePath == null) 
                        throw new Exception(Traduction.Instance.Langue.TargetPathInvalid); 

                    if (!Directory.Exists(targetFilePath))  
                        Directory.CreateDirectory(targetFilePath); 

                    targetFilePath = Path.Combine(targetFilePath, file.Name);
                    string ElapsedTime = ChronoTimer.Chrono(() =>
                    {
                        file.CopyTo(targetFilePath, true);
                        CallProgressEvent(file.Name, countFile, totalFileToCopy, percentage);
                    });

                    JsonData jsonFileInfo = new(
                        TargetFile,
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
                    tableLogs.Add(jsonFileInfo);
                }

                CallFinished(true);
            });
            _threadCopy.Start();
        }
    }
}
