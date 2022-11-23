using System;
namespace ProjetConsole
{
    public class Model
    {
        public string sourcePath { get; set; } = string.Empty;
        public string targetPath { get; set; } = string.Empty;
        public void saveFile()
        {
            string[] files = Directory.GetFiles(sourcePath);
            foreach (string sourceFile in files)
            {
                string fileName = Path.GetFileName(sourceFile);
                string destFile = Path.Combine(targetPath, fileName);
                File.Copy(sourceFile, destFile, true);
            }
        }

    }
}