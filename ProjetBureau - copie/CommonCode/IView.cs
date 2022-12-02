namespace CommonCode
{
    public enum langueEnum { english, french, spanish };
    public interface IView
    {
        string typeOfMode { get; }

        langueEnum askLanguage();
        string asklogType();
        string askSourcePath();
        string askTargetFile();
        string askTargetPath();
        void progress(bool v);
        void sendProgressInfoToView(string fileName, double countfile, int totalFileToCopy, double percentage);
        void sourcePathIsInvalid();
        void targetDirInvalid();
        void targetPathIsInvalid();
    }
}
