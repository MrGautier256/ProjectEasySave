using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ProjetBureau
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        Mutex? myMutex;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool aIsNewInstance = false;
            myMutex = new Mutex(true, "E5B6E344-20F0-45F4-A858-AFCE61BC8BB5", out aIsNewInstance);
            if (!aIsNewInstance)
            {
                MessageBox.Show("Already an instance is running...");
                App.Current.Shutdown();
            }
        }
    }
}
