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

namespace BaseProject.API;

public static class ConfigureServices
{
    public static IServiceCollection AddWebAPIService(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddEndpointsApiExplorer();
        //services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.SetupMvc();
        if (appSettings.Identity.IsLocal)
        {
            services.AddAuthLocal(appSettings.Identity);
        }
        services.AddAuth(appSettings.Identity);
        services.AddDistributedMemoryCache();
        services.AddMemoryCache();
        services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

        //// Middleware
        //services.AddSingleton<ExceptionHandlingMiddleware>();
        //services.AddSingleton<PerformanceMiddleware>();
        //services.AddSingleton<Stopwatch>();

        // Extension classes
        services.AddHealthChecks();
        services.AddCompressionCustom();
        services.AddCorsCustom(appSettings);
        services.AddHttpClient();
        services.AddSwaggerOpenAPI(appSettings);
        services.SetupHealthCheck(appSettings);

        // Json configuration
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.WriteIndented = true;
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        return services;
    }
}
