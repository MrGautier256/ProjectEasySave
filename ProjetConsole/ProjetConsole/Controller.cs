using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ProjetConsole
{
    // Interface Icontroller avec comme méthode execute() commune a tout controller
    // Icontroller interface with execute() as shared method to all the controllers
    public interface IController
    {
        public void execute();
    }

    // Classe Controller
    // Class Controller
    public class Controller : IController
    {
        private Model model;
        private View view;
        ~Controller() { }
        public Controller()
        {
            model = new Model(this);
            view = new View();
        }

        /// <summary>
        /// Execution sucessive des différentes fonctions necessaire au processus de sauvegarde
        /// Sucessive execution of the multiple functions necessary to the process of back-up 
        /// </summary>
        public void execute()
        {
            var language = view.askLanguage();
            Traduction.Instance.setLanguage(language);
            view.askSourcePath();
            view.askTargetFile();
            string targetFile = view.getTargetFile();
            view.askTargetPath();
            string sourcePath = view.getSourcePath();
            string targetPath = view.getTargetPath();
            if (checkPathIntegrity(sourcePath, targetPath) && !checkTargetDirectory(targetFile))
            {
                model.sourcePath = sourcePath;
                model.targetPath = targetPath;
                model.targetFile = view.getTargetFile(); ;
                view.progress(false);
                model.saveFile(model.sourcePath, model.targetPath);
                model.writeLog();
                view.progress(true);
            }
        }



        /// <summary>
        ///Envoie des informations de copie en temps réelles à la vue pour les afficher
        /// Sending the informations to copy in real time to the view in order to display them
        /// </summary>
        /// <param name="fileName">toto</param>
        /// <param name="countfile"></param>
        /// <param name="totalFileToCopy"></param>
        /// <param name="percentage"></param>
        public void sendProgressInfoToView(string fileName, double countfile, int totalFileToCopy, double percentage)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            string text = $"{percentage}% | {countfile}/{totalFileToCopy} {Traduction.Instance.Langue.InCopy} | {fileName}";
            view.display(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        // Regex verifiant la validité du nom de sauvegarde
        // Regex checking the validity of the Back-up's name
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
            {valid = false;}

            return valid;
        }

        // Execution des différentes fonctions de vérification de chemin
        // Execution of the different path control functions 
        private bool checkPathIntegrity(string source, string target)
        {
            bool integrity = false;
            if (pathIsValid(source) && pathExist(source))
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

        // Verification de l'existence du chemin
        // Verifying the path existence 
        private bool pathExist(string Path)
        {
            bool test = Directory.Exists(Path);
            return test;
        }

        // Verification de la validité du format du chemin
        // Verifying the validity of the path format

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
    }
}