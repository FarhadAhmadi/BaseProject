using BaseProject.API.Extensions;
using BaseProject.API.Middlewares;
using BaseProject.Domain.Authorization;
using BaseProject.Domain.Configurations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Serilog;

namespace BaseProject.API;

public static class ConfigureServices
{
    public static IServiceCollection AddWebAPIService(this IServiceCollection services, AppSettings appSettings)
    {
        Log.Information("Configuring Web API services...");

        services.AddEndpointsApiExplorer();
        Log.Information("Endpoints API Explorer registered.");

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        Log.Information("FluentValidation configured.");

        services.SetupMvc();
        Log.Information("MVC configured.");

        if (appSettings.Identity.IsLocal)
        {
            services.AddAuthLocal(appSettings.Identity);
            Log.Information("Local authentication configured.");
        }
        else
        {
            services.AddAuth(appSettings.Identity);
            Log.Information("Authentication configured.");
        }


        services.AddDistributedMemoryCache();
        services.AddMemoryCache();
        Log.Information("Memory cache configured.");

        services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
        Log.Information("Authorization handler registered.");

        // Extension classes
        services.AddHealthChecks();
        Log.Information("Health checks registered.");

        services.AddCompressionCustom();
        Log.Information("Response compression configured.");

        services.AddCorsCustom(appSettings);
        Log.Information("CORS configured.");

        services.AddHttpClient();
        Log.Information("HttpClient registered.");

        services.AddSwaggerOpenAPI(appSettings);
        Log.Information("Swagger/OpenAPI configured.");

        services.SetupHealthCheck(appSettings);
        Log.Information("Health check endpoints configured.");

        // JSON configuration
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.WriteIndented = true;
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        Log.Information("JSON serializer options configured.");

        Log.Information("Web API services registration completed.");
        return services;
    }
}
