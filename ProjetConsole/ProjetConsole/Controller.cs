using System;
using System.Diagnostics;

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
        public Controller()
        {
            model = new Model();
            view = new View();
        }

        public void execute()
        {
            var language = view.askLanguage();
            Traduction.Instance.setLanguage(language);
            view.askSourcePath();
            view.askTargetPath();
            string sourcePath = view.getSourcePath();
            string targetPath = view.getTargetPath();
            if (checkPathIntegrity(sourcePath, targetPath))
            {
                model.sourcePath = sourcePath;
                model.targetPath = targetPath;
                model.targetFile = view.getTargetFile(); ;
                model.setTotalFileToCopy();
                view.progress(false);
                model.saveFile(model.sourcePath, model.targetPath);
                model.writelog();
                view.progress(true);
            }
        }

        private bool checkPathIntegrity(string source, string target)
        {
            bool integrity = false;
            if (pathIsValid(source) && pathExist(source))
            {
                if (pathIsValid(target) && !pathExist(target) && (target!= ""))
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
            return Directory.Exists(Path);
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
                    string root = Path.GetPathRoot(path) ?? string.Empty;
                    isValid = string.IsNullOrEmpty(root.Trim(new char[] { '\\', '/' })) == false;
                }
            }
            catch
            {
                isValid = false;
            }
            return isValid;
        }
        public void buffering()
        {

        }
    }
}