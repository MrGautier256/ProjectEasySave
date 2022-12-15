using CommonCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TestUnitaires
{
    [TestClass]
    public class UTSaveFileService
    {
        private void CreateSourceandTarget(out string tempPathSource, out string tempPathTarget)
        {
            var masterDirectory = Path.Combine(Path.GetTempPath(), "_Save" + Guid.NewGuid().ToString());
            tempPathSource = Path.Combine(masterDirectory, "Source");
            tempPathTarget = Path.Combine(masterDirectory, "Destination");
            var dossierSource1 = Path.Combine(tempPathSource, "Dossier1");
            var sousDossierSource1 = Path.Combine(dossierSource1, "SousDossier1");
            var dossierSource2 = Path.Combine(tempPathSource, "Dossier2");

            Directory.CreateDirectory(tempPathSource);
            Directory.CreateDirectory(dossierSource1);
            Directory.CreateDirectory(sousDossierSource1);
            Directory.CreateDirectory(dossierSource2);
            Directory.CreateDirectory(tempPathTarget);

            FillDirectory(tempPathSource);
            FillDirectory(dossierSource1);
            FillDirectory(sousDossierSource1);
            FillDirectory(dossierSource2);
        }
        private string GenerateGarbage(int increment)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < increment; i++)
            {
                for (int j = 0; j < increment; j++)
                {
                    sb.AppendLine("test Unitaire Encrypt");
                }
            }
            return sb.ToString();
        }
        private void FillDirectory(string PathSource)
        {
            for (int i = 1; i < 20; i++)
            {
                File.WriteAllText(Path.Combine(PathSource, i + ".txt"), GenerateGarbage(i));
            }
        }
        [TestMethod]
        public void Teststart()
        {
            // Arrange
            string tempPathSource, tempPathTarget;
            CreateSourceandTarget(out tempPathSource, out tempPathTarget);

            SaveFileService savefileService = new(tempPathSource, tempPathTarget, "MaSauvegarde");
            bool finished = false;
            savefileService.Finished += (List<JsonData> arg1, bool arg2) => { finished = true; };

            // Act
            savefileService.Start();

            // Assert

            while (!finished)
            {
                Thread.Sleep(100);
            }

            var sourceDir = new DirectoryInfo(tempPathSource);
            var sourceFiles = sourceDir.GetFiles("*", SearchOption.AllDirectories);

            foreach (FileInfo file in sourceFiles)
            {
                string? fileDirectoryName = file.DirectoryName;
                string? targetFilePath = fileDirectoryName?.Replace(tempPathSource, tempPathTarget);
                string? filePath = Path.Combine(targetFilePath, file.Name);
                Assert.IsTrue(File.Exists(filePath), $"{filePath} n'existe pas");
            }

        }

        private class DataProgress
        {
            public override string ToString()
            {
                return $"{fileName} {countfile} {totalFileToCopy} {percentage}";
            }
            public string fileName = string.Empty;
            public double countfile;
            public int totalFileToCopy;
            public double percentage;
        }

        [TestMethod]
        public void TestSystemOfProgress()
        {
            // Arrange
            string tempPathSource, tempPathTarget;
            CreateSourceandTarget(out tempPathSource, out tempPathTarget);

            List<DataProgress> dataProgressList = new();

            SaveFileService savefileService = new(tempPathSource, tempPathTarget, "MaSauvegarde");
            bool finished = false;
            savefileService.Finished += (List<JsonData> arg1, bool arg2) => { finished = true; };

            savefileService.ProgressEvent += (string _fileName, int _countfile, int _totalFileToCopy, double _percentage) =>
            {
                var dataProgress = new DataProgress()
                { fileName = _fileName, countfile = _countfile, totalFileToCopy = _totalFileToCopy, percentage = _percentage };

                dataProgressList.Add(dataProgress);
                Debug.WriteLine(dataProgress.ToString());
            };
            // Act

            savefileService.Start();


            // Assert
            while (!finished)
            {
                Thread.Sleep(100);
            }
            Assert.AreEqual(76, dataProgressList.Count);
        }
    }
}
