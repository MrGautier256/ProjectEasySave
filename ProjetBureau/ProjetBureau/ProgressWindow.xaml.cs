using CommonCode;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ProjetBureau
{
    /// <summary>
    /// Logique d'interaction pour ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        public ProgressState progress = ProgressState.play;
        public ProgressWindow()
        {
            InitializeComponent();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            progress = ProgressState.stop;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            progress = ProgressState.play;
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            progress = ProgressState.pause;
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (progress == ProgressState.play) 
            { 
                progress = ProgressState.pause; 
              PlayPauseButton.Content = "Resume";
            } 
            else if (progress == ProgressState.pause) { 
                progress = ProgressState.play; 
                PlayPauseButton.Content = "Pause";}
            //DoEvents();
        }
    }
}
