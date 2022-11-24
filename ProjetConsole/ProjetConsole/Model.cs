using System;
using System.IO;

namespace ProjetConsole
{
    public class Model
    {
        public string sourcePath { get; set; } = string.Empty;
        public string targetPath { get; set; } = string.Empty;
        public void saveFile(string sourceDir, string targetDir)
        {
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories in a array
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(targetDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(targetDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If there is subdirecories, copying subdirectories and recursively call this method
            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(targetDir, subDir.Name);
                saveFile(subDir.FullName, newDestinationDir);

            }
        }

    }
}