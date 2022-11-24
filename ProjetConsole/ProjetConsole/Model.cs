using Newtonsoft.Json;
using System;
using System.IO;
using TesJson;

namespace ProjetConsole
{
    public class Model
    {
        public string sourcePath { get; set; } = string.Empty;
        public string targetPath { get; set; } = string.Empty;
        public int countfile { get; set; }
        private List<JsonData>? TableLog { get; set; } = new List<JsonData>();
        public int TotalFileToCopy { get; set; }

        public void setTotalFileToCopy()
        {
            TotalFileToCopy = TotalFileToCopy = Directory.GetFiles(sourcePath, ".", SearchOption.AllDirectories).Length;
        }

        public void saveFile(string sourceDir, string targetDir)
        {
            var dir = new DirectoryInfo(sourceDir);
            int TotalFileToCopy;

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
                countfile++;
                string targetFilePath = Path.Combine(targetDir, file.Name);
                file.CopyTo(targetFilePath, true);

                JsonData jsonFileInfo = new JsonData(
                    file.Name,
                    file.FullName,
                    targetFilePath,
                    countfile,
                    this.TotalFileToCopy,
                    file.Length,
                    this.TotalFileToCopy - countfile,
                    (countfile * 100) / this.TotalFileToCopy
                    );
                TableLog.Add(jsonFileInfo);
            }

            // If there is subdirecories, copying subdirectories and recursively call this method
            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(targetDir, subDir.Name);
                saveFile(subDir.FullName, newDestinationDir);

            }
            TotalFileToCopy = 0;
        }

        public void writelog()
        {
            string json = JsonConvert.SerializeObject(TableLog.ToArray());

            System.IO.File.AppendAllText(@"C:\Users\Gautier\source\repos\ProjectEasySave\ProjetConsole\ProjetConsole\log.json", json);
        }

    }
}