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

namespace ProjetBureau
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnBrowseFile_Click(object sender, RoutedEventArgs e)
        {
            //Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //dlg.FileName = "Document"; // Default file name
            //dlg.DefaultExt = ".txt"; // Default file extension
            //dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            //Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            //Process open file dialog box results
            if (result == true)
            {
                //Open document
                TextBox path = (TextBox)(((FrameworkElement)sender).Parent as FrameworkElement).FindName("textPath");
                path.Text = dlg.FileName;
                path.Focus(); //these 2 lines force the binding to trigger
                ((Button)sender).Focus();
            }
        }
        //private void btnBrowseFile_Click(object sender, RoutedEventArgs e)
        //{
        //    int size = -1;
        //    OpenFileDialog openFileDialog1 = new OpenFileDialog();
        //    DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
        //    if (result == DialogResult.OK) // Test result.
        //    {
        //        string file = openFileDialog1.FileName;
        //        try
        //        {
        //            string text = File.ReadAllText(file);
        //            size = text.Length;
        //        }
        //        catch (IOException)
        //        {
        //        }
        //    }
        //    Console.WriteLine(size); // <-- Shows file size in debugging mode.
        //    Console.WriteLine(result); // <-- For debugging use.
        //}

    }
}
