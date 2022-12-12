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
        public ISaveFileServiceCommand? SaveFileServiceCommand { get; set; }

        public ProgressWindow()
        {
            InitializeComponent();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            ArgumentNullException.ThrowIfNull(SaveFileServiceCommand);
            SaveFileServiceCommand.Stop();
            progress = ProgressState.stop;
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            ArgumentNullException.ThrowIfNull(SaveFileServiceCommand);
            if (progress == ProgressState.play)
            {
                progress = ProgressState.pause;
                PlayPauseButton.Content = "Resume";
                SaveFileServiceCommand.Pause();
            }
            else if (progress == ProgressState.pause)
            {
                progress = ProgressState.play;
                PlayPauseButton.Content = "Pause";
                SaveFileServiceCommand.Resume();
            }
        }
    }
}
