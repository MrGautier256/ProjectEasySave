namespace CommonCode
{
    public enum LangueEnum { english, french, spanish };

    public interface IViewProgress
    {
        void Progress(bool v);
        void ControlProgress(string fileName, int countfile, int totalFileToCopy, double percentage);
    }


    public interface IView : IViewProgress
    {
        string typeOfMode { get; }

        LangueEnum AskLanguage();
        string AsklogType();
        string AskSourcePath();
        string AskTargetFile();
        string AskTargetPath();
        void SourcePathIsInvalid();
        void TargetDirInvalid();
        void TargetPathIsInvalid();
        void SendSaveFileServiceCommand(ISaveFileServiceCommand saveFileService);
    }
}
