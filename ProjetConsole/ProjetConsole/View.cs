using System;
using System.Diagnostics;
using System.Reflection;

namespace ProjetConsole
{
    // Enumération des langues disponibles
    //Enumeration of the available languages
    public enum langueEnum { english, french, spanish };
    public class View
    {
        private string sourcePath = string.Empty;
        private string targetPath = string.Empty;
        private string targetFile = string.Empty;

        // --------------Demande d'informations à l'utilisateur (méthode) ------------------
        // --------------Ask Informations to user (methods) ------------------
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
                langageToPrint += $"{item}, ";
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

        // --------------Récupération d'information (méthode)------------------
        // --------------Get Info (methods)------------------
        public string getSourcePath()
        { return this.sourcePath; }
        public string getTargetPath()
        { return this.targetPath; }
        public string getTargetFile()
        { return this.targetFile; }


        // ---Méthodes informant l'utilisateurs que des informations sont invalides---
        // ---methods informing the user that information is invalid---
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
        // Display of the beginning and the end of the back-up
        public void progress(bool state)
        {
            if (!state) { Console.WriteLine("\n" + Traduction.Instance.Langue.Buffering); }
            else if (state) { Console.WriteLine("\n" + Traduction.Instance.Langue.Complete); }

        }

        // Affichage en temps réel des informations de la sauvegarde (Pourcentage | Nom du fichier | Nombre de fichier restant)
        // Display in real time the informations of the back-up (Percentage | File's name | Number of remaining files)
        public void display(string toDisplay)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(toDisplay);
            Console.ForegroundColor = ConsoleColor.White;
        }

    }

}