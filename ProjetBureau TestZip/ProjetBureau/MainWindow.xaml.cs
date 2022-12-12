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
using Windows.System.Preview;
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
            ImportLinkButton.Content = Traduction.Instance.Langue.ImportLink;
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
        /// <param name="toDisplay"></param>

        public void ControlProgress(string fileName, double countfile, int totalFileToCopy, double percentage, bool copy)
        {
            Dispatcher.Invoke(() =>
            {
                if (progressWindow.progress != ProgressState.pause)
                {
                    progressWindow.ContentCountsize.Text = $"{countfile}/{totalFileToCopy}";
                    progressWindow.ContentFilename.Text = $" {fileName}";
                    progressWindow.ContentHistory.Text = $"{countfile}/{totalFileToCopy} | {fileName}\n{progressWindow.ContentHistory.Text}";

                    if(copy) 
                    { 
                        progressWindow.ProgressBarCopy.Value = Convert.ToDouble(percentage);
                        progressWindow.ProgressBarSave.Value = Convert.ToDouble(percentage / 2);
                    }
                    else if (!copy) 
                    {
                        progressWindow.ProgressBarEncrypt.Value = Convert.ToDouble(percentage);
                        progressWindow.ProgressBarSave.Value = Convert.ToDouble(percentage + 50);
                    }


                }
            }, DispatcherPriority.Background);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controller controller = new Controller(this);
            controller.Execute();
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
            ImportLinkButton.Content = Traduction.Instance.Langue.ImportLink;
        }

        public void Progress(bool state)
        {
            Dispatcher.Invoke(() =>
            {
                if (!state)
                {
                    progressWindow.Show();
                    this.IsEnabled = false;
                    progressWindow.progress = ProgressState.play;
                }
                else if (state)
                {
                    progressWindow.Hide();
                    this.IsEnabled = true;
                    progressWindow.ContentCountsize.Text = string.Empty;
                    progressWindow.ContentFilename.Text = string.Empty;
                    progressWindow.ContentHistory.Text = string.Empty;
                    progressWindow.ProgressBarSave.Value = 0;
                    progressWindow.ProgressBarCopy.Value = 0;
                    progressWindow.ProgressBarEncrypt.Value = 0;

                }
            }, DispatcherPriority.Background);
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
        public void SendSaveFileServiceCommand(ISaveFileServiceCommand saveFileService)
        {
            progressWindow.SaveFileServiceCommand = saveFileService;
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            progressWindow.Close();
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            var importwindow = new ImportWindow(SelectLanguage.Text);
            importwindow.ShowDialog();
            this.Hide();
        }
    }
}
