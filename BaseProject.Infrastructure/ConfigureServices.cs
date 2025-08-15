using BaseProject.Application.Common.Interfaces;
using BaseProject.Domain.Configurations;
using BaseProject.Domain.Entities;
using BaseProject.Domain.Interfaces;
using BaseProject.Infrastructure.Common.Utilities;
using BaseProject.Infrastructure.Persistence;
using BaseProject.Infrastructure.Persistence.Repositories;
using BaseProject.Infrastructure.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BaseProject.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructuresService(this IServiceCollection services, AppSettings configuration)
    {
        Log.Information("Configuring database...");

        if (configuration.UseInMemoryDatabase)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("CleanArchitecture"));
            Log.Information("Using in-memory database.");
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.ConnectionStrings.DefaultConnection,
                sqlOptions => sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null)));
        }

        Log.Information("database Configured successfully.");

        Log.Information("Configuring Identity...");
        services.AddIdentity<ApplicationUser, RoleIdentity>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        Log.Information("Identity configured successfully.");

        Log.Information("Registering repositories and services...");
        services.AddScoped<IUserRepository, UserRepository>();
        //services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IMediaRepository, MediaRepository>();
        services.AddScoped<IForgotPasswordRepository, ForgotPasswordRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient<ApplicationDbContextInitializer>();

        services.AddScoped<SqlDapperContext>();

        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<ICookieService, CookieService>();

        Log.Information("Repositories and services registered successfully.");

        return services;
    }
}
