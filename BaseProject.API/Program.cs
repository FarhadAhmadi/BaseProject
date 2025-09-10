using BaseProject.API.Extensions;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Domain.Configurations;
using Serilog;

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    // --- Step 1: Load AppSettings ---
    AppSettings configuration = builder.Configuration.Get<AppSettings>()
        ?? throw ProgramException.AppsettingNotSetException();

    builder.Services.AddSingleton(configuration);

    // --- Step 2: Configure services & build app ---
    WebApplication app = await builder.ConfigureServices(configuration)
                           .ConfigurePipelineAsync(configuration);

    // Resolve logger from DI
    IAppLogger logger = app.Services.GetRequiredService<IAppLogger>();

    // Log that app is starting
    string port = configuration.AppUrl ?? "default port";
    logger.Info("Application running on port {Port}", port);
    logger.Info("Environment: {Environment}", builder.Environment.EnvironmentName);
    logger.Info("UseInMemoryDatabase: {UseInMemoryDatabase}", configuration.UseInMemoryDatabase);

    // Optional: log all configured URLs
    foreach (string url in app.Urls)
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
