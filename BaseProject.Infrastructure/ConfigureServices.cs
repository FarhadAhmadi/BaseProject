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

namespace BaseProject.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructuresService(this IServiceCollection services, AppSettings configuration)
    {
        services.AddScoped<AuditInterceptor>();

        if (configuration.UseInMemoryDatabase)
        {
            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                var interceptor = sp.GetRequiredService<AuditInterceptor>();
                options.UseInMemoryDatabase("CleanArchitecture")
                       .AddInterceptors(interceptor);
            });
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                var interceptor = sp.GetRequiredService<AuditInterceptor>();

                options.UseSqlServer(
                    configuration.ConnectionStrings.DefaultConnection,
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null
                    )
                ).AddInterceptors(interceptor);
            });
        }

        services.AddIdentity<ApplicationUser, RoleIdentity>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

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

        return services;
    }
}
