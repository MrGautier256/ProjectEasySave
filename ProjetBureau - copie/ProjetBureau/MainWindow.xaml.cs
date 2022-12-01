using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Windows.Globalization;

namespace ProjetBureau
{
    public enum langueEnum { english, french, spanish };
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Traduction.Instance.SetInterfaceLanguage("default");
            TextEnterSourcePath.Content = Traduction.Instance.Langue.EnterSourcePath;
            TextLanguage.Content = Traduction.Instance.Langue.SelectLanguage;
            TextEnterTargetPath.Content = Traduction.Instance.Langue.EnterTargetPath;
            TextEnterTargetFile.Content = Traduction.Instance.Langue.EnterTargetFile;
            TextEnterLogType.Content = Traduction.Instance.Langue.EnterLogType;
        }

        private string sourcePath = string.Empty;
        private string targetPath = string.Empty;
        private string targetFile = string.Empty;
        /// <summary>
        /// --------------Demande d'informations à l'utilisateur (méthode) ------------------
        /// --------------Ask Informations to user (methods) ------------------
        /// </summary>

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




        /// <summary>
        /// --------------Récupération d'information (méthode)------------------
        /// --------------Get Info (methods)------------------
        /// </summary>
        /// <returns></returns>

        public string getSourcePath()
        { return this.sourcePath; }
        public string getTargetPath()
        { return this.targetPath; }
        public string getTargetFile()
        { return this.targetFile; }

        /// <summary>
        /// ---Méthodes informant l'utilisateurs que des informations sont invalides---
        /// ---methods informing the user that information is invalid---
        /// </summary>

        public void sourcePathIsInvalid()
        {
            string? messageBoxText = Traduction.Instance.Langue.SourcePathInvalid;
            string caption = "Source Path Invalid";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
        }
        public void targetPathIsInvalid()
        { 
            string? messageBoxText = Traduction.Instance.Langue.TargetPathInvalid;
            string caption = "Source Path Invalid";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
        }
        public void targetDirInvalid()
        { 
            string? messageBoxText = Traduction.Instance.Langue.targetDirInvalid;
            string caption = "Source Path Invalid";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
        }

        /// <summary>
        /// Affichage du commencement et de la fin de la sauvegarde
        /// Display of the beginning and the end of the back-up
        /// </summary>
        /// <param name="state"></param>

        public void progress(bool state)
        {
            if (!state) { Console.WriteLine("\n" + Traduction.Instance.Langue.Buffering); }
            else if (state) { Console.WriteLine("\n" + Traduction.Instance.Langue.Complete); }
        }
        /// <summary>
        /// Affichage en temps réel des informations de la sauvegarde (Pourcentage | Nom du fichier | Nombre de fichier restant)
        /// Display in real time the informations of the back-up (Percentage | File's name | Number of remaining files)
        /// </summary>
        /// <param name="toDisplay"></param>

        public void display(double toDisplay)
        {
            ProgressBarSave.Dispatcher.Invoke(() => ProgressBarSave.Value = toDisplay, DispatcherPriority.Background);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IController controller = new Controller(this);
            controller.execute(textBoxSourcePath.Text, textBoxDestPath.Text, LogType.Text, textBoxNameSave.Text);
        }

        private void SelectLanguage_DropDownClosed(object sender, EventArgs e)
        {
            Traduction.Instance.SetInterfaceLanguage(SelectLanguage.Text);
            TextEnterSourcePath.Content = Traduction.Instance.Langue.EnterSourcePath;
            //TextLanguage.Content = Traduction.Instance.Langue.SelectLanguage;
            TextEnterTargetPath.Content = Traduction.Instance.Langue.EnterTargetPath;
            TextEnterTargetFile.Content = Traduction.Instance.Langue.EnterTargetFile;
            TextEnterLogType.Content = Traduction.Instance.Langue.EnterLogType;
        }
    }
}
