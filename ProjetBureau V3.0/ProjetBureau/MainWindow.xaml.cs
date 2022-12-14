using CommonCode;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Windows.System.Preview;
using static System.Net.Mime.MediaTypeNames;

namespace ProjetBureau
{
    public partial class MainWindow : Window, IView
    {
        readonly ProgressWindow progressWindow = new();
        private System.Windows.Forms.Timer timer;
        bool? previousState = null;
        RemoteView remoteView;

        public MainWindow()
        {
            InitializeComponent();

            remoteView = new();

            timer = new System.Windows.Forms.Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 500;

            Traduction.SetInterfaceLanguage(SelectLanguage.Text);
            TextEnterSourcePath.Content = Traduction.Instance.Langue.EnterSourcePath;
            TextLanguage.Content = Traduction.Instance.Langue.SelectLanguage;
            TextEnterTargetPath.Content = Traduction.Instance.Langue.EnterTargetPath;
            TextEnterTargetFile.Content = Traduction.Instance.Langue.EnterTargetFile;
            TextEnterLogType.Content = Traduction.Instance.Langue.EnterLogType;
            SaveButton.Content = Traduction.Instance.Langue.Save;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            var businessProcesses = Process.GetProcessesByName(Properties.Settings.Default.BusinessProcessName);
            bool businessProcessIsOn = (businessProcesses?.Length > 0);
            progressWindow.PlayPauseButton.IsEnabled = !businessProcessIsOn;
            if (previousState == businessProcessIsOn)
            { return; }
            if (businessProcessIsOn && progressWindow.progress == ProgressState.play && progressWindow.IsVisible)
            {
                progressWindow.progress = ProgressState.pause;
                progressWindow.SaveFileServiceCommand?.Pause();
            }
            else if (!businessProcessIsOn && progressWindow.progress == ProgressState.pause && progressWindow.IsVisible)
            {
                progressWindow.progress = ProgressState.play;
                progressWindow.SaveFileServiceCommand?.Resume();
            }
            previousState = businessProcessIsOn;
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
        LangueEnum IView.AskLanguage() { return Traduction.ConvertLanguage(SelectLanguage.Text); }

        public string AsklogType() { return "json"; }

        public string AskSourcePath() { return textBoxSourcePath.Text; }

        public string AskTargetFile() { return textBoxNameSave.Text; }

        public string AskTargetPath() { return textBoxDestPath.Text; }

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
        public void ControlProgress(string fileFullName, int countfile, int totalFileToCopy, double percentage)
        {
            Dispatcher.Invoke(() =>
            {
                if (progressWindow.progress != ProgressState.pause)
                {
                    string fileName = System.IO.Path.GetFileName(fileFullName);
                    progressWindow.ContentCountsize.Text = $"{countfile}/{totalFileToCopy}";
                    progressWindow.ContentFilename.Text = $" {fileName}";
                    progressWindow.ContentHistory.Text = $"{countfile}/{totalFileToCopy} | {fileName}\n{progressWindow.ContentHistory.Text}";
                    progressWindow.ProgressBarSave.Value = Convert.ToDouble(percentage);
                }
            }, DispatcherPriority.Background);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controller controller = new Controller(this, remoteView);
            controller.Execute();
        }

        /// <summary>
        /// Réglage du language selon le choix de l'utilisateur dans le menu déroulant
        /// Setting of language according to the user's choice in the drop-down menu
        /// </summary>
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

        /// <summary>
        /// Information de début et de fin de sauvegarde et affichage et fermeture de la fenetre de progression
        /// Backup start and end information, display and hiding of the progress window
        /// </summary>
        public void Progress(bool state)
        {
            Dispatcher.Invoke(() =>
            {
                if (!state)
                {
                    progressWindow.init();
                    progressWindow.Show();
                    timer.Start();
                    this.IsEnabled = false;
                    progressWindow.progress = ProgressState.play;
                }
                else if (state)
                {
                    timer.Stop();
                    progressWindow.Hide();
                    previousState = null;
                    this.IsEnabled = true;
                }
            }, DispatcherPriority.Background);
        }

        /// <summary>
        /// Methodes récupérant le chemin source de la sauvegarde 
        /// Methods get source path of the backup
        /// </summary>
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

        /// <summary>
        /// Methodes récupérant le chemin de destination de la sauvegarde 
        /// Methods get destination path of the backup
        /// </summary>
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
        public void SendSaveFileServiceCommand(ISaveFileServiceCommand saveFileService)
        {
            progressWindow.SaveFileServiceCommand = saveFileService;
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            timer.Tick -= Timer_Tick;
            progressWindow.Close();
        }
    }
}
