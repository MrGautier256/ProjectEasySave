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

namespace ProjetBureau
{
    /// <summary>
    /// Logique d'interaction pour ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        public progressState progress = progressState.play;
        public ProgressWindow()
        {
            InitializeComponent();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            progress = progressState.stop;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            progress = progressState.play;
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            progress = progressState.pause;
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (progress == progressState.play) { progress = progressState.pause; PlayPauseButton.Content = "Pause";} 
            else if (progress == progressState.pause) { progress = progressState.play; PlayPauseButton.Content = "Play";}
        }
    }
}
