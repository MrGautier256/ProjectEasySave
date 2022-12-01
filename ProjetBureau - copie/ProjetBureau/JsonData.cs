using System;

namespace ProjetBureau
{   
    /// <summary>
    /// Classe contenant tout les attributs qui seront enregistré dans le fichier .Log
    /// Class containing all the attributes who will be saved in the .Log file
    /// </summary>
    
    public class JsonData
    {
        public JsonData(string _SaveName, 
            string _FileName, 
            string _SourceFilePath, 
            string _TargetFilePath, 
            double _FileNumber, 
            int _TotalFilesToCopy, 
            long _filesSize, 
            double _NbFilesLeftToDo, 
            double _Progression, 
            string _Duration) 
        {
            SaveName = _SaveName;
            FileName = _FileName;
            SourceFilePath = _SourceFilePath;    
            TargetFilePath = _TargetFilePath;    
            FileNumber = Math.Truncate(_FileNumber);
            TotalFilesToCopy = _TotalFilesToCopy;
            FilesSize = _filesSize;
            NbFilesLeftToDo = Math.Truncate(_NbFilesLeftToDo); 
            Progression =  Math.Round(_Progression, 2, MidpointRounding.AwayFromZero);
            TimeStamp = DateTime.Now.ToString("MM/dd/yyyy HH’:’mm’:’ss");
            Duration = _Duration;
        }
        public string SaveName { get; set; }
        public string FileName { get; set; }
        public string SourceFilePath { get; set; }
        public string TargetFilePath { get; set; }
        public double FileNumber { get; set; }
        public double TotalFilesToCopy { get; set; }
        public long FilesSize { get; set; }
        public double NbFilesLeftToDo { get; set; }
        public double Progression { get; set; }
        public string TimeStamp { get; set; }
        public string Duration { get; set; }

    }

}