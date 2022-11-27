using System;
using System.Diagnostics;
using System.Reflection;

namespace ProjetConsole
{
    // Enumération des langues disponibles
    public enum langueEnum { english, french, spanish };
    public class View
    {
        private string sourcePath = string.Empty;
        private string targetPath = string.Empty;
        private string targetFile = string.Empty;

        // --------------Ask Info to user methods ------------------
        public void askSourcePath()
        {
            Console.WriteLine("\n" + Traduction.Instance.Langue.EnterSourcePath);
            this.sourcePath = Console.ReadLine() ?? string.Empty;
        }
        public void askTargetFile()
        {
            Console.WriteLine("\n" + Traduction.Instance.Langue.EnterTargetFile);
            this.targetFile = Console.ReadLine() ?? string.Empty;
        }
        public void askTargetPath()
        {
            Console.WriteLine("\n" + Traduction.Instance.Langue.EnterTargetPath);
            this.targetPath = Path.Combine(Console.ReadLine() ?? string.Empty, targetFile);
        }

        public langueEnum askLanguage()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            string langageToPrint = string.Empty;
            foreach (var item in Enum.GetValues(typeof(langueEnum)))
            {
                langageToPrint += item + ", ";
            }
            Console.WriteLine("Select language: " + langageToPrint);
            string? inputLanguage = Console.ReadLine()?.ToLower();
            langueEnum selectedLanguage;

            switch (inputLanguage)
            {
                case "french":
                case "fr":
                case "français":
                case "francais":
                    selectedLanguage = langueEnum.french;
                    break;
                case "spanish":
                case "es":
                case "espagnol":
                    selectedLanguage = langueEnum.spanish;
                    break;
                default:
                    selectedLanguage = langueEnum.english;
                    break;
            }
            return selectedLanguage;
        }

        // --------------Get Info methods------------------
        public string getSourcePath()
        { return this.sourcePath; }
        public string getTargetPath()
        { return this.targetPath; }
        public string getTargetFile()
        { return this.targetFile; }


        // ---Méthodes informant l'utilisateurs que des informations sont invalides---
        public void sourcePathIsInvalid()
        {
            Console.WriteLine(Traduction.Instance.Langue.SourcePathInvalid + "\n");
        }
        public void targetPathIsInvalid()
        {
            Console.WriteLine(Traduction.Instance.Langue.TargetPathInvalid + "\n");
        }
        public void targetDirInvalid()
        {
            Console.WriteLine(Traduction.Instance.Langue.targetDirInvalid + "\n");
        }

        // Affichage du commencement et de la fin de la sauvegarde
        public void progress(bool state)
        {
            if (!state) { Console.WriteLine("\n" + Traduction.Instance.Langue.Buffering); }
            else if (state) { Console.WriteLine("\n" + Traduction.Instance.Langue.Complete); }

        }

        // Affichage en temps réel des informations e la sauvegarde (Pourcentage | Nom du fichier | Nombre de fichier restant)
        public void Display(string toDisplay)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(toDisplay);
            Console.ForegroundColor = ConsoleColor.White;
        }

    }

}