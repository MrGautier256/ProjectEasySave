using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;

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

        public event Action<string, int, int, double>? ProgressEvent;

        public event Action<List<JsonData>, bool>? Finished;

        private ProgressState _progressState = ProgressState.play;
        private List<JsonData> tableLogs = new();
        private Thread? _threadCopy;
        private ManualResetEvent _manualResetEvent = new ManualResetEvent(true);

        /// <summary>
        /// Fonction appelée lors de la mise en pause de l'application
        /// Function called when app is paused
        /// </summary>
        public void Pause()
        {
            if (_threadCopy == null)
                throw new Exception("Cannot call Pause before starting");
            if (_progressState != ProgressState.play)
                throw new Exception("Cannot call Pause if state is not Playing");
            _progressState = ProgressState.pause;
            _manualResetEvent.Reset();
        }

        /// <summary>
        /// Fonction appelée lors de la reprise de l'application
        /// Function called when app is resumed
        /// </summary>
        public void Resume()
        {
            if (_threadCopy == null)
                throw new Exception("Cannot call Resume before starting");
            if (_progressState != ProgressState.pause)
                throw new Exception("Cannot call Resume if state is not Paused");
            _progressState = ProgressState.play;
            _manualResetEvent.Set();
        }

        /// <summary>
        /// Fonction appelée lors de l'interruption de l'application
        /// Function called when app is stopped
        /// </summary>
        public void Stop()
        {
            if (_threadCopy == null)
                throw new Exception("Cannot call Stop before starting");
            _manualResetEvent.Set();
            _progressState = ProgressState.stop;
        }

        /// <summary>
        /// Appelle l'évenement indiquant la fin de la copie
        /// Calls the event indicating the end of the copy
        /// </summary>
        private void CallFinished(bool finishedNormaly)
        {
            if (Finished != null)
                Finished(tableLogs, finishedNormaly);
        }


        /// <summary>
        /// Appelle l'évenement de progression
        /// Calls the progress event 
        /// </summary>
        private void CallProgressEvent(string fileName, int countfile, int totalFileToCopy, double percentage)
        {
            if (ProgressEvent != null)
                ProgressEvent(fileName, countfile, totalFileToCopy, percentage);
        }

        /// <summary>
        /// Fonction effectuant diverses actions pour la sauvegarde (creation des dossiers, copie et chiffrement, creation des logs)
        /// Function performing various actions for the backup (creation of folders, copy and encryption, creation of logs)
        /// </summary>
        public void Start()
        {
            _threadCopy = new Thread(() =>
            {
                var dir = new DirectoryInfo(SourceDir);
                long totalSize = 0;
                var files = dir.GetFiles("*", SearchOption.AllDirectories);
                int countFile = 1;
                long countSize = 0;
                object locking = new object();
                bool finishedNormally = true;

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
                Parallel.ForEach(files, new ParallelOptions { MaxDegreeOfParallelism = 4 }, (FileInfo file, ParallelLoopState state) =>
                {
                    if (_progressState == ProgressState.stop)
                    {
                        finishedNormally = false;
                        state.Break();
                        return;
                    }
                    _manualResetEvent.WaitOne();

                    string? fileDirectoryName = file.DirectoryName;
                    string? targetFilePath = fileDirectoryName?.Replace(SourceDir, TargetDir);

                    if (targetFilePath == null)
                        throw new Exception(Traduction.Instance.Langue.TargetPathInvalid);

                    if (!Directory.Exists(targetFilePath))
                        Directory.CreateDirectory(targetFilePath);

                    int countFileTemp = 0;
                    double percentage = 0;

                    targetFilePath = Path.Combine(targetFilePath, file.Name);

                    string ElapsedTime = ChronoTimer.Chrono(() =>
                    {
                        //file.CopyTo(targetFilePath, true);
                        SaveService.EncryptFile(file.FullName, targetFilePath);
                    });
                    lock (locking)
                    {
                        countFileTemp = countFile++;
                        countSize += file.Length;
                        percentage = Math.Round((double)countSize * 100 / totalSize, 2, MidpointRounding.AwayFromZero);
                        JsonData jsonFileInfo = new
                        (
                            TargetFile,
                            file.Name,
                            file.FullName,
                            targetFilePath,
                            countFileTemp,
                            totalFileToCopy,
                            file.Length,
                            totalFileToCopy - countFileTemp,
                            percentage,
                            ElapsedTime
                        );
                        tableLogs.Add(jsonFileInfo);
                        Debug.WriteLine(percentage);
                    }
                    CallProgressEvent(file.FullName, countFileTemp, totalFileToCopy, percentage);

                });

                CallFinished(finishedNormally);
            });
            _threadCopy.Start();
        }
    }
}
