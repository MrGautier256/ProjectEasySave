using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ProjetConsole
{
    public interface IController
    {
        public void execute();
    }
    public class Controller : IController
    {
        private Model model;
        private View view;
        ~Controller(){}
        public Controller()
        {
            model = new Model(this);
            view = new View();
        }

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
                model.setTotalFileToCopy();
                view.progress(false);
                model.setTotalSize(model.sourcePath, model.targetPath);
                model.saveFile(model.sourcePath, model.targetPath);
                model.writelog();
                view.progress(true);
            }
        }
        public void sendProgressInfoToView(string fileName, double countfile, int totalFileToCopy, double percentage)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            string text = percentage + "% | " + countfile + "/" + totalFileToCopy +" "+ Traduction.Instance.Langue.InCopy +" | " + fileName;
            view.Display(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
        private bool checkTargetDirectory(string DirName)
        {
            bool valid;
            Regex RgxUrl = new Regex("[^a-zA-Z0-9 ]");
            if (RgxUrl.IsMatch(DirName))
            {
                view.targetDirInvalid();
                valid = true;
            }
            else { 
                valid = false;
                
            }
            
            return valid;
        }
        private bool checkPathIntegrity(string source, string target)
        {
            bool integrity = false;
            if (pathIsValid(source) && pathExist(source))
            {
                if (pathIsValid(target) && !pathExist(target) && (target != ""))
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
        private bool pathExist(string Path)
        {
            bool test = Directory.Exists(Path);
            return test;
        }
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
                    string root = Path.GetPathRoot(path);
                    isValid = string.IsNullOrEmpty(root.Trim(new char[] { '\\', '/' })) == false;
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