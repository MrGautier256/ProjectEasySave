using CommonCode;
using System;
using System.Diagnostics;
using System.Reflection;

namespace ProjetConsole
{   
    /// <summary>
    /// Enumération des langues disponibles
    /// Enumeration of the available languages
    /// </summary>
    
    public class View:IView
    {
        public string typeOfMode => "Console";
        // Peut s'écrire: public string typeOfMode { get { return "Console"; } }

        /// <summary>
        /// --------------Demande d'informations à l'utilisateur (méthode) ------------------
        /// --------------Ask Informations to user (methods) ------------------
        /// </summary>

        public string AskSourcePath()
        {
            Console.WriteLine("\n" + Traduction.Instance.Langue.EnterSourcePath);
            return Console.ReadLine() ?? string.Empty;
        }
        public string AskTargetFile()
        {
            Console.WriteLine("\n" + Traduction.Instance.Langue.EnterTargetFile);
            return Console.ReadLine() ?? string.Empty;
        }
        public string AskTargetPath()
        {
            Console.WriteLine("\n" + Traduction.Instance.Langue.EnterTargetPath);
            return Console.ReadLine() ?? string.Empty;
        }
        public string AsklogType()
        {
            Console.WriteLine("\n" + Traduction.Instance.Langue.EnterLogType);
            return Console.ReadLine()?.ToLower() ?? string.Empty;
        }

        public LangueEnum AskLanguage()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            string langageToPrint = string.Empty;
            foreach (var item in Enum.GetValues(typeof(LangueEnum)))
            {
                langageToPrint += $"{item}, ";
            }
            Console.WriteLine("Select language: " + langageToPrint);
            string? inputLanguage = Console.ReadLine()?.ToLower();
            var selectedLanguage = inputLanguage switch
            {
                "french" or "fr" or "français" or "francais" => LangueEnum.french,
                "spanish" or "es" or "espagnol" => LangueEnum.spanish,
                _ => LangueEnum.english,
            };
            return selectedLanguage;
        }

        /// <summary>
        /// ---Méthodes informant l'utilisateurs que des informations sont invalides---
        /// ---methods informing the user that information is invalid---
        /// </summary>
        
        public void SourcePathIsInvalid()
        { Console.WriteLine(Traduction.Instance.Langue.SourcePathInvalid + "\n"); }
        public void TargetPathIsInvalid()
        { Console.WriteLine(Traduction.Instance.Langue.TargetPathInvalid + "\n"); }
        public void TargetDirInvalid()
        { Console.WriteLine(Traduction.Instance.Langue.targetDirInvalid + "\n"); }

        /// <summary>
        /// Affichage du commencement et de la fin de la sauvegarde
        /// Display of the beginning and the end of the back-up
        /// </summary>
        /// <param name="state"></param>
        
        public void Progress(bool state)
        {
            if (!state) { Console.WriteLine("\n" + Traduction.Instance.Langue.Buffering); }
            else if (state) { Console.WriteLine("\n" + Traduction.Instance.Langue.Complete); }
        }
        /// <summary>
        /// Affichage en temps réel des informations de la sauvegarde (Pourcentage | Nom du fichier | Nombre de fichier restant)
        /// Display in real time the informations of the back-up (Percentage | File's name | Number of remaining files)
        /// </summary>
        /// <param name="toDisplay"></param>
        
        public void Display(string[] toDisplay)
        {
            Console.WriteLine(toDisplay[0]);
        }

        public void ControlProgress(string fileFullName, int countfile, int totalFileToCopy, double percentage)
        {
            string fileName = Path.GetFileName(fileFullName);
            Console.ForegroundColor = ConsoleColor.Green;
            string[] text = {$"{percentage}% | {countfile}/{totalFileToCopy} {Traduction.Instance.Langue.InCopy} | {fileName}" };
            Display(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void SendSaveFileServiceCommand(ISaveFileServiceCommand saveFileService)
        {
            // Do Nothing
        }
    }

}