using BaseProject.Application.Common.Interfaces;
using Serilog;

namespace BaseProject.Infrastructure.Logging
{
    public class SerilogAppLogger : IAppLogger
    {
        public void Info(string message, params object[] properties) =>
            Log.Information(message, properties);

        public void Debug(string message, params object[] properties) =>
            Log.Debug(message, properties);

        public void Warning(string message, params object[] properties) =>
            Log.Warning(message, properties);

        public void Error(Exception? ex, string message, params object[] properties) =>
            Log.Error(ex, message, properties);
    }
}
