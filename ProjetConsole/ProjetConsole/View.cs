using System;
using System.Reflection;
using TesJson;

namespace ProjetConsole
{
    public enum langueEnum {english,french,spanish };
    public class View
    {
        string sourcePath = string.Empty;
        string targetPath = string.Empty;

    
        public langueEnum askLanguage()
        {
            string langageToPrint = string.Empty; 
            foreach(var item in Enum.GetValues(typeof(langueEnum)))
            {
                langageToPrint += item + ", ";
            }
            Console.WriteLine("Select language: " + langageToPrint);
            string? inputLanguage = Console.ReadLine()?.ToLower();
            langueEnum language;

            switch (inputLanguage)
            {
                case "french":
                case "fr":
                case "français":
                case "francais": 
                    language = langueEnum.french;
                    break;
                case "spanish":
                case "es":
                case "espagnol":
                    language = langueEnum.spanish;
                    break;
                default: language = langueEnum.english;
                    break;
            }
            return language;
        }

        // --------------Version Console------------------
        public void askSourcePath()
        {
            Console.WriteLine(Traduction.Instance.Langue.EnterSourcePath);
            this.sourcePath = Console.ReadLine() ?? string.Empty; 
        }
        public void askTargetPath()
        {
            Console.WriteLine(Traduction.Instance.Langue.EnterTargetPath);
            this.targetPath = Console.ReadLine() ?? string.Empty; 
        }

        public string getSourcePath()
        { return this.sourcePath; }

        public string getTargetPath()
        { return this.targetPath; }

        public void sourcePathIsInvalid()
        {
            Console.WriteLine(Traduction.Instance.Langue.SourcePathInvalid);
        }
        public void targetPathIsInvalid()
        {
            Console.WriteLine(Traduction.Instance.Langue.TargetPathInvalid);
        }
        public void targetPathIsInvalid(bool state)
        {
            if (!state) { Console.WriteLine(Traduction.Instance.Langue.Buffering); }
            else { Console.WriteLine(Traduction.Instance.Langue.Complete); }
        }

    }

}