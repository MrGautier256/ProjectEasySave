using System;
using System.Diagnostics;
using TesJson;

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
                model.saveFile();
            }
        }

        private bool checkPathIntegrity(string source, string target)
        {
            bool integrity = false;
            if (pathIsValid(source) && sourcePathExist(source))
            {
                if (pathIsValid(target))
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
        private bool sourcePathExist(string sourcePath)
        {
            bool exist = Directory.Exists(sourcePath);
            if (!exist)
            {
                view.sourcePathIsInvalid();
            }
            return exist;
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
    }
}