using BaseProject.Application.Common.Exceptions;
using BaseProject.Domain.Configurations;
using BaseProject.API.Extensions;
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

    // Log that app is starting
    var port = configuration.AppUrl ?? "default port";
    Log.Information("Application running on port {Port}", port);
    Log.Information("Environment: {Environment}", builder.Environment.EnvironmentName);
    Log.Information("UseInMemoryDatabase: {UseInMemoryDatabase}", configuration.UseInMemoryDatabase);

    // Optional: log all configured URLs
    foreach (var url in app.Urls)
    {
        Log.Information("Listening on: {Url}", url);
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
