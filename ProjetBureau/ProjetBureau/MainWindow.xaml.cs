using CommonCode;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Windows.Globalization;
using static System.Net.Mime.MediaTypeNames;

namespace ProjetBureau
{
    public partial class MainWindow : Window, IView
    {
        readonly ProgressWindow progressWindow = new();
        public MainWindow()
        {
            InitializeComponent();
            Traduction.SetInterfaceLanguage(SelectLanguage.Text);
            TextEnterSourcePath.Content = Traduction.Instance.Langue.EnterSourcePath;
            TextLanguage.Content = Traduction.Instance.Langue.SelectLanguage;
            TextEnterTargetPath.Content = Traduction.Instance.Langue.EnterTargetPath;
            TextEnterTargetFile.Content = Traduction.Instance.Langue.EnterTargetFile;
            TextEnterLogType.Content = Traduction.Instance.Langue.EnterLogType;
            SaveButton.Content = Traduction.Instance.Langue.Save;
            textBoxSourcePath.Text = "C:\\Users\\Gautier\\OneDrive - Association Cesi Viacesi mail\\CESI\\3ème Année\\Projet 2 - Programmation Système\\Projet\\TestCopie\\Source3";
            textBoxDestPath.Text = "C:\\Users\\Gautier\\OneDrive - Association Cesi Viacesi mail\\CESI\\3ème Année\\Projet 2 - Programmation Système\\Projet\\TestCopie";
        }
        public string typeOfMode => "Graphic";

        /// <summary>
        /// --------------Demande d'informations à l'utilisateur (méthode) ------------------
        /// --------------Ask Informations to user (methods) ------------------
        /// </summary>

        public static LangueEnum AskLanguage()
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
        {
            string? messageBoxText = Traduction.Instance.Langue.SourcePathInvalid;
            string caption = "Source Path Invalid";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;
            System.Windows.MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
        }
        public void TargetPathIsInvalid()
        {
            string? messageBoxText = Traduction.Instance.Langue.TargetPathInvalid;
            string caption = "Source Path Invalid";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;
            System.Windows.MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
        }
        public void TargetDirInvalid()
        {
            string? messageBoxText = Traduction.Instance.Langue.targetDirInvalid;
            string caption = "Source Path Invalid";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;
            System.Windows.MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
        }


        /// <summary>
        /// Affichage en temps réel des informations de la sauvegarde (Pourcentage | Nom du fichier | Nombre de fichier restant)
        /// Display in real time the informations of the back-up (Percentage | File's name | Number of remaining files)
        /// </summary>
        /// <param name="textToDisplay"></param>
        public void Display(string[] textToDisplay)
        {
            progressWindow.ContentCountsize.Dispatcher.Invoke(() => progressWindow.ContentCountsize.Text = textToDisplay[0], DispatcherPriority.Background);
            progressWindow.ContentFilename.Dispatcher.Invoke(() => progressWindow.ContentFilename.Text = textToDisplay[1], DispatcherPriority.Background);
            progressWindow.ContentHistory.Dispatcher.Invoke(() => progressWindow.ContentHistory.Text = textToDisplay[2], DispatcherPriority.Background);
            progressWindow.ProgressBarSave.Dispatcher.Invoke(() => progressWindow.ProgressBarSave.Value = Convert.ToDouble(textToDisplay[3]), DispatcherPriority.Background);
        }
        public ProgressState ControlProgress(string fileName, double countfile, int totalFileToCopy, double percentage)
        {
            DoEvents();
            if (progressWindow.progress != ProgressState.pause)
            {
                string[] text =
                    {
                    $"{countfile}/{totalFileToCopy}",
                    $" {fileName}",
                    $"{countfile}/{totalFileToCopy} | {fileName}\n{progressWindow.ContentHistory.Text}",
                    $"{percentage}"
                    };

                Display(text);
            }

            return progressWindow.progress;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controller controller = new Controller(this);
            controller.execute(DoEvents);
        }

        private void SelectLanguage_DropDownClosed(object sender, EventArgs e)
        {
            Traduction.SetInterfaceLanguage(SelectLanguage.Text);
            TextEnterSourcePath.Content = Traduction.Instance.Langue.EnterSourcePath;
            TextLanguage.Content = Traduction.Instance.Langue.SelectLanguage;
            TextEnterTargetPath.Content = Traduction.Instance.Langue.EnterTargetPath;
            TextEnterTargetFile.Content = Traduction.Instance.Langue.EnterTargetFile;
            TextEnterLogType.Content = Traduction.Instance.Langue.EnterLogType;
            SaveButton.Content = Traduction.Instance.Langue.Save;
        }

        public void Progress(bool state)
        {
            if (!state)
            {
                progressWindow.Show();
                progressWindow.progress = ProgressState.play;
            }
            else if (state)
            {
                progressWindow.Hide();
                progressWindow.ContentCountsize.Dispatcher.Invoke(() => progressWindow.ContentCountsize.Text = string.Empty, DispatcherPriority.Background);
                progressWindow.ContentFilename.Dispatcher.Invoke(() => progressWindow.ContentFilename.Text = string.Empty, DispatcherPriority.Background);
                progressWindow.ContentHistory.Dispatcher.Invoke(() => progressWindow.ContentHistory.Text = string.Empty, DispatcherPriority.Background);
                progressWindow.ProgressBarSave.Dispatcher.Invoke(() => progressWindow.ProgressBarSave.Value = 0, DispatcherPriority.Background);
            }
        }
        LangueEnum IView.AskLanguage() { return Traduction.ConvertLanguage(SelectLanguage.Text); }

        public string AsklogType() { return "json"; }

        public string AskSourcePath() { return textBoxSourcePath.Text; }

        public string AskTargetFile() { return textBoxNameSave.Text; }

        public string AskTargetPath() { return textBoxDestPath.Text; }

        private void BtnBrowseSourceFolder_Click(object sender, RoutedEventArgs e)
        {
            var folderDlg = new System.Windows.Forms.FolderBrowserDialog
            { ShowNewFolderButton = true };
            // Show the FolderBrowserDialog.  
            var result = folderDlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                textBoxSourcePath.Text = folderDlg.SelectedPath;
                //Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }
        private void BtnBrowseDestFolder_Click(object sender, RoutedEventArgs e)
        {
            var folderDlg = new System.Windows.Forms.FolderBrowserDialog
            { ShowNewFolderButton = true };
            // Show the FolderBrowserDialog.  
            var result = folderDlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                textBoxDestPath.Text = folderDlg.SelectedPath;
                //Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        public static void DoEvents()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                                                  new Action(delegate { }));
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            var importwindow = new ImportWindow();
            importwindow.Show();
            this.Close();
        }
    }
}
