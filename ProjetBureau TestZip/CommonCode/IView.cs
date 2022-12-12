namespace CommonCode
{
    public enum LangueEnum { english, french, spanish };
    public interface IView
    {
        string typeOfMode { get; }

        LangueEnum AskLanguage();
        string AsklogType();
        string AskSourcePath();
        string AskTargetFile();
        string AskTargetPath();
        void Progress(bool v);
        void ControlProgress(string fileName, double countfile, int totalFileToCopy, double percentage, bool copy);
        void SourcePathIsInvalid();
        void TargetDirInvalid();
        void TargetPathIsInvalid();
        void SendSaveFileServiceCommand(ISaveFileServiceCommand saveFileService);
    }
}
