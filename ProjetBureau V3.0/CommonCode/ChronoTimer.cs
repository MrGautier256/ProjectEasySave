using System;
using System.Diagnostics;

namespace CommonCode
{
    public static class ChronoTimer
    {
        //Methode chronométrant l'exécution d'une autre méthode
        // Method timing the execution of another method
        public static string Chrono(Action method)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            string ElapsedTime;
            try
            {
                method();
            }
            finally
            {
                stopWatch.Stop();
                ElapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    stopWatch.Elapsed.Hours, stopWatch.Elapsed.Minutes,
                    stopWatch.Elapsed.Seconds, stopWatch.Elapsed.Milliseconds / 100);
            }
            return ElapsedTime;
        }
    }
}