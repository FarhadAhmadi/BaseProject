using BaseProject.API.Middlewares;
using BaseProject.Application;
using BaseProject.Domain.Configurations;
using BaseProject.Infrastructure;
using BaseProject.Infrastructure.Persistence;
using CorrelationId;
using CorrelationId.DependencyInjection;
using Serilog;

namespace BaseProject.API.Extensions
{
    public static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder, AppSettings appsettings)
        {
            Log.Information("Starting ConfigureServices...");

            // Add Correlation ID middleware
            builder.Services.AddDefaultCorrelationId(options =>
            {
                options.AddToLoggingScope = true;
                options.IncludeInResponse = true;
            });

            Log.Information("Correlation ID middleware configured.");

            // Serilog configuration
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithProcessId()
                .Enrich.WithCorrelationIdHeader()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"
                )
                .WriteTo.File(
                    path: "Logs/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    fileSizeLimitBytes: 10_000_000,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"
                )
                .CreateLogger();

            builder.Host.UseSerilog();
            Log.Information("Serilog configured.");

            builder.Services.AddInfrastructuresService(appsettings);
            builder.Services.AddApplicationService(appsettings);
            builder.Services.AddWebAPIService(appsettings);

            Log.Information("Services registered successfully.");

            return builder.Build();
        }

        public static async Task<WebApplication> ConfigurePipelineAsync(this WebApplication app, AppSettings appsettings)
        {
            Log.Information("Starting ConfigurePipelineAsync...");

            using var scope = app.Services.CreateScope();

            if (!appsettings.UseInMemoryDatabase)
            {
                Log.Information("Initializing database...");
                var initialize = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
                await initialize.InitializeAsync();
                Log.Information("Database initialized successfully.");
            }
            else
            {
                Log.Information("Using in-memory database. Skipping initialization.");
            }

            app.UseCorrelationId();
            Log.Information("Correlation ID middleware enabled.");

            // Serilog automatic request logging
            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
            });
            Log.Information("Serilog request logging enabled.");

            // Global Exception Middleware
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            Log.Information("Exception handling middleware configured.");

            // Logging and performance tracking middlewares
            app.UseMiddleware<LoggingMiddleware>();
            Log.Information("Custom logging middleware configured.");

            app.UseMiddleware<PerformanceMiddleware>();
            Log.Information("Performance middleware configured.");

            app.UseHttpsRedirection();
            Log.Information("HTTPS redirection enabled.");

            app.UseCors("AllowSpecificOrigin");
            Log.Information("CORS configured.");

            app.UseSwagger(appsettings);
            Log.Information("Swagger configured.");

            app.ConfigureHealthCheck();
            Log.Information("Health checks configured.");

            app.UseAuthentication();
            app.UseAuthorization();
            Log.Information("Authentication and Authorization enabled.");

            app.MapControllers();
            Log.Information("Controller routes mapped successfully.");

            Log.Information("Pipeline configuration complete.");
            return app;
        }
    }
}
