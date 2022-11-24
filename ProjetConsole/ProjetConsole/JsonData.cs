namespace TesJson
{
    public class JsonData
    {
        public JsonData(string _Name, string _SourceFilePath, string _TargetFilePath, int _FileNumber, 
            int _TotalFilesToCopy, long _TotalFilesSize, int _NbFilesLeftToDo, float _Progression) 
        {
            Name= _Name;
            SourceFilePath= _SourceFilePath;    
            TargetFilePath= _TargetFilePath;    
            FileNumber= _FileNumber;
            TotalFilesToCopy= _TotalFilesToCopy;
            TotalFilesSize= _TotalFilesSize;
            NbFilesLeftToDo= _NbFilesLeftToDo;
            Progression = _Progression;
        }
        public string Name { get; set; }
        public string SourceFilePath { get; set; }
        public string TargetFilePath { get; set; }
        public int FileNumber { get; set; }
        public int TotalFilesToCopy { get; set; }
        public long TotalFilesSize { get; set; }
        public int NbFilesLeftToDo { get; set; }
        public float Progression { get; set; }

    }

}