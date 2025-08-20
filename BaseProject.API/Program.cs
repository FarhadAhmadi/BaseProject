using BaseProject.API.Extensions;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Domain.Configurations;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // --- Step 1: Load AppSettings ---
    var configuration = builder.Configuration.Get<AppSettings>()
        ?? throw ProgramException.AppsettingNotSetException();

    builder.Services.AddSingleton(configuration);

    // --- Step 2: Configure services & build app ---
    var app = await builder.ConfigureServices(configuration)
                           .ConfigurePipelineAsync(configuration);

    // Resolve logger from DI
    var logger = app.Services.GetRequiredService<IAppLogger>();

    // Log that app is starting
    var port = configuration.AppUrl ?? "default port";
    logger.Info("Application running on port {Port}", port);
    logger.Info("Environment: {Environment}", builder.Environment.EnvironmentName);
    logger.Info("UseInMemoryDatabase: {UseInMemoryDatabase}", configuration.UseInMemoryDatabase);

    // Optional: log all configured URLs
    foreach (var url in app.Urls)
    {
        logger.Info("Listening on: {Url}", url);
    }

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

// for integration tests
public partial class Program { }
