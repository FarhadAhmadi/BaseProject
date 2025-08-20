namespace BaseProject.Application.Common.Interfaces
{
    public interface IAppLogger
    {
        void Info(string message, params object[] properties);
        void Debug(string message, params object[] properties);
        void Warning(string message, params object[] properties);
        void Error(Exception? ex, string message, params object[] properties);
    }

}
