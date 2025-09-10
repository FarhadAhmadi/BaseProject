using BaseProject.API.Middlewares;
using BaseProject.Application;
using BaseProject.Domain.Configurations;
using BaseProject.Infrastructure;
using BaseProject.Infrastructure.Persistence;
using CorrelationId;
using CorrelationId.DependencyInjection;
using Serilog;
using System.Diagnostics;

namespace BaseProject.API.Extensions
{
    public static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder, AppSettings appsettings)
        {
            // Add Correlation ID middleware
            builder.Services.AddDefaultCorrelationId(options =>
            {
                options.AddToLoggingScope = true;
                options.IncludeInResponse = true;
            });

            // Serilog configuration
            SerilogExtension.Configure(appsettings);
            builder.Host.UseSerilog();

            // Register services
            builder.Services.AddInfrastructuresService(appsettings);
            builder.Services.AddApplicationService(appsettings);
            builder.Services.AddWebAPIService(appsettings);

            return builder.Build();
        }

        public static async Task<WebApplication> ConfigurePipelineAsync(this WebApplication app, AppSettings appsettings)
        {
            // Push TraceId dynamically for every request
            app.Use(async (context, next) =>
            {
                var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
                using (Serilog.Context.LogContext.PushProperty("TraceId", traceId))
                {
                    await next();
                }
            });

            using var scope = app.Services.CreateScope();

            if (!appsettings.UseInMemoryDatabase)
            {
                var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
                await initializer.InitializeAsync();
            }

            // CorrelationId first (so everything downstream has a correlation id)
            app.UseCorrelationId();

            // Logging (wraps request + response consistently)
            app.UseMiddleware<LoggingMiddleware>();

            // Exception handling (so errors are caught inside logging)
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Performance (optional, order depends if you want errors to stop timing early or not)
            app.UseMiddleware<PerformanceMiddleware>();

            app.UseHttpsRedirection();
            app.UseCors("AllowSpecificOrigin");

            app.UseSwagger(appsettings);

            app.ConfigureHealthCheck();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}