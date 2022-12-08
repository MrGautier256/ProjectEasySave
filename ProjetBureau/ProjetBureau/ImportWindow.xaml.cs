using CommonCode;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjetBureau
{
    public partial class ImportWindow : Window
    {
        public ImportWindow()
        {
            InitializeComponent();
            Traduction.SetInterfaceLanguage(SelectLanguage.Text);
            TextEnterSourcePath.Content = Traduction.Instance.Langue.EnterSourcePath;
            TextLanguage.Content = Traduction.Instance.Langue.SelectLanguage;
            TextEnterTargetPath.Content = Traduction.Instance.Langue.EnterTargetPath;
            TextEnterLogType.Content = Traduction.Instance.Langue.EnterLogType;
            ImportButton.Content = Traduction.Instance.Langue.Import;
            textBoxSourcePath.Text = "C:\\Users\\Gautier\\OneDrive - Association Cesi Viacesi mail\\CESI\\3ème Année\\Projet 2 - Programmation Système\\Projet\\TestCopie\\Source3";
            textBoxDestPath.Text = "C:\\Users\\Gautier\\OneDrive - Association Cesi Viacesi mail\\CESI\\3ème Année\\Projet 2 - Programmation Système\\Projet\\TestCopie";
        }
        public string typeOfMode => "Graphic";

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

        public string AsklogType() { return "json"; }

        public string AskSourcePath() { return textBoxSourcePath.Text; }

        public string AskTargetPath() { return textBoxDestPath.Text; }

        private void btnBrowseFile_Click(object sender, RoutedEventArgs e)
        {
            int size = -1;
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == System.Windows.Forms.DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    string text = File.ReadAllText(file);
                    size = text.Length;
                }
                catch (IOException)
                {
                }
            }
        }

        private void BtnBrowseFolder_Copy_Click(object sender, RoutedEventArgs e)
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
    }
}
