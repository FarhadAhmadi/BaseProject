using BaseProject.Domain.Configurations;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System.Diagnostics;

namespace BaseProject.API.Extensions
{
    public static class SerilogExtension
    {
        public static void Configure(AppSettings appSettings)
        {
            Log.Logger = new LoggerConfiguration()
                // Minimum log levels
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)

                // Enrich logs with context
                .Enrich.FromLogContext()                  // For LogContext.PushProperty
                .Enrich.WithMachineName()                 // Machine name
                .Enrich.WithThreadId()                    // Thread ID
                .Enrich.WithProcessId()                   // Process ID
                .Enrich.WithCorrelationIdHeader()         // Correlation ID from middleware
                .Enrich.WithProperty("TraceId", Activity.Current?.Id) // Trace ID from Activity

                // Console sink
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"
                )

                // File sink with rolling files
                .WriteTo.File(
                    path: "Logs/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    fileSizeLimitBytes: 10_000_000,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"
                )

                // Optional: JSON file for structured logging (good for ELK/Seq)
                .WriteTo.File(
                    new JsonFormatter(),
                    path: "Logs/log.json",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    shared: true
                )

                .CreateLogger();
        }
    }

}
