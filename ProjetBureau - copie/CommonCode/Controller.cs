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
        private Model model;
        private IView view;
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
            var language = view.askLanguage();
            Traduction.Instance.setLanguage(language);
            string logType = view.asklogType();

            string saveName = view.askTargetFile();
            string sourcePath = view.askSourcePath();
            string targetPath = view.askTargetPath();

            targetPath = combinePathAndName(targetPath, saveName);

            if (checkPathIntegrity(sourcePath, targetPath) && !checkTargetDirectory(saveName))
            {
                model.logType = setLogType(logType);
                model.sourcePath = sourcePath;
                model.targetPath = targetPath;
                model.targetFile = saveName;
                view.progress(false);
                var tableLog = SaveService.saveFile(model.sourcePath, model.targetPath, model.targetFile, model.logType,view.controlProgress);
                SaveService.writeLog(tableLog, model.targetFile, model.logType);
                view.progress(true);
            }
        }

        private string combinePathAndName(string targetPath, string saveName)
        {
            return Path.Combine(targetPath, saveName);
        }

        private string setLogType(string logtype)
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

        private bool checkTargetDirectory(string DirName)
        {
            bool valid;
            Regex RgxUrl = new Regex("[^a-zA-Z0-9 ]");
            if (RgxUrl.IsMatch(DirName))
            {
                view.targetDirInvalid();
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

        private bool checkPathIntegrity(string source, string target)
        {
            bool integrity = false;
            if (pathIsValid(source) && Directory.Exists(source))
            {
                if (pathIsValid(target) && (target != ""))
                {
                    integrity = true;
                }
                else
                {
                    view.targetPathIsInvalid();
                    integrity = false;

                }
            }
            else
            {
                view.sourcePathIsInvalid();
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
        

        private bool pathIsValid(string path, bool allowRelativePaths = false)
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
        ~Controller() { }

    }
}