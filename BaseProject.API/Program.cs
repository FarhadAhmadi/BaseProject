using BaseProject.Application.Common.Exceptions;
using BaseProject.Domain.Configurations;
using BaseProject.API.Extensions;
using Serilog;
using CorrelationId.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// --- Step 1: Load AppSettings ---
var configuration = builder.Configuration.Get<AppSettings>()
    ?? throw ProgramException.AppsettingNotSetException();

builder.Services.AddSingleton(configuration);

// --- Step 2: Configure services & build app ---
var app = await builder.ConfigureServices(configuration)
                       .ConfigurePipelineAsync(configuration);

await app.RunAsync();

// for integration tests
public partial class Program { }
