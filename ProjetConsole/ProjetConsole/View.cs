using System;
using System.Reflection;

namespace ProjetConsole
{
    public class View
    {
        string sourcePath = string.Empty;
        string targetPath = string.Empty;

        // --------------Version Console------------------
        public void askSourcePath()
        {
            Console.WriteLine("Entrez le chemin source: \n Enter source Path:");
            this.sourcePath = Console.ReadLine() ?? string.Empty; 
        }
        public void askTargetPath()
        {
            Console.WriteLine("Entrez le chemin de destination: \n Enter target Path:");
            this.targetPath = Console.ReadLine() ?? string.Empty; 
        }

        public string getSourcePath()
        { return this.sourcePath; }

        public string getTargetPath()
        { return this.targetPath; }

        public void sourcePathIsInvalid()
        {
            Console.WriteLine("Le chemin source n'existe pas ou n'est pas valide! \n Source path does not exist or is invalid!");
        }
        public void targetPathIsInvalid()
        {
            Console.WriteLine("Le chemin de destination n'existe pas ou n'est pas valide! \n Destination path does not exist or is invalid!");
        }

        // --------------Version Graphique ------------------
        /*
        public void askSourcePathUser(MainWindow fenetre)
        { this.inputUser = fenetre.TextInputUser.Text; }


        public void askTargetPathUser(MainWindow fenetre)
        { this.inputUser = fenetre.TextInputUser.Text; }
        */

        // --------------UpdateView------------------
        /*
        public void updateView(string upperText)
        {
            Console.WriteLine(upperText);
        }
        public void updateView(string upperText, MainWindow fenetre)
        {
            fenetre.TextResult.Text = upperText;
        }
        */

    }

}