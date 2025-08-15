using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Common.Mappings;
using BaseProject.Application.Services;
using BaseProject.Domain.Configurations;
using BaseProject.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BaseProject.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services, AppSettings appsettings)
    {
        services.AddAutoMapper(typeof(MapProfile).Assembly);

        services.AddTransient<IUserContext, UserContext>();
        services.AddTransient<IAuthService, AuthService>();
        //services.AddTransient<IBookService, BookService>();
        //services.AddTransient<IAuthorService, AuthorService>();
        //services.AddTransient<IPublisherService, PublisherService>();
        //services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<IMailService, MailService>();
        services.AddTransient<IMediaService, MediaService>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IAuthIdentityService, AuthIdentityService>();

        if (appsettings.FileStorageSettings.LocalStorage)
        {
            services.AddSingleton<IFileService, LocalStorageService>();
        }
        else
        {
            services.AddSingleton<IFileService, CloudinaryStorageService>();
        }

        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ICurrentTime, CurrentTime>();
        services.AddScoped<ICurrentUser, CurrentUser>();

        return services;
    }
}
