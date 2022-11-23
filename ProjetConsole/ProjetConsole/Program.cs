using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ProjetTest
{
    public class Program
    {
        static void Main()
        {

            copy();
        }

        public static void copy()
        {
            string fileNameTest;
            string destFile;
            string sourcePath = @"C:\Users\Gautier\source\repos\CESI A3\ProjetConsole\ProjetConsole\Source";
            string targetPath = @"C:\Users\Gautier\source\repos\CESI A3\ProjetConsole\ProjetConsole\Destination";

            if (System.IO.Directory.Exists(sourcePath))
            {
                string[] files = System.IO.Directory.GetFiles(sourcePath);

                // Copy the files and overwrite destination files if they already exist.
                foreach (string sourceFile in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    fileName = System.IO.Path.GetFileName(sourceFile);
                    destFile = System.IO.Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(sourceFile, destFile, true);
                }
            }
            else
            {
                Console.WriteLine("Source path does not exist!");
            }

            // Keep console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}