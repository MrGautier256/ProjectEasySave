﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace CommonCode
{
    public class Controller 
    {
        private readonly Model model;
        private readonly IView view;
        private readonly IViewProgress remoteView;
        private SaveFileService? _saveFileService;
        public Controller(IView _view, IViewProgress _remoteView)
        {
            model = new Model();
            view = _view;
            remoteView = _remoteView;
        }

        /// <summary>
        /// Execution sucessive des différentes fonctions necessaire au processus de sauvegarde
        /// Sucessive execution of the multiple functions necessary to the process of backup 
        /// </summary>
        public void Execute()
        {
            var language = view.AskLanguage();
            Traduction.SetLanguage(language);
            string logType = view.AsklogType();

            string saveName = view.AskTargetFile();
            string sourcePath = view.AskSourcePath();
            string targetPath = view.AskTargetPath();

            targetPath = CombinePathAndName(targetPath, saveName);

            if (CheckPathIntegrity(sourcePath, targetPath) && !CheckTargetDirectory(saveName))
            {
                model.logType = SetLogType(logType);
                model.sourcePath = sourcePath;
                model.targetPath = targetPath;
                model.targetFile = saveName;

                view.Progress(false);
                remoteView.Progress(false);

                _saveFileService = new SaveFileService(model.sourcePath, model.targetPath, model.targetFile);
                _saveFileService.Finished += SaveFileService_Finished;
                _saveFileService.ProgressEvent += remoteView.ControlProgress;
                _saveFileService.ProgressEvent += view.ControlProgress;
                view.SendSaveFileServiceCommand(_saveFileService);
                _saveFileService.Start();
            }
        }

        /// <summary>
        /// Méthode appelant la méthode d'écriture de log et indiquant la fin du processus de sauvegarde
        /// Method calling the write log method and indicating the end of the backup process
        /// </summary>
        private void SaveFileService_Finished(List<JsonData> tableLog, bool finishedNormaly)
        {
            SaveService.WriteLog(tableLog, model.targetFile, model.logType);
            view.Progress(true);
            remoteView.Progress(true);

            _saveFileService.Finished -= SaveFileService_Finished;
            _saveFileService.ProgressEvent -= view.ControlProgress;
        }

        /// <summary>
        /// Combinez le chemin et le nom de la sauvegarde pour créer le répertoire de la sauvegarde
        /// Combine Path and save name to create directory of the save
        /// </summary>
        private static string CombinePathAndName(string targetPath, string saveName)
        {
            return Path.Combine(targetPath, saveName);
        }

        /// <summary>
        /// Définition du type de log choisis par l'utilisateur
        /// Setting of the type of log chosen by the user
        /// </summary>
        private static string SetLogType(string logtype)
        {
            if (logtype == "xml") {return "xml";}
            else{return "json";}
        }

        /// <summary>
        /// Regex verifiant la validité du nom de sauvegarde
        /// Regex checking the validity of the Back-up's name
        /// </summary>
        private bool CheckTargetDirectory(string DirName)
        {
            bool valid;
            Regex RgxUrl = new("[^a-zA-Z0-9 ]");
            if (DirName.Length >= 60 || RgxUrl.IsMatch(DirName))
            {
                view.TargetDirInvalid();
                valid = true;
            }
            else
            { valid = false; }

            return valid;
        }

        /// <summary>
        /// Execution des différentes fonctions de vérification de chemin
        /// Execution of the different path control functions 
        /// </summary>
        private bool CheckPathIntegrity(string source, string target)
        {
            bool integrity;
            if (PathIsValid(source) && Directory.Exists(source))
            {
                if (PathIsValid(target) && (target != ""))
                {
                    integrity = true;
                }
                else
                {
                    view.TargetPathIsInvalid();
                    integrity = false;
                }
            }
            else
            {
                view.SourcePathIsInvalid();
                integrity = false;
            }
            return integrity;
        }

        /// <summary>
        /// Verification de la validité du format du chemin
        /// Verifying the validity of the path format
        /// </summary>
        private static bool PathIsValid(string path, bool allowRelativePaths = false)
        {
            bool isValid;
            try
            {
                string fullPath = Path.GetFullPath(path);

                if (allowRelativePaths)
                {
                    isValid = Path.IsPathRooted(path);
                }
                else
                {
                    string? root = Path.GetPathRoot(path);
                    isValid = string.IsNullOrEmpty(root?.Trim(new char[] { '\\', '/' })) == false;
                }
            }
            catch
            {
                isValid = false;
            }
            return isValid;
        }
    }
}