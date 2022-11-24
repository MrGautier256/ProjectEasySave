        }
﻿namespace TesJson
{
    public class JsonData
    {
        public JsonData(string _Name, string _SourceFilePath, string _TargetFilePath, int _FileNumber, 
            int _TotalFilesToCopy, long _TotalFilesSize, int _NbFilesLeftToDo, float _Progression, string _Duration) 
        {
            Name= _Name;
            SourceFilePath= _SourceFilePath;    
            TargetFilePath= _TargetFilePath;    
            FileNumber= _FileNumber;
            TotalFilesToCopy= _TotalFilesToCopy;
            TotalFilesSize= _TotalFilesSize;
            NbFilesLeftToDo= _NbFilesLeftToDo;
            Progression = _Progression;
            TimeStamp = DateTime.Now.ToString("MM/dd/yyyy HH’:’mm’:’ss");
            Duration = _Duration;
        }
        public string Name { get; set; }
        public string SourceFilePath { get; set; }
        public string TargetFilePath { get; set; }
        public int FileNumber { get; set; }
        public int TotalFilesToCopy { get; set; }
        public long TotalFilesSize { get; set; }
        public int NbFilesLeftToDo { get; set; }
        public float Progression { get; set; }
        public string TimeStamp { get; set; }
        public string Duration { get; set; }



    }

}