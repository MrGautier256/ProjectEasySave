using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace CommonCode
{
    /// <summary>
    ///  Classe Controller
    ///  Class Controller
    /// </summary>

    public class Controller : IController
    {
        private readonly Model model;
        private readonly IView view;
        public Controller(IView _view)
        {
            model = new Model();
            view = _view;
        }

        /// <summary>
        /// Execution sucessive des différentes fonctions necessaire au processus de sauvegarde
        /// Sucessive execution of the multiple functions necessary to the process of back-up 
        /// </summary>
        public void execute()
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
                var tableLog = SaveService.SaveFile(model.sourcePath, model.targetPath, model.targetFile,view.ControlProgress);
                SaveService.WriteLog(tableLog, model.targetFile, model.logType);
                view.Progress(true);
            }
        }

        private static string CombinePathAndName(string targetPath, string saveName)
        {
            return Path.Combine(targetPath, saveName);
        }

        private static string SetLogType(string logtype)
        {
            if (logtype == "xml") {return "xml";}
            else{return "json";}
        }


        /// <summary>
        /// Regex verifiant la validité du nom de sauvegarde
        /// Regex checking the validity of the Back-up's name
        /// </summary>
        /// <param name="DirName"></param>
        /// <returns></returns>
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
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
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
        /// <param name="path"></param>
        /// <param name="allowRelativePaths"></param>
        /// <returns></returns>
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