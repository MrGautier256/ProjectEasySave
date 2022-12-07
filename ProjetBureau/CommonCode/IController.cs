namespace CommonCode
{
    /// <summary>
    /// Interface Icontroller avec comme méthode execute() commune a tout controller
    /// Icontroller interface with execute() as shared method to all the controllers
    /// </summary>
    public interface IController
    {
        public void execute();
        //public void execute(string sourcePath, string targetPath, string logType, string saveName);
    }
}